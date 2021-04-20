using NUnit.Framework;
using System.Threading;

namespace Domore.Diagnostics {
    [TestFixture]
    public class StopwatchTimerTest {
        private StopwatchTimer Subject {
            get => _Subject ?? (_Subject = new StopwatchTimer());
            set => _Subject = value;
        }
        private StopwatchTimer _Subject;

        [SetUp]
        public void SetUp() {
            Subject = null;
        }

        [Test]
        public void Notify_RaisesPropertyChanged() {
            var raised = false;
            Subject.PropertyChanged += (s, e) => {
                raised = true;
            };
            using (Subject.Notify()) {
                Thread.Sleep(10);
                Assert.That(raised, Is.True);
            }
        }
    }
}
