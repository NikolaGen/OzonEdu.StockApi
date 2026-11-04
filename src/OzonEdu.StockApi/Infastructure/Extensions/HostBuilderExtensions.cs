using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OzonEdu.StockApi.Configuration.Middlewares;
using OzonEdu.StockApi.Infastructure.Filters;
using OzonEdu.StockApi.Infastructure.StartupFilters;
using Microsoft.Extensions.DependencyInjection;

namespace OzonEdu.StockApi.Infastructure.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddInfastructure(this IHostBuilder builder)
        {
            builder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(options =>
                {
                    // HTTP/1.1 для REST и Swagger
                    options.ListenAnyIP(5000, o => o.Protocols = HttpProtocols.Http1);

                    // gRPC (HTTP/2) — порт 5002, как в docker-compose
                    options.ListenAnyIP(5002, o => o.Protocols = HttpProtocols.Http2);
                });
            });
            
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IStartupFilter, SwaggerStartupFilter>();
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "OzonEdu.StockApi", Version = "v1" });
                
                    options.CustomSchemaIds(x => x.FullName);
                
                    var xmlFileName = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
                    var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                    options.IncludeXmlComments(xmlFilePath);
                });
                services.AddGrpc(options => options.Interceptors.Add<LoggingInterceptor>());
                services.AddGrpcReflection();
            });
            
            return builder;
        }

        public static IHostBuilder AddHttp(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
            });
            return  builder;
        }
    }
}