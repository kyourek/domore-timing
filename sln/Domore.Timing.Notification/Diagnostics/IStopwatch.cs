using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Domore.Diagnostics {
    [Guid("85489DD4-E448-4D1D-952C-1AAD93E90620")]
    [ComVisible(true)]
#if NETCOREAPP
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
#else
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
#endif
    public interface IStopwatch : INotifyPropertyChanged {
        [DispId(1)]
        TimeSpan Elapsed { get; }

        [DispId(2)]
        long ElapsedMilliseconds { get; }

        [DispId(3)]
        long ElapsedTicks { get; }

        [DispId(4)]
        bool IsRunning { get; }
    }
}
