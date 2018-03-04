using System;
using System.Threading;
using Xamarin.Forms;

namespace HealthManager.ViewModel.Util
{
    public class Timer
    {
        private static CancellationTokenSource _cancellationTokenSource;
        private readonly Action _callback;
        private readonly TimeSpan _timeSpan;

        public Timer(TimeSpan timeSpan, Action callback)
        {
            _timeSpan = timeSpan;
            _callback = callback;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            var cts = _cancellationTokenSource; // safe copy
            Device.StartTimer(_timeSpan, () =>
            {
                if (cts.IsCancellationRequested) return false;
                _callback.Invoke();
                return true; //true to continuous, false to single use
            });
        }

        public void Stop()
        {
            Interlocked.Exchange(ref _cancellationTokenSource, new CancellationTokenSource()).Cancel();
        }
    }
}