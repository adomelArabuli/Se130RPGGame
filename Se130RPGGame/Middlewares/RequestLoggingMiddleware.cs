using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;

namespace Se130RPGGame.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public RequestLoggingMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var queryParams = request.Query;
            var requestBody = request.Body;

            if (queryParams.Any())
            {
                Console.WriteLine("Query params");
                foreach (var param in queryParams.Keys)
                {
                    var values = queryParams[param];
                    foreach (var value in values)
                    {
                        Console.WriteLine($"{param}: {value}");
                    }
                }
            }
            else if(requestBody != null)
            {
                context.Request.EnableBuffering();

                var body = await new StreamReader(requestBody).ReadToEndAsync();

                context.Request.Body.Position = 0;

                if (!string.IsNullOrEmpty(body))
                {
                    Console.WriteLine("Request body:");
                    var jsonBody = JObject.Parse(body);
                    foreach (var prop in jsonBody.Properties())
                    {
                        Console.WriteLine($"{prop.Name} / {prop.Value}");
                    }
                }
            }
            else
            {
                Console.WriteLine("No query params found");
            }

            await _requestDelegate(context);

            var response = context.Response;
            Console.WriteLine($"Response: {response.StatusCode}");
        }
    }
}
