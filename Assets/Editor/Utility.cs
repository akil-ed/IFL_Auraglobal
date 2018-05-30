using System.Linq;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Utility : Editor
{
    [MenuItem("Utlity/Enable Selected Object %&a")]
    public static void EnableCurremtObject()
    {
        foreach (Transform child in Selection.activeGameObject.transform.parent)
        {
            if (child.gameObject != Selection.activeGameObject)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    [MenuItem("Utlity/Add Namer %&n")]
    public static void AddNamer()
    {
        Selection.activeGameObject.AddComponent<Namer>();
    }

    [MenuItem("Utlity/Add Set Active Events To Button %&b")]
    public static void AddSetActiveEventsToButton()
    {
        Button button = Selection.gameObjects.First(a => a.GetComponent<Button>() != null).GetComponent<Button>();
        foreach (var item in Selection.gameObjects)
        {
            UnityAction<bool> action = null;
            if (item != button.gameObject)
            {
                action = item.SetActive;
            }
            else
            {
                action = button.transform.parent.gameObject.SetActive;
            }
            UnityEventTools.AddBoolPersistentListener(button.onClick, action, false);
        }
    }

    [MenuItem("GameObject/Create Other/Button")]
    public static void CreateButton()
    {
        EnableCurremtObject();
        GameObject button = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Button.prefab", typeof(GameObject)) as GameObject;
        button = Instantiate(button);
        button.transform.SetParent(Selection.activeTransform);
        button.transform.localPosition = Vector3.zero;
        button.transform.localScale = new Vector3(1, 1, 1);
        Selection.activeGameObject = button;
    }
}
