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
    public class StopwatchTimer : Notifier, IStopwatch {
        private readonly Stopwatch Stopwatch = new Stopwatch();
        private readonly static PropertyChangedEventArgs PropertyChangedEventArgs = new PropertyChangedEventArgs(string.Empty);

        private static void TimerCallback(object state) {
            var @this = (StopwatchTimer)state;
            @this.OnPropertyChanged(PropertyChangedEventArgs);
        }

        public TimeSpan Elapsed => Stopwatch.Elapsed;
        public long ElapsedMilliseconds => Stopwatch.ElapsedMilliseconds;
        public long ElapsedTicks => Stopwatch.ElapsedTicks;
        public bool IsRunning => Stopwatch.IsRunning;

        public TimeSpan Period {
            get => _Period;
            set => _Period = Change(_Period, value, nameof(Period));
        }
        private TimeSpan _Period = TimeSpan.FromMilliseconds(10);

        public void Reset() {
            Stopwatch.Reset();
        }

        public IDisposable Start(TimeSpan? period = null) {
            Stopwatch.Start();
            return new Timer(
                callback: TimerCallback,
                state: this,
                dueTime: TimeSpan.Zero,
                period: period ?? TimeSpan.FromMilliseconds(10));
        }

        public void Stop() {
            Stopwatch.Stop();
        }
    }
}
