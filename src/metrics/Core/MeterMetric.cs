using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using metrics.Stats;
using metrics.Support;

namespace metrics.Core
{
    /// <summary>
    /// A meter metric which measures mean throughput and one-, five-, and fifteen-minute exponentially-weighted moving average throughputs.
    /// </summary>
    /// <see href="http://en.wikipedia.org/wiki/Moving_average#Exponential_moving_average">EMA</see>
    public class MeterMetric : IMetric, IMetered, IDisposable
    {
        private AtomicLong _count = new AtomicLong();
        private long _startTime = DateTime.UtcNow.Ticks;

        private EWMA _m1Rate;
        private EWMA _m5Rate;
        private EWMA _m15Rate;

        private static readonly SharedTimer Timer = new SharedTimer(TimeSpan.FromSeconds(5));

        public MeterMetric(string eventType, TimeUnit rateUnit)
        {
            var timestamp = Timer.ElapsedNanoseconds;

            _m1Rate = EWMA.OneMinuteEWMA(timestamp);
            _m5Rate = EWMA.FiveMinuteEWMA(timestamp);
            _m15Rate = EWMA.FifteenMinuteEWMA(timestamp);

            EventType = eventType;
            RateUnit = rateUnit;

            Timer.Tick += Tick;
        }

        /// <summary>
        /// Clears all recorded values
        /// </summary>
        public void Clear()
        {
            _count.Set(0);

            var timestamp = Timer.ElapsedNanoseconds;

            _m1Rate.Clear(timestamp);
            _m5Rate.Clear(timestamp);
            _m15Rate.Clear(timestamp);

            _startTime = DateTime.UtcNow.Ticks;
        }

        /// <summary>
        /// Returns the meter's rate unit
        /// </summary>
        /// <returns></returns>
        public TimeUnit RateUnit { get; private set; }

        /// <summary>
        /// Returns the type of events the meter is measuring
        /// </summary>
        /// <returns></returns>
        public string EventType { get; private set; }

        private void Tick(long timestamp)
        {
            _m1Rate.Tick(timestamp);
            _m5Rate.Tick(timestamp);
            _m15Rate.Tick(timestamp);
        }

        /// <summary>
        /// Mark the occurrence of an event
        /// </summary>
        public void Mark()
        {
            Mark(1);
        }

        /// <summary>
        /// Mark the occurrence of a given number of events
        /// </summary>
        public void Mark(long n)
        {
            _count.AddAndGet(n);
            _m1Rate.Update(n);
            _m5Rate.Update(n);
            _m15Rate.Update(n);
        }

        /// <summary>
        ///  Returns the number of events which have been marked
        /// </summary>
        /// <returns></returns>
        public long Count
        {
            get { return _count.Get(); }
        }

        /// <summary>
        /// Returns the fifteen-minute exponentially-weighted moving average rate at
        /// which events have occured since the meter was created
        /// <remarks>
        /// This rate has the same exponential decay factor as the fifteen-minute load
        /// average in the top Unix command.
        /// </remarks>
        /// </summary>
        public double FifteenMinuteRate
        {
            get
            {
                return _m15Rate.Rate(RateUnit);
            }
        }

        /// <summary>
        /// Returns the five-minute exponentially-weighted moving average rate at
        /// which events have occured since the meter was created
        /// <remarks>
        /// This rate has the same exponential decay factor as the five-minute load
        /// average in the top Unix command.
        /// </remarks>
        /// </summary>
        public double FiveMinuteRate
        {
            get
            {
                return _m5Rate.Rate(RateUnit);
            }
        }

        /// <summary>
        /// Returns the mean rate at which events have occured since the meter was created
        /// </summary>
        public double MeanRate
        {
            get
            {
                if (Count != 0)
                {
                    var elapsed = (DateTime.UtcNow.Ticks - _startTime) * 100; // 1 DateTime Tick == 100ns
                    return ConvertNanosRate(Count / (double)elapsed);
                }
                return 0.0;
            }
        }

        /// <summary>
        /// Returns the one-minute exponentially-weighted moving average rate at
        /// which events have occured since the meter was created
        /// <remarks>
        /// This rate has the same exponential decay factor as the one-minute load
        /// average in the top Unix command.
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        public double OneMinuteRate
        {
            get
            {
                return _m1Rate.Rate(RateUnit);
            }
        }

        private double ConvertNanosRate(double ratePerNs)
        {
            return ratePerNs * RateUnit.ToNanos(1);
        }

        [IgnoreDataMember]
        public IMetric Copy
        {
            get
            {
                var metric = new MeterMetric(EventType, RateUnit)
                                 {
                                     _count = Count,
                                     _m1Rate = _m1Rate,
                                     _m5Rate = _m5Rate,
                                     _m15Rate = _m15Rate
                                 };
                return metric;
            }
        }

        public void Dispose()
        {
            Timer.Tick -= Tick;
        }
    }
}
