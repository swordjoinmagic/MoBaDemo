using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestPointEnterAndExit : MonoBehaviour{

    public RectTransform rectTransform;

    private void Start() {
        Debug.Log(rectTransform.anchoredPosition);
    }
}

