using System.Text;
using System.Text.Json;
using Google.Protobuf.WellKnownTypes;

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
        try
        {
            await ValidateRequest(context);

            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
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

        return context.Response.WriteAsJsonAsync(res);
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
        }
        else
        {
            throw new ArgumentException("Unsupported content type : {context.Request.ContentType}");
        }

        context.Request.Body.Position = 0;
    } 
}
