using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Move : MonoBehaviour {

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButton(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {

                Vector3 vector3 = new Vector3(hit.point.x,0,hit.point.z);

                GetComponent<Animator>().SetBool("isRun", true);
                GetComponent<NavMeshAgent>().SetDestination(vector3);
            }
        }
	}
}
