using EVotingSystem.DataBase;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EVotingSystem.Services
{
    public class CandidateResultMonitor : IHostedService, IDisposable
    {
        #region "Fields"
        private Timer TimerObj;
        private readonly ILogger<CandidateResultMonitor> Logger;
        private FireStoreManager FireStore;
        #endregion

        public CandidateResultMonitor(ILogger<CandidateResultMonitor> Logger)
        {
            this.Logger = Logger;
            Initialize();
        }

        #region "Overloaded Methods"
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("CandidateResultMonitor: StartAsync.");
            TimerObj = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("CandidateResultMonitor: StopAsync.");
            TimerObj?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            Logger.LogInformation("CandidateResultMonitor: Dispose.");
            TimerObj?.Dispose();
        }

        #endregion

        #region "Private Methods"
        private void Initialize()
        {
            TimerObj = null;
            FireStore = new FireStoreManager();
        }
        private void DoWork(object state)
        {
            Logger.LogInformation("CandidateResultMonitor: DoWork.");
        }
        #endregion
    }
}