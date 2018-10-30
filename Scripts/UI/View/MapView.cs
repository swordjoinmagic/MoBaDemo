using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class MapView : MonoBehaviour {

    private EventTrigger eventTrigger;

    private void Start() {
        eventTrigger = GetComponent<EventTrigger>();
        var clickEntry = new EventTrigger.Entry();
        clickEntry.eventID = EventTriggerType.PointerClick;
        clickEntry.callback.AddListener(eventdata => {
            
        });
    }
}
