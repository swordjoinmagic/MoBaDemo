namespace Assets.Scripts.Helpers
{
    using UnityEngine;

    public class FPSCounter
    {
        private const float Frequency = 0.1f;
        private const int DataSize = 100;

        private float[] fpsData;
        private float[] deltaTimeData;

        private float timer;
        private int index;

        private int lastFrameCount;
        private float lastTime;

        public FPSCounter()
        {
            index = 0;
            timer = 0f;

            fpsData = new float[DataSize];
            deltaTimeData = new float[DataSize];
        }

        public float Fps
        {
            get { return fpsData[Mathf.Max(index - 1, 0)]; }
        }

        public void Update(float deltaTime)
        {
            timer += deltaTime;

            if (timer >= Frequency)
            {
                var timeSpan = Time.realtimeSinceStartup - lastTime;
                var frameCount = Time.frameCount - lastFrameCount;

                fpsData[index] = frameCount / timeSpan;
                deltaTimeData[index] = timer;

                index = (index + 1) % DataSize;

                lastFrameCount = Time.frameCount;
                lastTime = Time.realtimeSinceStartup;

                timer = 0f;
            }
        }

        /// <summary>
        /// Return average fps for last N seconds.
        /// </summary>
        /// <param name="forLastSec">Last seconds for calculating.</param>
        /// <returns>Average fps for last N seconds.</returns>
        public float GetAverageFps(float forLastSec)
        {
            var fpsSum = 0f;
            var timeSum = 0f;

            var localIndex = index - 1;
            var totalDataCount = 0;

            while (timeSum <= forLastSec)
            {
                if (localIndex < 0)
                {
                    localIndex += DataSize;
                }

                fpsSum += fpsData[localIndex];
                timeSum += deltaTimeData[localIndex];

                localIndex--;
                totalDataCount++;

                if (totalDataCount == DataSize)
                {
                    Debug.LogWarning("Too few data to calculate average fps for such long term.");
                    break;
                }
            }

            return fpsSum / totalDataCount;
        }
    }
}