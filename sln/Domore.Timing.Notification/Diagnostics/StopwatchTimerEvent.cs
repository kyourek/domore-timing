using System;
using System.Runtime.InteropServices;

namespace Domore.Diagnostics {
    [Guid("4D0433D0-189A-4CB4-9915-C42339A4B9B0")]
    [ComVisible(true)]
    public delegate void StopwatchTimerEventHandler(object sender, StopwatchTimerEventArgs e);

    [Guid("17FF5816-4FC9-4E14-8D00-7D1C4802BDFF")]
    [ComVisible(true)]
#if NETCOREAPP
    [ClassInterface(ClassInterfaceType.None)]
#else
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
#endif
    public class StopwatchTimerEventArgs : EventArgs {
        public TimeSpan Elapsed { get; }

        public StopwatchTimerEventArgs(TimeSpan elapsed) {
            Elapsed = elapsed;
        }
    }
}
