using System;
using System.Runtime.InteropServices;

namespace Domore.Timing {
    [Guid("2730DD97-E473-4CA6-9503-F67E035BC5C1")]
    [ComVisible(true)]
#if NETCOREAPP
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
#else
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
#endif
    public interface IDelay {
        event DelayEventHandler Starting;
        event DelayEventHandler Blocking;
        event DelayEventHandler Complete;

        [DispId(1)]
        double For(double milliseconds);

        [DispId(2)]
        TimeSpan For(TimeSpan time);

        [ComVisible(false)]
        double For(double milliseconds, Func<bool> cancel);

        [ComVisible(false)]
        double For(double milliseconds, Delay.CancelDelegate cancel);

        [ComVisible(false)]
        TimeSpan For(TimeSpan time, Func<bool> cancel);

        [ComVisible(false)]
        TimeSpan For(TimeSpan time, Delay.CancelDelegate cancel);

        [ComVisible(false)]
        double Until(Func<bool> predicate, int millisecondsTimeout, Delay.CancelDelegate cancel);

        [ComVisible(false)]
        double Until(Func<bool> predicate, int millisecondsTimeout, Func<bool> cancel);

        [ComVisible(false)]
        double Until(Func<bool> predicate, int millisecondsTimeout);

        [ComVisible(false)]
        TimeSpan Until(Func<bool> predicate, TimeSpan timeout, Delay.CancelDelegate cancel);

        [ComVisible(false)]
        TimeSpan Until(Func<bool> predicate, TimeSpan timeout, Func<bool> cancel);

        [ComVisible(false)]
        TimeSpan Until(Func<bool> predicate, TimeSpan timeout);
    }
}
