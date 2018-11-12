namespace Assets.MobileOptimizedWater.Scripts
{
    using Assets.Scripts.Helpers;
    using UnityEngine;
    using UnityEngine.UI;

    public class FPSUI : MonoBehaviour
    {
        [SerializeField] private Text fpsText;

        private FPSCounter fpsCounter;

        public void Awake()
        {
            fpsCounter = new FPSCounter();
        }

        public void Update()
        {
            fpsCounter.Update(Time.deltaTime);
            fpsText.text = "Fps: " + fpsCounter.GetAverageFps(1f).ToString("###");
        }
    }
}
