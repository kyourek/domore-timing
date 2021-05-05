using NUnit.Framework;
using System;
using System.Diagnostics;

namespace Domore.Timing {
    [TestFixture]
    public class BlockServiceTest {
        private BlockService Subject {
            get => _Subject ?? (_Subject = new BlockService());
            set => _Subject = value;
        }
        private BlockService _Subject;

        [SetUp]
        public void SetUp() {
            Subject = null;
        }

        [Test]
        public void For_DelaysForSomeTime() {
            var time = TimeSpan.FromMilliseconds(10);
            Subject.For(time);
            Assert.That(Subject.Elapsed, Is.GreaterThanOrEqualTo(time));
        }

        [Test]
        public void Until_ThrowsTimeoutException() {
            Assert.That(
                () => Subject.Until(() => false, TimeSpan.FromMilliseconds(1)),
                Throws.InstanceOf(typeof(DelayTimeoutException)));
        }

        [Test]
        public void Until_WaitsForPredicate() {
            var sw = Stopwatch.StartNew();
            Subject.Until(() => sw.ElapsedMilliseconds > 10, TimeSpan.FromMilliseconds(100));
            Assert.That(Subject.Elapsed, Is.GreaterThanOrEqualTo(TimeSpan.FromMilliseconds(1)));
        }
    }
}
