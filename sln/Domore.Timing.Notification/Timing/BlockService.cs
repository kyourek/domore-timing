using System;
using System.Runtime.InteropServices;

namespace Domore.Timing {
    using Notification;

    [Guid("D1BFA078-FA33-4A57-B62D-21AB9ADDE131")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class BlockService : Notifier, IBlock {
        private void Update(DelayEventArgs e) {
            if (null == e) throw new ArgumentNullException(nameof(e));
            Elapsed = e.Elapsed;
            Remaining = e.Remaining;
            Determinate = e.Determinate;
            if (Canceling) {
                Canceling = false;
                e.Cancel = true;
            }
        }

        private void Delay_Starting(object sender, DelayEventArgs e) {
            Update(e);
        }

        private void Delay_Blocking(object sender, DelayEventArgs e) {
            Update(e);
        }

        private void Delay_Complete(object sender, DelayEventArgs e) {
            Update(e);
        }

        private void Delay(Action<IDelay> action) {
            if (null == action) throw new ArgumentNullException(nameof(action));
            var delay = DelayService;
            try {
                Timeout = null;
                Delaying = true;
                delay.Blocking += Delay_Blocking;
                delay.Starting += Delay_Starting;
                delay.Complete += Delay_Complete;
                action(delay);
            }
            catch (DelayTimeoutException ex) {
                Timeout = ex;
                throw;
            }
            finally {
                delay.Blocking -= Delay_Blocking;
                delay.Starting -= Delay_Starting;
                delay.Complete -= Delay_Complete;
                Delaying = false;
            }
        }

        public IDelay DelayService {
            get => _DelayService ?? (_DelayService = new Delay().Service());
            set => Change(ref _DelayService, value, nameof(DelayService));
        }
        private IDelay _DelayService;

        public bool Determinate {
            get => _Determinate;
            private set => Change(ref _Determinate, value, nameof(Determinate));
        }
        private bool _Determinate;

        public DelayTimeoutException Timeout {
            get => _Timeout;
            private set => Change(ref _Timeout, value, nameof(Timeout));
        }
        private DelayTimeoutException _Timeout;

        public TimeSpan Elapsed {
            get => _Elapsed;
            private set => Change(ref _Elapsed, value, nameof(Elapsed));
        }
        private TimeSpan _Elapsed;

        public TimeSpan Remaining {
            get => _Remaining;
            private set => Change(ref _Remaining, value, nameof(Remaining));
        }
        private TimeSpan _Remaining;

        public bool Delaying {
            get => _Delaying;
            private set => Change(ref _Delaying, value, nameof(Delaying));
        }
        private bool _Delaying;

        public bool Canceling {
            get => _Canceling;
            private set {
                if (_Canceling != value) {
                    _Canceling = value;
                    NotifyPropertyChanged(nameof(Canceling));
                }
            }
        }
        private volatile bool _Canceling;

        public void Cancel() {
            Canceling = true;
        }

        public void For(TimeSpan time, Func<bool> cancel) {
            Delay(delay => delay.For(time, cancel));
        }

        public void For(TimeSpan time) {
            For(time, null);
        }

        public void Until(Func<bool> predicate, TimeSpan timeout, Func<bool> cancel) {
            Delay(delay => delay.Until(predicate, timeout, cancel));
        }

        public void Until(Func<bool> predicate, TimeSpan timeout) {
            Until(predicate, timeout, null);
        }
    }
}
