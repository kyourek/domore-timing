using System;
using System.Runtime.InteropServices;

namespace Domore.Timing {
    [Guid("DC8D36B9-C15C-4D18-A3E6-4DB8F5E05555")]
    [ComVisible(true)]
#if NETCOREAPP
    [ClassInterface(ClassInterfaceType.None)]
#else
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
#endif
    public class Delay {
        private static readonly DelayService DelayService = new DelayService();

        public static double For(double milliseconds, CancelMillisecondsDelegate cancel) =>
            DelayService.For(milliseconds, cancel);

        public static double For(double milliseconds, Func<bool> cancel) =>
            DelayService.For(milliseconds, cancel);

        public static double For(double milliseconds) =>
            DelayService.For(milliseconds);

        public IDelay Service() =>
            new DelayService();

        public delegate bool CancelTimeSpanDelegate(TimeSpan elapsed, TimeSpan remaining);
        public delegate bool CancelMillisecondsDelegate(double elapsed, double remaining);
    }
}
