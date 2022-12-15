using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventTrigger : BaseTrigger
{
    public SimpleEventsHandler<LevelEventTrigger,bool> EnterEvent;
    public LevelEventType EventType;
    [Tooltip("Usage depends on type. For Text - containerID. For Pickup - itemID")]public string ContentIDString;

    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        EnterEvent?.Invoke(this,true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        EnterEvent?.Invoke(this,false);
    }
}
