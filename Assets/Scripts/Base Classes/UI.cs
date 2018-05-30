using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UI : MonoBehaviour, IView, IPointerClickHandler
{
    public IDataEvent Event;

    public void OnPointerClick(PointerEventData eventData)
    {
        Event.Fire(data);
    }
    public abstract IData data { get; }
    public abstract void Display(string str);
}
