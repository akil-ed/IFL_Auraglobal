using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/StringEvent")]
public class IDataEvent : ScriptableObject
{
    List<IDataDelegate> dListeners = new List<IDataDelegate>();

    public void AddListener(IDataDelegate listener)
    {
        dListeners.Add(listener);
    }

    public void RemoveListener(IDataDelegate listener)
    {
        dListeners.Remove(listener);
    }

    public void Fire(IData str)
    {
        foreach (var d in dListeners)
        {
            d.Invoke(str);
        }
    }
}


public delegate void IDataDelegate(IData str);