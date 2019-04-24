using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TestFollowMouse : MonoBehaviour{

    public GameObject g;

    private void Start() {
        Transform t =  g.transform.Find("ItemImage");
        Debug.Log(t==null);
        Image image = t.GetComponent<Image>();
        Debug.Log(image==null);
    }
}

