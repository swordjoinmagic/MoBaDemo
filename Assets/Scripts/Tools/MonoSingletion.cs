using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// MonoBehavior的单例类扩展
/// </summary>
public abstract class MonoSingletion<T> : MonoBehaviour where T:MonoBehaviour{
    protected static T instance = null;

    public static T Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<T>();
            }
            return instance;
        }
    }
}

