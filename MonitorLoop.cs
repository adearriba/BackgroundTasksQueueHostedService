using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BackgroundTasksQueueHostedService
{
    public class MonitorLoop
    {
        private readonly Channel<IBackgroundTask> _channel;
        private readonly ILogger _logger;
        private readonly CancellationToken _cancellationToken;
        private IServiceProvider _serviceProvider;

        public MonitorLoop(Channel<IBackgroundTask> channel, 
            ILogger<MonitorLoop> logger, 
            IServiceProvider serviceProvider,
            IHostApplicationLifetime applicationLifetime)
        {
            _channel = channel;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _cancellationToken = applicationLifetime.ApplicationStopping;
        }

        public void StartMonitorLoop()
        {
            _logger.LogInformation("Monitor Loop is starting.");
            Task.Run(() => Monitor());
        }

        public void Monitor()
        {
            while (!_cancellationToken.IsCancellationRequested
                    && !_channel.Reader.Completion.IsCompleted)
            {
                var keyStroke = Console.ReadKey();

                if (keyStroke.Key == ConsoleKey.W)
                {
                    _channel.Writer.WriteAsync(new BackgroundTask(_serviceProvider));
                }
            }
        }
    }
}