using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEventFuc
{
    /// <summary>
    /// 为gameobject注册点击事件
    /// </summary>
    /// <param name="component"></param>
    /// <param name="callBack"></param>
    public static void RegisterClickEvent(GameObject component, UnityAction<BaseEventData> callBack)
    {
        if (component == null)
        {
            return;
        }
        if (callBack == null)
        {
            Debug.LogError("NO FUNCTION");
            return;
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(callBack);
        EventTrigger eventTrigger = component.GetComponent<EventTrigger>();
        if (eventTrigger != null)
        {
            ScrollRect list = null;
            if (list == null)
            {
                Transform t = component.transform;
                while (t != null)
                {
                    list = t.GetComponent<ScrollRect>();
                    if (list != null)
                    {
                        break;
                    }
                    t = t.parent;
                }
            }
            foreach (var trigger in eventTrigger.triggers)
            {
                if (trigger.eventID == EventTriggerType.BeginDrag)
                {
                    trigger.callback.AddListener((BaseEventData data2) =>
                    {
                        list.OnBeginDrag(data2 as PointerEventData);
                    });
                }
                else if (trigger.eventID == EventTriggerType.Drag)
                {
                    trigger.callback.AddListener((BaseEventData data2) =>
                    {
                        list.OnDrag(data2 as PointerEventData);
                    });
                }
                else if (trigger.eventID == EventTriggerType.EndDrag)
                {
                    trigger.callback.AddListener((BaseEventData data2) =>
                    {
                        list.OnEndDrag(data2 as PointerEventData);
                    });
                }
            }
        }
        if (eventTrigger == null)
        {
            eventTrigger = component.AddComponent<EventTrigger>();
        }
        eventTrigger.triggers.Add(entry);
    }

    /// <summary>
    /// 为gameobject注册点击事件
    /// </summary>
    /// <param name="component"></param>
    /// <param name="callBack"></param>
    public static void RegisterClickEvent(GameObject component, UnityAction callBack)
    {
        if (component == null)
        {
            return;
        }
        if (callBack == null)
        {
            Debug.LogError("NO FUNCTION");
            return;
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((BaseEventData data) =>
        {
            callBack.Invoke();
        });
        EventTrigger eventTrigger = component.GetComponent<EventTrigger>();
        if (eventTrigger != null)
        {
            ScrollRect list = null;
            if (list == null)
            {
                Transform t = component.transform;
                while (t != null)
                {
                    list = t.GetComponent<ScrollRect>();
                    if (list != null)
                    {
                        break;
                    }
                    t = t.parent;
                }
            }
            foreach (var trigger in eventTrigger.triggers)
            {
                if (trigger.eventID == EventTriggerType.BeginDrag)
                {
                    trigger.callback.AddListener((BaseEventData data2) =>
                    {
                        list.OnBeginDrag(data2 as PointerEventData);
                    });
                }
                else if (trigger.eventID == EventTriggerType.Drag)
                {
                    trigger.callback.AddListener((BaseEventData data2) =>
                    {
                        list.OnDrag(data2 as PointerEventData);
                    });
                }
                else if (trigger.eventID == EventTriggerType.EndDrag)
                {
                    trigger.callback.AddListener((BaseEventData data2) =>
                    {
                        list.OnEndDrag(data2 as PointerEventData);
                    });
                }
            }
        }
        if (eventTrigger == null)
        {
            eventTrigger = component.AddComponent<EventTrigger>();
        }
        eventTrigger.triggers.Add(entry);
    }

    /// <summary>
    /// 通用的注册ui事件
    /// </summary>
    /// <param name="component"></param>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void RegisterEvent(GameObject component, EventTriggerType eventType, UnityAction<BaseEventData> callBack)
    {
        if (component == null)
        {
            return;
        }
        if (callBack == null)
        {
            Debug.LogError("NO FUNCTION");
            return;
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(callBack);
        EventTrigger eventTrigger = component.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = component.AddComponent<EventTrigger>();
        }
        eventTrigger.triggers.Add(entry);
    }

}

