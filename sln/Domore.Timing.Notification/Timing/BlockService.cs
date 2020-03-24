using System;
using System.Runtime.InteropServices;

namespace Domore.Timing {
    using Notification;

    [Guid("D1BFA078-FA33-4A57-B62D-21AB9ADDE131")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class BlockService : Notifier, IBlock {
        private void Delay_Blocking(object sender, DelayEventArgs e) {
            if (e != null) {
                Elapsed = e.Elapsed;
                Remaining = e.Remaining;

                if (Canceling) {
                    Canceling = false;
                    e.Cancel = true;
                }
            }
        }

        public IDelay Delay {
            get => _Delay ?? (_Delay = new Delay().Service());
            set => _Delay = Change(_Delay, value, nameof(Delay));
        }
        private IDelay _Delay;

        public TimeSpan Elapsed {
            get => _Elapsed;
            private set => _Elapsed = Change(_Elapsed, value, nameof(Elapsed));
        }
        private TimeSpan _Elapsed;

        public TimeSpan Remaining {
            get => _Remaining;
            private set => _Remaining = Change(_Remaining, value, nameof(Remaining));
        }
        private TimeSpan _Remaining;

        public bool Delaying {
            get => _Delaying;
            private set => _Delaying = Change(_Delaying, value, nameof(Delaying));
        }
        private bool _Delaying;

        public bool Canceling {
            get => _Canceling;
            set => _Canceling = Change(_Canceling, value, nameof(Canceling));
        }
        private volatile bool _Canceling;

        public void For(TimeSpan time) {
            var delay = Delay;
            try {
                Delaying = true;
                delay.Blocking += Delay_Blocking;
                delay.For(time);
            }
            finally {
                delay.Blocking -= Delay_Blocking;
                Delaying = false;
            }
        }
    }
}
