using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.AsstesBundles {    
    public class CreateAssetBundles {

        [MenuItem("Assets/Build AssetBundles")]
        static void BuildAllAssetBundles() {
            BuildPipeline.BuildAssetBundles(
                Application.dataPath+"/AssetBundle/", 
                BuildAssetBundleOptions.ChunkBasedCompression,
                BuildTarget.StandaloneWindows
                );           
        }

        [MenuItem("Assets/Load AssetBundles")]
        static void Load() {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.dataPath + "/AssetBundle/skilldata");
            Debug.Log(assetBundle);

            object[] vs = assetBundle.LoadAllAssets();
            foreach (var i in vs) {
                Debug.Log(i.GetType().ToString());
            }
        }

    }
}
