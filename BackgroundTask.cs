using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BackgroundTasksQueueHostedService{
    public class BackgroundTask : IBackgroundTask
    {
        private IServiceProvider _serviceProvider;

        public BackgroundTask(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task DoWork(CancellationToken token = default)
        {
            using(var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetService<ILogger<BackgroundTask>>();
                int delayLoop = 0;
                var guid = Guid.NewGuid().ToString();

                logger.LogInformation(
                    "Queued Background Task {Guid} is starting.", guid);

                while (!token.IsCancellationRequested && delayLoop < 3)
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5), token);
                    }
                    catch (OperationCanceledException)
                    {
                        // Prevent throwing if the Delay is cancelled
                    }

                    delayLoop++;

                    logger.LogInformation(
                        "Queued Background Task {Guid} is running. " +
                        "{DelayLoop}/3", guid, delayLoop);
                }

                if (delayLoop == 3)
                {
                    logger.LogInformation(
                        "Queued Background Task {Guid} is complete.", guid);
                }
                else
                {
                    logger.LogInformation(
                        "Queued Background Task {Guid} was cancelled.", guid);
                }    
            }
        }
    }
}
