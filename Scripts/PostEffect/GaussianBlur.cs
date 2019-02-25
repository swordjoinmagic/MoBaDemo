using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GaussianBlur : MonoBehaviour {

    public Shader gaussianBlurShader;

    private Material material;

    public Material Material {
        get {
            if (material == null)
                material = new Material(gaussianBlurShader);
            return material;
        }
    }

    // 模糊处理的迭代次数
    [Range(0,4)]
    public int iterations = 3;

    // 模糊半径
    [Range(0.2f,3.0f)]
    public float blurSize = 0.6f;

    // 分辨率下降比率
    [Range(1,8)]
    public int downSample = 2;

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (Material!=null) {
            int screenWidth = source.width / downSample;
            int screenHeight = source.height / downSample;

            // 申请两个RT，用于迭代模糊
            RenderTexture r1 = RenderTexture.GetTemporary(screenWidth,screenHeight,0,source.format);
            RenderTexture r2 = RenderTexture.GetTemporary(screenWidth,screenHeight,0, source.format);

            // 将原图拷贝到降分辨率的RT上
            Graphics.Blit(source,r1);

            for (int i=0;i<iterations;i++) {
                Material.SetFloat("_BlurSize",blurSize*i+1.0f);
                // 第一次高斯模糊，竖向模糊
                Graphics.Blit(r1,r2,Material,0);

                // 第二次高斯模糊，横向模糊
                Graphics.Blit(r2,r1,Material,1);
            }

            // 输出结果
            Graphics.Blit(r1,destination);

            // 释放申请的两块renderBuffer内容
            RenderTexture.ReleaseTemporary(r1);
            RenderTexture.ReleaseTemporary(r2);
        }
    }
}
