namespace Assets.MobileOptimizedWater.Scripts
{
    using UnityEngine;

    public class CameraRotator : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;

        public void Update()
        {
            var angles = transform.eulerAngles;
            angles.y += Time.deltaTime * speed;

            transform.eulerAngles = angles;
        }
    }
}
