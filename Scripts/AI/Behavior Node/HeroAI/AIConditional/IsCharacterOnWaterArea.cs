using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI.Behavior_Node.HeroAI.AIConditional {
    /// <summary>
    /// 判断单位是否处于泉水附近，
    /// </summary>
    public class IsCharacterOnWaterArea : Conditional{

        private HeroMono hero;
        public SharedTransform waterTransform;      // 泉水位置
        public float relaxRadius = 4f;

        public override void OnStart() {
            hero = GetComponent<HeroMono>();
        }

        public override TaskStatus OnUpdate() {
            if (Vector3.Distance(hero.transform.position,waterTransform.Value.position)<=relaxRadius) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
