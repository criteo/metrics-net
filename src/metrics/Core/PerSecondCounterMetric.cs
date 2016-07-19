using System;
using metrics.Stats;
using System.Text;
using System.Runtime.Serialization;

namespace metrics.Core
{
    public class PerSecondCounterMetric : IMetric, IDisposable
    {
        private readonly string _eventType;
        private readonly TimeUnit _rateUnit;

        private static readonly SharedTimer Timer = new SharedTimer(TimeSpan.FromSeconds(1));

        private EWMA _ewma;

        private void TimeElapsed(long timestamp)
        {
            _ewma.Tick(timestamp);
        }
        public void LogJson(StringBuilder sb)
        {
            sb.Append("{\"count\":").Append(CurrentValue)
              .Append(",\"rate unit\":\"").Append(RateUnit).Append("\"}");

        }
        [IgnoreDataMember]
        public IMetric Copy
        {
            get
            {
                var metric = new PerSecondCounterMetric(EventType, RateUnit)
                {
                    _ewma = _ewma
                };

                return metric;
            }
        }

        public void Mark(long n)
        {
            _ewma.Update(n);
        }

        public void Mark()
        {
            _ewma.Update(1);
        }

        public double CurrentValue
        {
            get { return _ewma.Rate(_rateUnit); }
        }

        public string EventType
        {
            get { return _eventType; }
        }

        public TimeUnit RateUnit
        {
            get { return _rateUnit; }
        }

        private PerSecondCounterMetric(string eventType, TimeUnit rateUnit)
        {
            _ewma = EWMA.OneSecondEWMA(Timer.ElapsedNanoseconds);

            Timer.Tick += TimeElapsed;

            _eventType = eventType;
            _rateUnit = rateUnit;
        }

        public static PerSecondCounterMetric New(string eventType)
        {
            return new PerSecondCounterMetric(eventType, TimeUnit.Seconds);
        }

        public void Dispose()
        {
            Timer.Tick -= TimeElapsed;
        }
    }
}