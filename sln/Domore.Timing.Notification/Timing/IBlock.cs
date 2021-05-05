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
        bool Canceling { get; }

        [DispId(5)]
        bool Determinate { get; }

        [DispId(6)]
        void Cancel();

        [DispId(7)]
        void For(TimeSpan time);

        [ComVisible(false)]
        void For(TimeSpan time, Func<bool> cancel);

        [ComVisible(false)]
        void Until(Func<bool> predicate, TimeSpan timeout, Func<bool> cancel);

        [ComVisible(false)]
        void Until(Func<bool> predicate, TimeSpan timeout);
    }
}
