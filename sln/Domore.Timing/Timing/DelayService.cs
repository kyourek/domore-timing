using System;
using System.Diagnostics;

namespace Domore.Timing {
    using Threading;

    internal class DelayService : IDelay {
        private static readonly TimeSpan OneMillisecond = TimeSpan.FromMilliseconds(1);
        private static readonly TimeSpan TenMilliseconds = TimeSpan.FromMilliseconds(10);

        private bool CancelDelay(TimeSpan elapsed, TimeSpan remaining, Delay.CancelTimeSpanDelegate cancel) {
            if (cancel?.Invoke(elapsed, remaining) ?? false) {
                return true;
            }

            var e = new DelayEventArgs(elapsed, remaining);
            OnBlocking(e);
            return e.Cancel;
        }

        private int? SleepTime(TimeSpan remaining) {
            if (remaining < OneMillisecond) return 0;
            if (remaining < TenMilliseconds) return 1;
            return 10;
        }

        protected virtual void OnStarting(DelayEventArgs e) =>
            Starting?.Invoke(this, e);

        protected virtual void OnBlocking(DelayEventArgs e) =>
            Blocking?.Invoke(this, e);

        protected virtual void OnComplete(DelayEventArgs e) =>
            Complete?.Invoke(this, e);

        public event DelayEventHandler Starting;
        public event DelayEventHandler Blocking;
        public event DelayEventHandler Complete;

        public SleepService Sleep {
            get => _Sleep ?? (_Sleep = new SleepService());
            set => _Sleep = value;
        }
        private SleepService _Sleep;

        public TimeSpan For(TimeSpan time) =>
            For(time, default(Delay.CancelTimeSpanDelegate));

        public double For(double milliseconds) =>
            For(milliseconds, default(Delay.CancelMillisecondsDelegate));

        public TimeSpan For(TimeSpan time, Func<bool> cancel) =>
            For(time, (_, __) => cancel?.Invoke() ?? false);

        public double For(double milliseconds, Func<bool> cancel) =>
            For(milliseconds, (_, __) => cancel?.Invoke() ?? false);

        public TimeSpan For(TimeSpan time, Delay.CancelTimeSpanDelegate cancel) {
            if (time <= TimeSpan.Zero) return TimeSpan.Zero;

            OnStarting(new DelayEventArgs(TimeSpan.Zero, time));

            var sw = Stopwatch.StartNew();
            var elapsed = sw.Elapsed;
            while (elapsed < time) {
                var remaining = time - elapsed;
                if (remaining > OneMillisecond) {
                    if (CancelDelay(elapsed, remaining, cancel)) {
                        break;
                    }

                    var sleepTime = SleepTime(remaining);
                    if (sleepTime.HasValue) {
                        Sleep.For(sleepTime.Value);
                    }
                }

                elapsed = sw.Elapsed;
            }

            OnComplete(new DelayEventArgs(elapsed, TimeSpan.Zero));

            return elapsed;
        }

        public double For(double milliseconds, Delay.CancelMillisecondsDelegate cancel) =>
            For(TimeSpan.FromMilliseconds(milliseconds), cancel == null
                ? default(Delay.CancelTimeSpanDelegate)
                : (elapsed, remaining) => cancel(elapsed.TotalMilliseconds, remaining.TotalMilliseconds)
            ).TotalMilliseconds;
    }
}
