using System;
using NUnit.Framework;
using metrics.Stats;

namespace metrics.Tests.Stats
{
    [TestFixture]
    public class EWMATests
    {
        private const long InitialTimestamp = 0;
        private const long FiveSecondsTimestamp = 5000000000;

        [Test]
        public void Can_relative_decay_rates_for_discrete_values()
        {
            var one = EWMA.OneMinuteEWMA(InitialTimestamp);
            one.Update(100000);
            one.Tick(FiveSecondsTimestamp);

            var five = EWMA.FiveMinuteEWMA(InitialTimestamp);
            five.Update(100000);
            five.Tick(FiveSecondsTimestamp);

            var fifteen = EWMA.FifteenMinuteEWMA(InitialTimestamp);
            fifteen.Update(100000);
            fifteen.Tick(FiveSecondsTimestamp);

            var rateOne = one.Rate(TimeUnit.Seconds);
            var rateFive = five.Rate(TimeUnit.Seconds);
            var rateFifteen = fifteen.Rate(TimeUnit.Seconds);

            Assert.AreEqual(20000, rateOne);
            Assert.AreEqual(20000, rateFive);
            Assert.AreEqual(20000, rateFifteen);

            var timestamp = FiveSecondsTimestamp;

            ElapseMinute(timestamp, one);
            rateOne = one.Rate(TimeUnit.Seconds);

            ElapseMinute(timestamp, five);
            rateFive = five.Rate(TimeUnit.Seconds);

            ElapseMinute(timestamp, fifteen);
            rateFifteen = fifteen.Rate(TimeUnit.Seconds);

            Assert.AreEqual(7357.5888234288504d, rateOne);
            Assert.AreEqual(16374.615061559636d, rateFive);
            Assert.AreEqual(18710.13970063235d, rateFifteen);
        }

        [Test]
        public void Can_retrieve_decaying_rate_with_discrete_value()
        {
            var ewma = EWMA.OneMinuteEWMA(InitialTimestamp);
            ewma.Update(3);
            ewma.Tick(FiveSecondsTimestamp);    // Assumes 5 seconds have passed

            var rate = ewma.Rate(TimeUnit.Seconds);
            Assert.AreEqual(rate, 0.6, "the EWMA has a rate of 0.6 events/sec after the first tick");

            var timestamp = FiveSecondsTimestamp;

            timestamp = ElapseMinute(timestamp, ewma);

            rate = ewma.Rate(TimeUnit.Seconds);
            AssertIsCloseTo(0.22072766, rate, 8, "the EWMA has a rate of 0.22072766 events/sec after 1 minute");

            timestamp = ElapseMinute(timestamp, ewma);

            rate = ewma.Rate(TimeUnit.Seconds);
            AssertIsCloseTo(0.08120116, rate, 8, "the EWMA has a rate of 0.08120116 events/sec after 2 minutes");

            timestamp = ElapseMinute(timestamp, ewma);

            rate = ewma.Rate(TimeUnit.Seconds);
            AssertIsCloseTo(0.02987224, rate, 8, "the EWMA has a rate of 0.02987224 events/sec after 3 minutes");

            timestamp = ElapseMinute(timestamp, ewma);

            rate = ewma.Rate(TimeUnit.Seconds);
            AssertIsCloseTo(0.01098938, rate, 8, "the EWMA has a rate of 0.01098938 events/sec after 4 minutes");

            timestamp = ElapseMinute(timestamp, ewma);

            rate = ewma.Rate(TimeUnit.Seconds);
            AssertIsCloseTo(0.00404276, rate, 8, "the EWMA has a rate of 0.00404276 events/sec after 5 minutes");

            timestamp = ElapseMinute(timestamp, ewma);

            rate = ewma.Rate(TimeUnit.Seconds);
            AssertIsCloseTo(0.00148725, rate, 8, "the EWMA has a rate of 0.00148725 events/sec after 6 minutes");

            timestamp = ElapseMinute(timestamp, ewma);

            rate = ewma.Rate(TimeUnit.Seconds);
            AssertIsCloseTo(0.00054712, rate, 8, "the EWMA has a rate of 0.00054712 events/sec after 7 minutes");

            ElapseMinute(timestamp, ewma);

            rate = ewma.Rate(TimeUnit.Seconds);
            AssertIsCloseTo(0.00020127, rate, 8, "the EWMA has a rate of 0.00020127 events/sec after 8 minutes");
        }

        private static long ElapseMinute(long currentTimestamp, EWMA ewma)
        {
            for(var i = 1; i <= 12; i++)
            {
                currentTimestamp += FiveSecondsTimestamp;

                ewma.Tick(currentTimestamp);
            }

            return currentTimestamp;
        }

        public static void AssertIsCloseTo(double expected, double actual, int digits, string message)
        {
            var right = actual.ToString().Substring(0, digits + 2);
            var left = expected.ToString().Substring(0, digits + 2);
            Assert.AreEqual(Convert.ToDouble(left), Convert.ToDouble(right), message);
        }
    }
}
