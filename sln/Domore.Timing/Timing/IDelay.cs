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
        double For(double milliseconds, Delay.CancelMillisecondsDelegate cancel);

        [ComVisible(false)]
        TimeSpan For(TimeSpan time, Func<bool> cancel);

        [ComVisible(false)]
        TimeSpan For(TimeSpan time, Delay.CancelTimeSpanDelegate cancel);
    }
}
