using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorChanged : MonoBehaviour {

    public Texture2D oldCursorTexture;
    public Texture2D cursorTexture;
    CursorMode cursorMode = CursorMode.Auto;
    Vector2 hotSpot = Vector2.zero;

    // Update is called once per frame
    void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.CompareTag("Enermy")) {
                Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
            } else {
                Cursor.SetCursor(oldCursorTexture, hotSpot, cursorMode);

            }

        } else {

            Cursor.SetCursor(oldCursorTexture, hotSpot, cursorMode);
        }

    }
}
