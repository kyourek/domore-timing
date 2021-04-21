using Domore.Notification;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Domore.Diagnostics {
    [Guid("53DAE2F2-E3D0-48BC-B4CD-E9BD10F1C207")]
    [ComVisible(true)]
#if NETCOREAPP
    [ClassInterface(ClassInterfaceType.None)]
#else
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
#endif
    public class StopwatchAgent : Notifier, IStopwatch {
        private readonly Stopwatch Stopwatch = new Stopwatch();

        public TimeSpan Elapsed => Stopwatch.Elapsed;
        public long ElapsedMilliseconds => Stopwatch.ElapsedMilliseconds;
        public long ElapsedTicks => Stopwatch.ElapsedTicks;
        public bool IsRunning => Stopwatch.IsRunning;

        public void Start() {
            Stopwatch.Start();
            NotifyPropertyChanged();
        }

        public void Stop() {
            Stopwatch.Stop();
            NotifyPropertyChanged();
        }
    }
}
