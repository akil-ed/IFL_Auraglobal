using System.Collections.Generic;
using System.Linq;
using Firebase.Database;
using UnityEngine;

public class BaseDisplaySystem : MonoBehaviour
{
    public string path;
    public UI prefab;
    public Transform parent;
    public List<UI> instances;

    protected void ChildAdded(object sender, ChildChangedEventArgs e)
    {
        UI instance = Instantiate(prefab);
        instances.Add(instance);
        instance.Display(e.Snapshot.GetRawJsonValue());
        instance.transform.SetParent(parent);
        instance.transform.localScale = Vector3.one;
    }

    public void Clear()
    {
        foreach (var item in instances.ToList())
        {
            Destroy(item.gameObject);
        }
        instances = new List<UI>();
    }
}