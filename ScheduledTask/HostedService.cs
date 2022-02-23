using System;
using System.Threading;
using System.Threading.Tasks;
using Covid_Api.Data;
using Covid_Api.Methods;
using Covid_Api.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Covid_Api.ScheduledTask
{
    public abstract class HostedService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;

        public HostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        private Task currentTask;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        protected abstract Task ExecuteAsync(CancellationToken cToken);

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            currentTask = ExecuteAsync(cancellationTokenSource.Token);

            if (currentTask.IsCompleted)
                return currentTask;

            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<CovidAppContext>();
                AppMethods.LogDb(new Log() { Message = "StopAsync", Exception = string.Empty, Date = DateTime.Now }, context);

            }

            if (currentTask == null)
                return;


            try
            {
                cancellationTokenSource.Cancel();
            }
            finally
            {
                await Task.WhenAny(currentTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }
        public virtual void Dispose()
        {
            cancellationTokenSource.Cancel();
        }
    }



}