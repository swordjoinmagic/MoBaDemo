using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace FSM {

    /// <summary>
    /// 黑板类，保存FSM状态机中公共变量,
    /// 拥有常用的Animator,NavMeshAgent,GameObject等公共变量,
    /// 同时,有一个字典用于存储这些公共变量
    /// </summary>
    public class BlackBorad {
        private Animator animator;
        private NavMeshAgent agent;
        private GameObject gameObject;
        private CharacterMono characterMono;
        public Dictionary<string, object> dictonary = new Dictionary<string, object>();

        public Animator Animator {
            get {
                return animator;
            }

            set {
                animator = value;
            }
        }

        public NavMeshAgent Agent {
            get {
                return agent;
            }

            set {
                agent = value;
            }
        }

        public GameObject GameObject {
            get {
                return gameObject;
            }

            set {
                gameObject = value;
            }
        }

        public CharacterMono CharacterMono {
            get {
                if (characterMono == null)
                    characterMono = gameObject.GetComponent<CharacterMono>();
                return characterMono;
            }
        }

        // 设置\获取布尔型变量
        public void SetBool(string key, bool value) {

            dictonary[key] = value;
        }
        public bool GetBool(string key) {
            return (bool)dictonary[key];
        }

        // 设置\获取Vector3变量
        public void SetVector3(string key, Vector3 value) {
            dictonary[key] = value;
        }
        public Vector3 GetVector3(string key) {
            return (Vector3)dictonary[key];
        }

        // 设置\获取Transform变量
        public void SetTransform(string key, Transform value) {
            dictonary[key] = value;
        }
        public Transform GetTransform(string key) {
            return dictonary[key] as Transform;
        }

        // 设置\获取整形变量
        public void SetInt(string key, int value) {
            dictonary[key] = value;
        }
        public int GetInt(string key) {
            return (int)dictonary[key];
        }

        // 设置\获取浮点型变量
        public void SetFloat(string key, float value) {
            dictonary[key] = value;
        }
        public float GetFloat(string key) {
            return (float)dictonary[key];
        }

        // 设置\获取GameObject对象
        public void SetGameObject(string key, GameObject value) {
            dictonary[key] = value;
        }
        public GameObject GetGameObject(string key) {
            return dictonary[key] as GameObject;
        }

        // 设置\获取Object对象
        public void SetObject(string key,object value) {
            dictonary[key] = value;
        }
        public object GetObject(string key) {
            return dictonary[key];
        }
    }
}
