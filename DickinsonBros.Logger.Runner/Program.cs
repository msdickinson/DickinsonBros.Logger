using DickinsonBros.Logger.Abstractions;
using DickinsonBros.Logger.Extensions;
using DickinsonBros.Logger.Runner.Services;
using DickinsonBros.Redactor.Extensions;
using DickinsonBros.Redactor.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DickinsonBros.Logger.Runner
{
    class Program
    {
        IConfiguration _configuration;
        async static Task Main()
        {
            await new Program().DoMain();
        }
        async Task DoMain()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConsole();
            });

            loggerFactory.CreateLogger<Program>();

            try
            {
                using (var applicationLifetime = new ApplicationLifetime())
                {
                    var services = InitializeDependencyInjection();
                    ConfigureServices(services, applicationLifetime);

                    using (var provider = services.BuildServiceProvider())
                    {
                        var loggingService = provider.GetRequiredService<ILoggingService<Program>>();

                        var data = new Dictionary<string, object>
                                   {
                                       { "Username", "DemoUser" },
                                       { "Password",
@"{
    ""Password"": ""password""
}"
                                       }
                                   };

                        var message = "Generic Log Message";
                        var exception = new Exception("Error");

                        loggingService.LogDebugRedacted(message);
                        loggingService.LogDebugRedacted(message, data);

                        loggingService.LogInformationRedacted(message);
                        loggingService.LogInformationRedacted(message, data);

                        loggingService.LogWarningRedacted(message);
                        loggingService.LogWarningRedacted(message, data);

                        loggingService.LogErrorRedacted(message, exception);
                        loggingService.LogErrorRedacted(message, exception, data);
                        applicationLifetime.StopApplication();

                        provider.ConfigureAwait(true);
                    }
                }
                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("End...");
                Console.ReadKey();
            }
        }

        private void ConfigureServices(IServiceCollection services, ApplicationLifetime applicationLifetime)
        {
            services.AddOptions();
            services.AddLogging(config =>
            {
                config.AddConfiguration(_configuration.GetSection("Logging"));

                if (Environment.GetEnvironmentVariable("BUILD_CONFIGURATION") == "DEBUG")
                {
                    config.AddConsole();
                }
            });

            services.AddSingleton<IApplicationLifetime>(applicationLifetime);
            services.AddLoggingService();
            services.AddRedactorService();
            services.Configure<RedactorServiceOptions>(_configuration.GetSection(nameof(RedactorServiceOptions)));
        }

        IServiceCollection InitializeDependencyInjection()
        {
            var aspnetCoreEnvironment = Environment.GetEnvironmentVariable("BUILD_CONFIGURATION");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{aspnetCoreEnvironment}.json", true);
            _configuration = builder.Build();
            var services = new ServiceCollection();
            services.AddSingleton(_configuration);
            return services;
        }
    }
}