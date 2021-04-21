using System;
using System.Runtime.InteropServices;

namespace Domore.Timing {
    [Guid("7129C898-0AAB-4F73-B7EF-6CDB7210A8D6")]
    [ComVisible(true)]
#if NETCOREAPP
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
#else
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
#endif
    public interface IBlock {
        [DispId(1)]
        TimeSpan Elapsed { get; }

        [DispId(2)]
        TimeSpan Remaining { get; }

        [DispId(3)]
        bool Delaying { get; }

        [DispId(4)]
        bool Canceling { get; set; }

        [DispId(5)]
        void For(TimeSpan time, Func<bool> cancel = null);
    }
}
