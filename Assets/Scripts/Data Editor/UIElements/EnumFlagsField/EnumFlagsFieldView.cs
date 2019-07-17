using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class EnumFlagsFieldView : MonoBehaviour{

    EnumFlagsFieldDropDown dropDown;

    private Type enumType;

    public Type EnumType {
        get {
            return enumType;
        }

        set {
            enumType = value;

            if(dropDown==null) dropDown = GetComponentInChildren<EnumFlagsFieldDropDown>();

            dropDown.Enum = (Enum)Enum.ToObject(EnumType, 0);
        }
    }

    private void Start() {
        dropDown = GetComponentInChildren<EnumFlagsFieldDropDown>();

        EnumType = typeof(UnitType);
    }


}

