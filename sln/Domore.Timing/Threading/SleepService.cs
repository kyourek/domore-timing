using System.Threading;

namespace Domore.Threading {
    internal class SleepService {
        public virtual void For(int milliseconds) =>
            Thread.Sleep(milliseconds);
    }
}
