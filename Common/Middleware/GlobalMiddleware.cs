using System.Text;
using System.Text.Json;

namespace EmbeddingCloudRun;

public class GlobalMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalMiddleware> _logger;

    public GlobalMiddleware(RequestDelegate next, ILogger<GlobalMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalResponseBody = context.Response.Body;
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await ValidateRequest(context);

            await _next(context);

            await LoggingApiResponse(context);

            context.Response.Body.Position = 0;

            await responseBody.CopyToAsync(originalResponseBody);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        ApiResponse res;
        
        switch (ex)
        {
            case UnauthorizedAccessException:
                res = new ApiResponse
                {
                    resultCode = "U401",
                    resultMessage = "Unauthorized access.",
                    errorMessage = ex.Message
                };

                context.Response.StatusCode = 401;

                break;
            case ArgumentException:
                res = new ApiResponse
                {
                    resultCode = "U400",
                    resultMessage = "Bad request.",
                    errorMessage = ex.Message
                };

                context.Response.StatusCode = 400;

                break;
            default:
                res = new ApiResponse
                {
                    resultCode = "U500",
                    resultMessage = "Internal server error.",
                    errorMessage = ex.Message
                };

                context.Response.StatusCode = 500;

                break;
        }

        await context.Response.WriteAsJsonAsync(res);
    }

    private async Task ValidateRequest(HttpContext context)
    {
        // Read Request Content
        context.Request.EnableBuffering();

        if (context.Request.HasFormContentType)
        {
            var form = await context.Request.ReadFormAsync();

            if (form.Files.Count == 0)
            {
                throw new ArgumentException("File is missing in the request.");
            }

            // 檢查文字欄位
            var senderCode = form["header.senderCode"];
            if (string.IsNullOrWhiteSpace(senderCode))
            {
                throw new ArgumentException("Missing senderCode.");
            }

            _logger.LogInformation("header.senderCode: {senderCode}, filename: {filename}, contentType: {contentType}", senderCode, form.Files[0].FileName, form.Files[0].ContentType);
        } 
        else if (context.Request.HasJsonContentType())
        {
            ApiRequest<ApiResponse>? requestBody;
            string? requestBodyString;

            using var reader = new StreamReader(
                context.Request.Body, 
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true
            );

            requestBodyString = await reader.ReadToEndAsync();

            requestBody = JsonSerializer.Deserialize<ApiRequest<ApiResponse>>(requestBodyString);

            // Validate Request Header
            if (requestBody?.header == null)
            {
                throw new ArgumentException("Missing request header.");
            }

            if (requestBody.header.senderCode == null || requestBody.header.senderCode.Trim() == "")
            {
                throw new ArgumentException("Missing senderCode in request header.");
            } 

            // Validate Request Body
            if (requestBody.body == null) {
                throw new ArgumentException("Missing request body.");
            }

            _logger.LogInformation("FullRoute: {route}, request: {request}", context.Request.Path, requestBodyString);
        }
        else
        {
            throw new ArgumentException("Unsupported content type : {context.Request.ContentType}");
        }

        context.Request.Body.Position = 0;
    } 

    private async Task LoggingApiResponse(HttpContext context)
    {
        context.Response.Body.Position = 0; 

        // Read Response Content
        using var reader = new StreamReader(
            context.Response.Body,
            Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true
        );

        var responseBody = await reader.ReadToEndAsync();

        context.Response.Body.Position = 0; 

        _logger.LogInformation("FullRoute: {route}, response: {response}", context.Request.Path, responseBody);        
    }
}
