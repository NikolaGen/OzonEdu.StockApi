using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OzonEdu.StockApi.Configuration.Middlewares;
using OzonEdu.StockApi.Grpc;
using OzonEdu.StockApi.GrpcServices;
using OzonEdu.StockApi.HttpClients;
using OzonEdu.StockApi.Infastructure.Extensions;
using OzonEdu.StockApi.Services;

namespace OzonEdu.StockApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IStockService, StockService>();
            services.AddInfastructure();
            //services.AddHttpClient<IStockHttpClient, StockHttpClient>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.Map("/version", builder => builder.UseMiddleware<VersionMiddleware>());
            app.Map("/ready", builder => builder.UseMiddleware<ReadyLiveMiddleware>());
            app.Map("/live", builder => builder.UseMiddleware<ReadyLiveMiddleware>());

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<StockApiGrpcService>();
                endpoints.MapControllers();
                endpoints.MapGrpcReflectionService();
            });
        }
    }

    public interface ILoggingService
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class CustomHostedService : IHostedService, IDisposable
    {
        private Timer _timer;

        // private readonly ILoggingService _loggingService;
        private readonly IServiceProvider _serviceProvider;

        public CustomHostedService(IServiceProvider serviceProvider)
        {
            // _loggingService = loggingService;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();

            async void Callback(object _)
            {
                try
                {
                    loggingService.Log("IHostedService tick");
                    await Task.Delay(100, cancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            _timer = new Timer(Callback, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

    public class CustomBackGroundService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while
                (!stoppingToken
                     .IsCancellationRequested) // до тех пор пока не прийдет завершение, пока НЕ пришла команда остановиться — крутись
            {
                Console.WriteLine("IHostedService tick");
                try
                {
                    await Task.Delay(100, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break; // пришла отмена — выходим из цикла аккуратно
                } // пауза на 100 миллисекунд, но «умная»: поток не блокируется, он свободен делать другое. await = «подожди здесь, но не тормози весь поток».
            }
        }
    }
}