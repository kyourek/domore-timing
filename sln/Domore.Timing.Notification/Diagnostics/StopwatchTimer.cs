using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Domore.Diagnostics {
    using Notification;

    [Guid("EE7450D4-0770-4536-96B7-04E91F827DFE")]
    [ComVisible(true)]
#if NETCOREAPP
    [ClassInterface(ClassInterfaceType.None)]
#else
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
#endif
    public class StopwatchTimer : Notifier, IStopwatch, IDisposable {
        private volatile bool TimerStarted;
        private readonly object TimerLocker = new object();
        private readonly Stopwatch Stopwatch = new Stopwatch();
        private readonly PropertyChangedEventArgs PropertyChangedEventArgs = new PropertyChangedEventArgs(string.Empty);

        private Timer Timer =>
            _Timer ?? (
            _Timer = new Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite));
        private Timer _Timer;

        private void TimerCallback(object state) {
            if (Stopwatch.IsRunning) {
                OnRunning(new StopwatchTimerEventArgs(Stopwatch.Elapsed));
            }
            else {
                TimerStop();
            }
            OnPropertyChanged(PropertyChangedEventArgs);
        }

        private void TimerStart() {
            if (TimerStarted == false) {
                lock (TimerLocker) {
                    if (TimerStarted == false) {
                        Timer.Change(TimeSpan.Zero, Period);
                        TimerStarted = true;
                    }
                }
            }
        }

        private void TimerStop() {
            if (TimerStarted) {
                lock (TimerLocker) {
                    if (TimerStarted) {
                        Timer.Change(Timeout.Infinite, Timeout.Infinite);
                        TimerStarted = false;
                    }
                }
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                if (_Timer != null) {
                    lock (TimerLocker) {
                        if (_Timer != null) {
                            _Timer.Dispose();
                        }
                    }
                }
            }
        }

        protected virtual void OnRunning(StopwatchTimerEventArgs e) =>
            Running?.Invoke(this, e);

        public event StopwatchTimerEventHandler Running;

        public TimeSpan Elapsed => Stopwatch.Elapsed;
        public long ElapsedMilliseconds => Stopwatch.ElapsedMilliseconds;
        public long ElapsedTicks => Stopwatch.ElapsedTicks;
        public bool IsRunning => Stopwatch.IsRunning;

        public TimeSpan Period {
            get => _Period;
            set => _Period = Change(_Period, value, nameof(Period));
        }
        private TimeSpan _Period = TimeSpan.FromMilliseconds(10);

        public StopwatchTimer(bool start = false) {
            if (start) {
                Start();
            }
        }

        public void Reset() {
            Stopwatch.Reset();
        }

        public void Restart() {
            Stopwatch.Restart();
            TimerStart();
        }

        public void Start() {
            Stopwatch.Start();
            TimerStart();
        }

        public void Stop() {
            Stopwatch.Stop();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~StopwatchTimer() {
            Dispose(false);
        }
    }
}
