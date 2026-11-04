using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace OzonEdu.StockApi.Configuration.Middlewares
{
/**/    /*public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context) // HttpContext - вся информация о текущем запросе; читается так: «метод, который фреймворк вызывает для обработки запроса». метод, который в качестве параметра принимает всю инф о тек запросе. Invoke переводится как «вызвать», «обратиться», «запустить».  
        {
            await LogRequest(context);
            await  _next(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            try
            {
                if (context.Request.ContentLength > 0)
                {
                    context.Request.EnableBuffering();

                    /*var buffer = new byte[context.Request.ContentLength.Value];
                    await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                    var bodyAsText = Encoding.UTF8.GetString(buffer);#1# // Создай переменную bodyAsText. Пусть её тип компилятор выведет сам. Значение — результат декодирования массива байтов buffer с помощью кодировки UTF-8. То есть преврати байты в текст
                    string bodyAsText;
                    using (var reader = new StreamReader(
                               context.Request.Body,
                               Encoding.UTF8,
                               detectEncodingFromByteOrderMarks: false,
                               bufferSize: 1024,
                               leaveOpen: true))
                    {
                        bodyAsText = await reader.ReadToEndAsync();
                    }
                    _logger.LogInformation("Request logged");
                    _logger.LogDebug(bodyAsText);

                    context.Request.Body.Position = 0;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not request body");
            }*/
    public class RequestResponseLoggingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 1. Исключаем gRPC
            var contentType = context.Request.ContentType;
            if (contentType != null && contentType.StartsWith("application/grpc"))
            {
                await _next(context);
                return;
            }

            // 2. Логируем request (метод, путь, заголовки, тело)
            await LogRequest(context);

            // 3. Пропускаем дальше
            await _next(context);

            // 4. Логируем response (статус, заголовки, route)
            LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            _logger.LogInformation("REQUEST {Method} {Path}",
                context.Request.Method, context.Request.Path);

            foreach (var header in context.Request.Headers)
            {
                _logger.LogInformation("  Header {Key}: {Value}",
                    header.Key, header.Value.ToString());
            }

            await LogRequestBody(context);
        }

        private async Task LogRequestBody(HttpContext context)
        {
            if (context.Request.ContentLength == null || context.Request.ContentLength.Value == 0)
            {
                return;
            }

            try
            {
                context.Request.EnableBuffering();

                string bodyAsText;
                using (var reader = new StreamReader(
                           context.Request.Body,
                           Encoding.UTF8,
                           detectEncodingFromByteOrderMarks: false,
                           bufferSize: 1024,
                           leaveOpen: true))
                {
                    bodyAsText = await reader.ReadToEndAsync();
                }

                context.Request.Body.Position = 0;

                _logger.LogInformation("  Body: {Body}", bodyAsText);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not read request body");
            }
        }

        private void LogResponse(HttpContext context)
        {
            _logger.LogInformation("RESPONSE {StatusCode}", context.Response.StatusCode);

            foreach (var header in context.Response.Headers)
            {
                _logger.LogInformation("  Header {Key}: {Value}",
                    header.Key, header.Value.ToString());
            }

            var endpoint = context.GetEndpoint() as RouteEndpoint;
            string route;
            if (endpoint != null)
                route = endpoint.RoutePattern.RawText;
            else
                route = context.Request.Path.ToString();

            _logger.LogInformation("  Route: {Route}", route);
        }
    }

    public class LoggingInterceptor : Interceptor
    {
        private readonly ILogger<LoggingInterceptor> _logger;

        public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
        {
            _logger = logger;
        }
        
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            // 1. Логируем запрос
            _logger.LogInformation("Request: {Request}", JsonSerializer.Serialize(request));

            // 2. Ждём реальный ответ (разворачиваем Task в сам объект)
            var response = await continuation(request, context);

            // 3. Логируем ответ — теперь это настоящий объект, а не Task
            _logger.LogInformation("Response: {Response}", JsonSerializer.Serialize(response));

            return response;
        }

        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            _logger.LogInformation("Streaming has been called");
            return  base.AsyncServerStreamingCall(request, context, continuation);
        }
    }
}