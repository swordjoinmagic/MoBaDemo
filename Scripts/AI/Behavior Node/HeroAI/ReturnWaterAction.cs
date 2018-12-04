using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ReturnWaterAction : Action {

    private HeroMono heroMono;

    public override void OnStart() {
        heroMono = GetComponent<HeroMono>();
    }

    public override TaskStatus OnUpdate() {
        return base.OnUpdate();
    }
}
