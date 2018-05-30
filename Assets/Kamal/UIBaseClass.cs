using DoozyUI;
using UnityEngine;

public class UIBaseClass : MonoBehaviour
{
    protected void Show(UIElement element)
    {
        element.gameObject.SetActive(true);
        element.Show(false);
    }

    protected void Hide(UIElement element)
    {
        element.Hide(false);
    }

    protected void SetActive(GameObject obj, bool val)
    {
        obj.SetActive(val);
    }

    protected UI CreateUIElementIntoCanvas(UI element, Transform parent, string json)
    {
        var instance = Instantiate(element);
        instance.Display(json);
        instance.transform.SetParent(parent);
        return instance;
    }
}