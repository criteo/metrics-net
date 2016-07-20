using System;
using System.Diagnostics;
using System.Threading;

namespace metrics.Core
{
    /// <summary>
    /// Timer designed to be shared by multiple MeterMetric instances
    /// The timer ticks only if there's subscribers
    /// </summary>
    internal class SharedTimer
    {
        private readonly object _syncRoot = new object();

        private readonly Stopwatch _stopwatch;
        private readonly TimeSpan _interval;

        private Timer _timer;
        private int _subscribersCount;

        public SharedTimer(TimeSpan interval)
        {
            _stopwatch = Stopwatch.StartNew();
            _interval = interval;
        }

        private event Action<long> InternalTick;

        /// <summary>
        /// Gets the number of nanoseconds elapsed since the instance was created
        /// </summary>
        public long ElapsedNanoseconds
        {
            get
            {
                // Stopwatch.Frequency: Number of ticks per second
                // _stopwatch.ElapsedTicks / Stopwatch.Frequency: Elapsed seconds
                // * 1000000000 (10^9): Conversion to nanoseconds
                return _stopwatch.ElapsedTicks * 1000000000 / Stopwatch.Frequency;
            }
        }

        /// <summary>
        /// Event triggered when the timer ticks (with the interval defined in the constructor)
        /// The parameter is a timestamp counting the elapsed nanoseconds since the SharedTimer instance was created
        /// Since the timer is shared, the first tick may occur before the set interval,
        /// use the timestamp to adjust your computations accordingly
        /// </summary>
        public event Action<long> Tick
        {
            add
            {
                lock (_syncRoot)
                {
                    InternalTick += value;
                    _subscribersCount++;

                    if (_timer == null)
                    {
                        Start();
                    }
                }
            }

            remove
            {
                lock (_syncRoot)
                {
                    InternalTick -= value;
                    _subscribersCount--;

                    if (_subscribersCount == 0)
                    {
                        Stop();
                    }
                }
            }
        }

        private void Start()
        {
            _timer = new Timer(OnTick, null, _interval, _interval);
        }

        private void Stop()
        {
            var timer = _timer;

            if (timer != null)
            {
                timer.Dispose();
                _timer = null;
            }
        }

        private void OnTick(object state)
        {
            var handler = InternalTick;

            if (handler != null)
            {
                handler(ElapsedNanoseconds);
            }
        }
    }
}
