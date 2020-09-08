using System.Threading;
using System.Threading.Tasks;

namespace BackgroundTasksQueueHostedService
{
    public interface IBackgroundTask<T>
    {
        Task<T> DoWork(CancellationToken token = default);
    }

    public interface IBackgroundTask
    {
        Task DoWork(CancellationToken token = default);
    }
}