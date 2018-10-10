using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GamePlay.Test {
    class AccelateTime : MonoBehaviour{

        public float timeScale;

        private void Start() {
            Time.timeScale = timeScale;
        }
    }
}
