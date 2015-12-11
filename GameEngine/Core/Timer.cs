using System.Diagnostics;

namespace GameEngine.Core
{
    public class Timer
    {
        /// <summary>
        /// Gets or sets the current frame number.
        /// </summary>
        public uint FrameNumber { get; private set; }

        /// <summary>
        /// Gets or sets the time in milliseconds that the last frame occurred.
        /// </summary>
        public long LastFrameTime { get; private set; }

        /// <summary>
        /// Gets or sets the duration of the last frame in milliseconds.
        /// </summary>
        public long LastFrameDuration { get; private set; }

        /// <summary>
        /// Gets or sets the average frame duration in milliseconds.
        /// </summary>
        public double AverageFrameDuration { get; private set; }

        /// <summary>
        /// Gets or sets whether the timer is paused or not.
        /// </summary>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// Gets or sets the average frames per second.
        /// </summary>
        public float FPS { get; private set; }

        /// <summary>
        /// Initialize the timer.
        /// </summary>
        public void Init()
        {
            FrameNumber = 0;
            LastFrameDuration = 0;
            AverageFrameDuration = 0;
            FPS = 0;
            LastFrameTime = CurrentTicksToMilliseconds();

            IsPaused = false;
        }

        /// <summary>
        /// Update the timer properties.
        /// </summary>
        public void Update()
        {
            // Advance the frame number.
            if (!IsPaused)
            {
                FrameNumber++;
            }

            // Update the timing information.
            long currentTime = CurrentTicksToMilliseconds();
            LastFrameDuration = currentTime - LastFrameTime;
            LastFrameTime = currentTime;

            // Update the frame rate.
            if (FrameNumber > 1)
            {
                if (AverageFrameDuration <= 0)
                {
                    AverageFrameDuration = LastFrameDuration;
                }
                else
                {
                    AverageFrameDuration *= 0.99;
                    AverageFrameDuration += 0.01 * LastFrameDuration;
                    FPS = (float)(1000.0 / AverageFrameDuration);
                }
            }
        }

        /// <summary>
        /// Convert the current clock ticks to milliseconds.
        /// </summary>
        private long CurrentTicksToMilliseconds()
        {
            return Stopwatch.GetTimestamp() / (Stopwatch.Frequency / 1000);
        }

        #region Singleton Implementation

        /// <summary>
        /// Instance of the timer.
        /// </summary>
        private static Timer instance;

        /// <summary>
        /// Creates an instance of the <see cref="Timer"/> class.
        /// </summary>
        private Timer()
        {
            // Prevents external construction.
        }

        /// <summary>
        /// Creates an instance of the <see cref="Timer"/> class.
        /// </summary>
        /// <param name="timer">Another instance of the timer class.</param>
        private Timer(Timer timer)
        {
            // Prevents external copy construction.
        }

        /// <summary>
        /// Gets an instance of the <see cref="Timer"/> class.
        /// </summary>
        public static Timer Instance
        {
            get { return instance ?? (instance = new Timer()); }
        }

        #endregion
    }
}
