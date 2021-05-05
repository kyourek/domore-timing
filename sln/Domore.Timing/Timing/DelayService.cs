using System;
using System.Diagnostics;

namespace Domore.Timing {
    using Threading;

    internal class DelayService : IDelay {
        private static readonly TimeSpan OneMillisecond = TimeSpan.FromMilliseconds(1);
        private static readonly TimeSpan TenMilliseconds = TimeSpan.FromMilliseconds(10);

        private int? SleepTime(TimeSpan remaining) {
            if (remaining < OneMillisecond) return 0;
            if (remaining < TenMilliseconds) return 1;
            return 10;
        }

        private bool Canceling(TimeSpan elapsed, TimeSpan? remaining, Delay.CancelDelegate cancel) {
            var e = new DelayEventArgs(elapsed, remaining);
            if (cancel?.Invoke(e) == true) return true;

            OnBlocking(e);
            return e.Cancel;
        }

        private TimeSpan CallComplete(TimeSpan elapsed) {
            OnComplete(new DelayEventArgs(elapsed, TimeSpan.Zero));
            return elapsed;
        }

        private Stopwatch CallStarting(TimeSpan? remaining) {
            OnStarting(new DelayEventArgs(TimeSpan.Zero, remaining));
            return Stopwatch.StartNew();
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
            For(time, default(Delay.CancelDelegate));

        public double For(double milliseconds) =>
            For(milliseconds, default(Delay.CancelDelegate));

        public TimeSpan For(TimeSpan time, Func<bool> cancel) =>
            For(time, _ => cancel?.Invoke() ?? false);

        public double For(double milliseconds, Func<bool> cancel) =>
            For(milliseconds, _ => cancel?.Invoke() ?? false);

        public TimeSpan For(TimeSpan time, Delay.CancelDelegate cancel) {
            if (time <= TimeSpan.Zero) return TimeSpan.Zero;

            var sw = CallStarting(time);
            for (; ; ) {
                var elapsed = sw.Elapsed;
                if (elapsed >= time) return CallComplete(elapsed);

                var remaining = time - elapsed;
                if (remaining > OneMillisecond) {
                    var canceling = Canceling(elapsed, remaining, cancel);
                    if (canceling) return CallComplete(elapsed);

                    var sleepTime = SleepTime(remaining);
                    if (sleepTime.HasValue) Sleep.For(sleepTime.Value);
                }
            }
        }

        public double For(double milliseconds, Delay.CancelDelegate cancel) {
            return For(TimeSpan.FromMilliseconds(milliseconds), cancel).TotalMilliseconds;
        }

        public TimeSpan Until(Func<bool> predicate, TimeSpan timeout, Delay.CancelDelegate cancel) {
            if (null == predicate) throw new ArgumentNullException(nameof(predicate));
            if (true == predicate()) return TimeSpan.Zero;

            var sw = CallStarting(null);
            for (; ; ) {
                var elapsed = sw.Elapsed;
                if (elapsed > timeout) {
                    throw new DelayTimeoutException();
                }
                var complete = predicate() || Canceling(elapsed, null, cancel);
                if (complete) {
                    return CallComplete(elapsed);
                }
                Sleep.For(0);
            }
        }

        public TimeSpan Until(Func<bool> predicate, TimeSpan timeout, Func<bool> cancel) =>
            Until(predicate, timeout, _ => cancel?.Invoke() ?? false);

        public TimeSpan Until(Func<bool> predicate, TimeSpan timeout) =>
            Until(predicate, timeout, default(Delay.CancelDelegate));

        public double Until(Func<bool> predicate, int millisecondsTimeout, Delay.CancelDelegate cancel) {
            return Until(predicate, TimeSpan.FromMilliseconds(millisecondsTimeout), cancel).TotalMilliseconds;
        }

        public double Until(Func<bool> predicate, int millisecondsTimeout, Func<bool> cancel) =>
            Until(predicate, millisecondsTimeout, _ => cancel?.Invoke() ?? false);

        public double Until(Func<bool> predicate, int millisecondsTimeout) =>
            Until(predicate, millisecondsTimeout, default(Delay.CancelDelegate));
    }
}
