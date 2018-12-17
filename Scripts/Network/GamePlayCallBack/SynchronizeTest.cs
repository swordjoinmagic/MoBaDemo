using UnityEngine;

public class SynchronizeTest {

    public SynchronizeTest(CharacterMono characterMono) {
        Init(characterMono);
    }

    public void Init(CharacterMono characterMono) {
        characterMono.OnMove += TransformSynchronize;
    }

    private float time;

    // 位置同步
    public void TransformSynchronize(CharacterMono characterMono,Vector3 pos) {
        //time += Time.smoothDeltaTime;
        //if (time >= 0.1f) {
        NetWorkManager.Instance.SendPos();
        //Debug.Log("单位正在移动");
        //time = 0;
        //}
    }

    // 动画同步
    public void AniamtionSynchronize(CharacterMono characterMono, string operation) {

    }
}

