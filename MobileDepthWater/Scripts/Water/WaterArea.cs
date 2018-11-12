namespace Assets.Scripts.Water
{
    using UnityEngine;

    /// <summary>
    /// Area that belongs to some water
    /// </summary>
    public class WaterArea : MonoBehaviour
    {
        /// <summary>
        /// Water shader properties of the water this area belong
        /// </summary>
        [SerializeField] private WaterPropertyBlockSetter waterProperties;

        public MaterialPropertyBlock WaterPropertyBlock
        {
            get { return waterProperties.MaterialPropertyBlock; }
        }
    }
}
