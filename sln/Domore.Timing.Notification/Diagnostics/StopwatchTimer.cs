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
        public TimeSpan Period { get; set; } = TimeSpan.FromMilliseconds(10);

        public IDisposable Notify() {
            return new Timer(
                callback: TimerCallback,
                state: this,
                dueTime: TimeSpan.Zero,
                period: Period);
        }

        public void Start() {
            Stopwatch.Start();
        }

        public void Stop() {
            Stopwatch.Stop();
        }

        public void Reset() {
            Stopwatch.Reset();
        }

        public void Restart() {
            Stopwatch.Restart();
        }
    }
}
