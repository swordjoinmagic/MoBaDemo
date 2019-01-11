using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 用于编辑器的InputField控件
/// 
/// 
/// </summary>
public class InputFieldView : MonoBehaviour {
    InputField inputField;

    private InputField InputField {
        get {
            if (inputField == null) inputField = GetComponentInChildren<InputField>();
            return inputField;
        }
    }

    public InputField.ContentType ContentType {
        get {
            return InputField.contentType;
        }
        set {
            InputField.contentType = value;
        }
    }

    public int GetIntValue() {
        return int.Parse(InputField.text); 
    }
    public string GetStringValue() {
        return InputField.text;
    }
    public float GetFloatValue() {
        return float.Parse(InputField.text);
    }

}

