using UnityEngine;
class NetWorkPlayerr : MonoBehaviour{
    private void Update() {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(h,0,v)*Time.deltaTime*10);
        if((v!=0 || h!=0))
            NetWorkManager.Instance.SendPos();
    }
}

