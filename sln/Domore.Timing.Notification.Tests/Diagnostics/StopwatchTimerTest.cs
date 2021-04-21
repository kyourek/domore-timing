using NUnit.Framework;
using System;
using System.Threading;

namespace Domore.Diagnostics {
    [TestFixture]
    public class StopwatchTimerTest {
        [Test]
        public void Constructor_RaisesPropertyChanged() {
            var raised = false;
            using (var subject = new StopwatchTimer(period: TimeSpan.FromMilliseconds(1))) {
                subject.PropertyChanged += (s, e) => raised = true;
                Thread.Sleep(5);
            }
            Assert.That(raised, Is.True);
        }
    }
}
