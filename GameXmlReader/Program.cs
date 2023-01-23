using GameXmlReader.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using OsborneSupremacy.Extensions.AspNet;
using GameXmlReader.Models;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var settings = configuration
    .GetAndValidateTypedSection("Settings", new SettingsValidator());

await Host.CreateDefaultBuilder()
    .ConfigureServices((hostContent, services) => {
        services.AddHostedService<ConsoleHostedService>();
        services.AddSingleton(settings);
        services.AddSingleton<ExecutorService>();
        services.AddSingleton<GameXmlService>();
        services.AddSingleton<GameScanner>();
    })
    .UseSerilog()
    .RunConsoleAsync();
