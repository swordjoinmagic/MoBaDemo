using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class EnumFlagsFieldView : MonoBehaviour{

    EnumFlagsFieldDropDown dropDown;

    [SerializeField]
    public int index;

    [SerializeField]
    public string typeName;

    [SerializeField]
    public Type enumType;


    private void Start() {
        dropDown = GetComponentInChildren<EnumFlagsFieldDropDown>();

        enumType = Type.GetType(typeName);

        dropDown.Enum = (Enum)Enum.ToObject(enumType,0);
    }


}

