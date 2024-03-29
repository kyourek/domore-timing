﻿using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Domore.Diagnostics {
    [Guid("EE7450D4-0770-4536-96B7-04E91F827DFE")]
    [ComVisible(true)]
#if NETCOREAPP
    [ClassInterface(ClassInterfaceType.None)]
#else
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
#endif
    public class StopwatchTimer : StopwatchAgent, IDisposable {
        private readonly Timer Timer;

        private static void TimerCallback(object state) {
            var @this = (StopwatchTimer)state;
            @this.NotifyPropertyChanged();
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) Timer.Dispose();
        }

        public TimeSpan Period { get; }

        public StopwatchTimer(TimeSpan period) {
            Timer = new Timer(callback: TimerCallback, state: this, dueTime: TimeSpan.Zero, period: Period = period);
        }

        public StopwatchTimer() : this(TimeSpan.FromMilliseconds(10)) {
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
