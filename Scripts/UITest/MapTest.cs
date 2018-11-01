using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapTest : MonoBehaviour, IPointerClickHandler {

    //监听点击
    public void OnPointerClick(PointerEventData eventData) {
        PassEvent(eventData, ExecuteEvents.pointerClickHandler);
    }


    //把事件透下去
    public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
        where T : IEventSystemHandler {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        GameObject current = data.pointerCurrentRaycast.gameObject;
        Debug.Log("ResultsCount:"+results.Count);
        for (int i = 0; i < results.Count; i++) {
            if (current != results[i].gameObject) {
                ExecuteEvents.Execute(results[i].gameObject, data, function);
            }
        }
    }


}