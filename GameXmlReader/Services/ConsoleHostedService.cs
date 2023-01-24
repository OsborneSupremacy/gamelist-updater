using Microsoft.Extensions.Hosting;
using System.Threading;

namespace GameXmlReader.Services;

public class ConsoleHostedService : IHostedService
{
    private readonly IHostApplicationLifetime _applicationLifetime;

    private readonly ILogger<ConsoleHostedService> _logger;

    private readonly ExecutorService _executorService;

    public ConsoleHostedService(
        IHostApplicationLifetime applicationLifetime,
        ILogger<ConsoleHostedService> logger,
        ExecutorService executorService
        )
    {
        _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _executorService = executorService ?? throw new ArgumentNullException(nameof(executorService));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _applicationLifetime.ApplicationStarted.Register(async () =>
        {
            try
            {
                _logger.LogInformation("Hello. Welcome to Gamelist Updater");
                await _executorService.ExecuteAsync(cancellationToken);
            } catch (Exception ex)
            {
                _logger.LogCritical("Critical exception encountered. Application shutting down.", ex);
#if DEBUG
                _logger.LogCritical(ex.ToString());
#endif
            } finally
            {
                _logger.LogInformation("Application shutting down.");
                _applicationLifetime.StopApplication();
            }
        });
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => 
        Task.CompletedTask;
}
