using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/Event")]
public class Event : ScriptableObject
{
    List<ParameterlessDelegate> listeners = new List<ParameterlessDelegate>();

    public void AddListener(ParameterlessDelegate listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(ParameterlessDelegate listener)
    {
        listeners.Remove(listener);
    }

    public void Fire()
    {
        foreach (var listener in listeners)
        {
            listener.Invoke();
        }
    }
}

public delegate void ParameterlessDelegate();