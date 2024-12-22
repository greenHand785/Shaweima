using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventCenter
{
    public EventCenter()
    {
        EventTable.Clear();
    }
    public static Dictionary<CombatEventType, Delegate> EventTable = new Dictionary<CombatEventType, Delegate>();

    public static void OnListenerAdding(CombatEventType eventType, Delegate callback)
    {
        if (!EventTable.ContainsKey(eventType))
        {
            EventTable.Add(eventType, null);
        }
        Delegate d = EventTable[eventType];
        if (d != null && d.GetType() != callback.GetType())
        {
            throw new Exception($"添加事件失败：事件{eventType}对应的类型为{d.GetType()}，尝试添加的类型为{callback.GetType()}");
        }
    }

    public static void OnListenerRemoving(CombatEventType eventType, Delegate callback)
    {
        if (EventTable.ContainsKey(eventType))
        {
            EventTable.TryGetValue(eventType, out Delegate d);
            if (d == null)
            {
                throw new Exception($"移除事件失败：事件码{eventType}没有对应的事件");
            }
            else if (d.GetType() != callback.GetType())
            {
                throw new Exception($"移除事件失败：事件码{eventType}对应的类型为{d.GetType()}，尝试移除的类型为{callback.GetType()}");
            }
        }
        else
        {
            throw new Exception($"移除事件失败：没有对应的事件码{eventType}");
        }
    }

    internal static void Broadcast<T>(CombatEventType event_OnAfterShiftingObj, object onAfterShiftingObj)
    {
        throw new NotImplementedException();
    }

    public static void OnListenerRemoved(CombatEventType eventType)
    {
        if (EventTable[eventType] == null)
            EventTable.Remove(eventType);
    }

    public static void AddListener(CombatEventType eventType, Callback callback)
    {
        OnListenerAdding(eventType, callback);
        EventTable[eventType] = (Callback)EventTable[eventType] + callback;
    }

    public static void AddListener<T>(CombatEventType eventType, Callback<T> callback)
    {
        OnListenerAdding(eventType, callback);
        EventTable[eventType] = (Callback<T>)EventTable[eventType] + callback;
    } 

    public static void AddListener<T0, T1>(CombatEventType eventType, Callback<T0, T1> callback)
    {
        OnListenerAdding(eventType, callback);
        EventTable[eventType] = (Callback<T0, T1>)EventTable[eventType] + callback;
    }
    public static void AddListener<T0, T1, T2>(CombatEventType eventType, Callback<T0, T1, T2> callback)
    {
        OnListenerAdding(eventType, callback);
        EventTable[eventType] = (Callback<T0, T1, T2>)EventTable[eventType] + callback;
    }

    public static void AddListener<T0, T1, T2, T3>(CombatEventType eventType, Callback<T0, T1, T2, T3> callback)
    {
        OnListenerAdding(eventType, callback);
        EventTable[eventType] = (Callback<T0, T1, T2, T3>)EventTable[eventType] + callback;
    }

    public static void RemoveListener(CombatEventType eventType, Callback callback)
    {
        OnListenerRemoving(eventType, callback);
        EventTable[eventType] = (Callback)EventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }

    public static void RemoveListener<T>(CombatEventType eventType, Callback<T> callback)
    {
        OnListenerRemoving(eventType, callback);
        EventTable[eventType] = (Callback<T>)EventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }

    public static void RemoveListener<T0, T1>(CombatEventType eventType, Callback<T0, T1> callback)
    {
        OnListenerRemoving(eventType, callback);
        EventTable[eventType] = (Callback<T0, T1>)EventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }

    public static void RemoveListener<T0, T1, T2>(CombatEventType eventType, Callback<T0, T1, T2> callback)
    {
        OnListenerRemoving(eventType, callback);
        EventTable[eventType] = (Callback<T0, T1, T2>)EventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }

    public static void RemoveListener<T0, T1, T2, T3>(CombatEventType eventType, Callback<T0, T1, T2, T3> callback)
    {
        OnListenerRemoving(eventType, callback);
        EventTable[eventType] = (Callback<T0, T1, T2, T3>)EventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }

    public static void Broadcast(CombatEventType eventType)
    {
        if (EventTable.TryGetValue(eventType, out Delegate d))
        {
            Callback callback = d as Callback;
            callback?.Invoke();//如果不为空调用，unity2017以下不可简写
        }
    }

    public static void Broadcast<T>(CombatEventType eventType, T t)
    {
        if (EventTable.TryGetValue(eventType, out Delegate d))
        {
            Callback<T> callback = d as Callback<T>;
            callback?.Invoke(t);
        }
    }

    public static void Broadcast<T0, T1>(CombatEventType eventType, T0 t, T1 e)
    {
        if (EventTable.TryGetValue(eventType, out Delegate d))
        {
            Callback<T0, T1> callback = d as Callback<T0, T1>;
            callback?.Invoke(t, e);
        }
    }

    public static void Broadcast<T0, T1, T2>(CombatEventType eventType, T0 t, T1 e, T2 l)
    {
        if (EventTable.TryGetValue(eventType, out Delegate d))
        {
            Callback<T0, T1, T2> callback = d as Callback<T0, T1, T2>;
            callback?.Invoke(t, e, l);
        }
    }

    public static void Broadcast<T0, T1, T2, T3>(CombatEventType eventType, T0 t, T1 e, T2 l, T3 w)
    {
        if (EventTable.TryGetValue(eventType, out Delegate d))
        {
            Callback<T0, T1, T2, T3> callback = d as Callback<T0, T1, T2, T3>;
            callback?.Invoke(t, e, l, w);
        }
    }

    internal static void Broadcast(CombatEventType event_PostAerial, object postAeiral)
    {
        throw new NotImplementedException();
    }
}
