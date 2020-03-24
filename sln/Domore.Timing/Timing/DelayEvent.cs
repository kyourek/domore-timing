using System;
using System.Runtime.InteropServices;

namespace Domore.Timing {
    [Guid("D9520017-DB41-419F-950C-8234147AB9FA")]
    [ComVisible(true)]
    public delegate void DelayEventHandler(object sender, DelayEventArgs e);

    [Guid("F55A82F3-0B0D-4255-9A0F-8E6E5890F9D4")]
    [ComVisible(true)]
#if NETCOREAPP
    [ClassInterface(ClassInterfaceType.None)]
#else
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
#endif
    public class DelayEventArgs : EventArgs {
        public TimeSpan Elapsed { get; }
        public TimeSpan Remaining { get; }
        public bool Cancel { get; set; }

        public DelayEventArgs(TimeSpan elapsed, TimeSpan remaining) {
            Elapsed = elapsed;
            Remaining = remaining;
        }
    }
}
