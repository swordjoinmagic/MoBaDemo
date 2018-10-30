using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 管理鼠标指针模型的单例Mono类
/// </summary>
public class MouseIconManager : MonoBehaviour {

    private static MouseIconManager instance;
    public static MouseIconManager Instace {
        get {
            if (null == instance) {
                instance = FindObjectOfType<MouseIconManager>();
            }
            return instance;
        }
    }

    public enum MouseState {
        Default,
        Attack,
        Spell,
        Up,         // 当鼠标指针向上移动的时候
        Down,
        Left,
        Right
    }

    CursorMode cursorMode = CursorMode.Auto;
    Vector2 hotSpot = Vector2.zero;

    // 默认的鼠标指针图片
    public Texture2D defaultCursorTexture;

    // 选定攻击目标时的鼠标指针图片
    public Texture2D attackCursorTexture;

    // 在释放法术选定敌人时的鼠标指针图片
    public Texture2D spellCursorTexture;

    // 箭头图片,默认为向右
    public Texture2D arrow;

    /// <summary>
    /// 复原鼠标指针为默认形态
    /// </summary>
    public void Recovery() {
        Cursor.SetCursor(defaultCursorTexture, hotSpot, cursorMode);
    }

    public Texture2D RotateArrow(float angle) {
        Texture2D newArrow = new Texture2D(arrow.width, arrow.height) {
            wrapMode = TextureWrapMode.Clamp,
        };
        //newArrow
        Color32[] color32s = arrow.GetPixels32();
        Color32[] newColor32s = new Color32[arrow.width * arrow.height];
        // 向左旋转90度即为向上
        for (int i = 0; i < arrow.height; i++) {
            for (int j = 0; j < arrow.width; j++) {

                // 角度转弧度
                float radian = angle * Mathf.Deg2Rad;
                float sin = Mathf.Sin(radian);
                float cos = Mathf.Cos(radian);
                int xc = arrow.width / 2;
                int yc = arrow.height / 2;

                int x = (int)(cos * (i - xc) + sin * (j - yc) + xc);
                int y = (int)(-sin * (i - xc) + cos * (j - yc) + yc);

                if (x > -1 && x < arrow.width && y > -1 && y < arrow.height) {
                    newColor32s[j * arrow.width + i] = color32s[y * arrow.width + x];
                }
            }
        }
        newArrow.SetPixels32(newColor32s);
        newArrow.Apply();
        byte[] bytes = newArrow.GetRawTextureData();
        FileStream fs = new FileStream("e:/a.png",FileMode.OpenOrCreate);
        fs.Write(bytes,0,bytes.Length);
        fs.Flush();
        fs.Close();
        return newArrow;
    }

    public void ChangeMouseIcon(MouseState mouseState) {
        switch (mouseState) {
            case MouseState.Default:
                Cursor.SetCursor(defaultCursorTexture, hotSpot, cursorMode);
                break;
            case MouseState.Attack:
                Cursor.SetCursor(attackCursorTexture, hotSpot, cursorMode);
                break;
            case MouseState.Spell:
                Cursor.SetCursor(spellCursorTexture, hotSpot, cursorMode);
                break;
            case MouseState.Up:
                Cursor.SetCursor(RotateArrow(90), hotSpot, cursorMode);
                break;
            case MouseState.Left:
                Cursor.SetCursor(RotateArrow(180), hotSpot, cursorMode);
                break;
            case MouseState.Right:
                Cursor.SetCursor(arrow, hotSpot, cursorMode);
                break;
            case MouseState.Down:
                Cursor.SetCursor(RotateArrow(270), hotSpot, cursorMode);
                break;
        }
    }
}
