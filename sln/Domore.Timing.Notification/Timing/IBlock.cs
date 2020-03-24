using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        IDelay Delay { get; set; }

        [DispId(2)]
        TimeSpan Elapsed { get; }

        [DispId(3)]
        TimeSpan Remaining { get; }

        [DispId(4)]
        bool Delaying { get; }

        [DispId(5)]
        bool Canceling { get; }

        [DispId(6)]
        void For(TimeSpan time);
    }
}
