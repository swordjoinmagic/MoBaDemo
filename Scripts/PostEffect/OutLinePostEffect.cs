using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class OutLinePostEffect : MonoBehaviour {
    // 用于渲染描边人物的纯色Shader
    public Shader outlineSoliderColorShader;
    private Material outlineSoliderMaterial;

    public Material OutlineSoliderMaterial {
        get {
            if (outlineSoliderMaterial == null) {
                outlineSoliderMaterial = new Material(outlineSoliderColorShader);
            }
            return outlineSoliderMaterial;
        }
    }

    public Material GaussianBlurMaterial {
        get {
            if (gaussianBlurMaterial == null) {
                gaussianBlurMaterial = new Material(gaussianBlurShader);
            }
            return gaussianBlurMaterial;
        }
    }

    private Material OutlineSubstractionMaterial {
        get {
            if (outlineSubstractionMaterial == null) {
                outlineSubstractionMaterial = new Material(outlineSubstractionShader);
            }
            return outlineSubstractionMaterial;
        }
    }

    public Material OutlineAddtionalMaterial {
        get {
            if (outlineAddtionalMaterial == null) {
                outlineAddtionalMaterial = new Material(outlineAdditionalShader);
            }
            return outlineAddtionalMaterial;
        }
    }

    public GameObject TargetObject {
        get {
            return targetObject;
        }

        set {
            targetObject = value;

            commandBuffer.ClearRenderTarget(true, true, Color.black);
            if (targetObject != null) {
                // 将目标物体的所有render都扔到CommandBuffer里面去
                Renderer[] renderers = value.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers) {
                    commandBuffer.DrawRenderer(r, OutlineSoliderMaterial);
                }
            }
        }
    }

    // 用于高斯模糊的Shader
    public Shader gaussianBlurShader;
    private Material gaussianBlurMaterial;

    // 用于将高斯模糊后的图和原图进行相减的描边Shader
    public Shader outlineSubstractionShader;
    private Material outlineSubstractionMaterial;

    // 用于将轮廓图和场景图进行叠加的Shader
    public Shader outlineAdditionalShader;
    private Material outlineAddtionalMaterial;

    // 要进行描边的目标对象
    private GameObject targetObject;

    // CommandBuffer的渲染目标
    private RenderTexture renderTexture;
    private CommandBuffer commandBuffer;

    //==========================
    // 用于高斯模糊

    // 分辨率下降的比率
    public int downSample = 1;

    // 模糊迭代次数
    public int iteration = 2;

    // 模糊半径
    [Range(0.2f, 3.0f)]
    public float blurSize = 0.6f;


    //============================
    // 用于描边

    // 描边颜色
    public Color outLineColor = Color.green;
    // 描边强度
    [Range(0.0f,10.0f)]
    public float outLineStrength = 3.0f;


    private void OnEnable() {
        renderTexture = RenderTexture.GetTemporary(Screen.width/downSample,Screen.height/downSample,0);

        // 创建用于渲染纯色目标RT的CommandBuffer
        commandBuffer = new CommandBuffer();
        commandBuffer.SetRenderTarget(renderTexture);
        commandBuffer.ClearRenderTarget(true,true,Color.black);

    }

    public void ClearRenderTarget() {
        commandBuffer.ClearRenderTarget(true, true, Color.black);
    }


    private void OnDisable() {
        if (renderTexture != null) {
            RenderTexture.ReleaseTemporary(renderTexture);
            renderTexture = null;
        }
        if (commandBuffer != null) {
            commandBuffer.Release();
            commandBuffer = null;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (OutlineSoliderMaterial != null && OutlineSubstractionMaterial != null && GaussianBlurMaterial != null) {

            // 设置描边颜色
            OutlineSoliderMaterial.SetColor("_OutlineColor", outLineColor);

            // 通过Graphic执行Command Buffer
            Graphics.ExecuteCommandBuffer(commandBuffer);

            // 对渲染完成的目标纯色RT进行模糊处理(将其变宽)
            RenderTexture temp1 = RenderTexture.GetTemporary(source.width / downSample, source.height / downSample, 0);
            RenderTexture temp2 = RenderTexture.GetTemporary(source.width / downSample, source.height / downSample, 0);

            // 对该图片进行纵向高斯模糊
            Graphics.Blit(renderTexture, temp1, GaussianBlurMaterial, 0);
            // 对该图片进行横向高斯模糊
            Graphics.Blit(temp1, temp2, GaussianBlurMaterial, 1);

            // 对该纯色图片进行迭代模糊
            for (int i = 0; i < iteration; i++) {
                GaussianBlurMaterial.SetFloat("_BlurSize", blurSize * i + 1.0f);
                // 纵向
                Graphics.Blit(temp2, temp1, GaussianBlurMaterial, 0);
                // 横向
                Graphics.Blit(temp1, temp2, GaussianBlurMaterial, 1);
            }

            // 将模糊后的图片减去原图得到物体的轮廓图
            OutlineSubstractionMaterial.SetTexture("_BlurTex", temp2);
            Graphics.Blit(renderTexture, temp1, OutlineSubstractionMaterial);

            // 轮廓图和场景图进行叠加
            OutlineAddtionalMaterial.SetTexture("_BlurTex", temp1);      // 轮廓图
            OutlineAddtionalMaterial.SetFloat("_OutlineStrength", outLineStrength);  // 边缘强度
            Graphics.Blit(source, destination, OutlineAddtionalMaterial);

            RenderTexture.ReleaseTemporary(temp1);
            RenderTexture.ReleaseTemporary(temp2);
        } else {
            Graphics.Blit(source,destination);
        }
    }
}
