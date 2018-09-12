using System.Collections;
using System.Collections.Generic;
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
        Spell
    }

    CursorMode cursorMode = CursorMode.Auto;
    Vector2 hotSpot = Vector2.zero;

    // 默认的鼠标指针图片
    public Texture2D defaultCursorTexture;

    // 选定攻击目标时的鼠标指针图片
    public Texture2D attackCursorTexture;

    // 在释放法术选定敌人时的鼠标指针图片
    public Texture2D spellCursorTexture;

    /// <summary>
    /// 复原鼠标指针为默认形态
    /// </summary>
    public void Recovery() {
        Cursor.SetCursor(defaultCursorTexture, hotSpot, cursorMode);
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
        }
    }
}
