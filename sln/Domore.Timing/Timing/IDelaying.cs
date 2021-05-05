using System;
using System.Runtime.InteropServices;

namespace Domore.Timing {
    [Guid("E10BA997-D29D-4968-857A-C4604CED3329")]
    [ComVisible(true)]
#if NETCOREAPP
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
#else
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
#endif
    public interface IDelaying {
        bool Determinate { get; }
        double ElapsedMilliseconds { get; }
        double RemainingMilliseconds { get; }
        TimeSpan ElapsedTime { get; }
        TimeSpan RemainingTime { get; }
    }
}
