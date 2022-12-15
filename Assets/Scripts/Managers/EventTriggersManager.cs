using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggersManager : MonoBehaviour
{
    [SerializeField] private List<LevelEventTrigger> triggers = new List<LevelEventTrigger>();
    private GameTextComp _text;


    private void Awake()
    {
        triggers.AddRange(FindObjectsOfType<LevelEventTrigger>());
        foreach (var trigger in triggers)
        { trigger.EnterEvent += OnEventActivated; }
    }
    private void OnDisable()
    {
        foreach (var t in triggers) { t.EnterEvent -= OnEventActivated; }
    }

    private void Start()
    {
        _text = FindObjectOfType<GameTextComp>();
        _text.IsShown = false;
    }

    private void OnEventActivated(LevelEventTrigger tr, bool isEnter)
    {

        switch (tr.EventType)
        {
            case LevelEventType.TextDisplay:
                if (isEnter)
                {
                    _text.IsShown  = true;
                    _text.SetText(TextsManager.GetInstance.GetContainerByID(tr.ContentIDString));
                }
                else
                {
                    _text.IsShown = false;
                }
                break;
            case LevelEventType.LevelComplete:
                GameManager.GetInstance.OnLevelComplete();
                break;
            case LevelEventType.Cutscene:
                break;
            case LevelEventType.ItemPickup:
                GameManager.GetInstance.OnItemPickup(tr.ContentIDString);
                break;
            default:
                Debug.LogWarning($"{this} can't handle event of type {tr.EventType}");
                break;
        }
    }
}
