using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    public Event Event;

    public UnityEvent reponse;

    private void Awake()
    {
        Event.AddListener(Invoke);
    }

    private void OnDisable()
    {
        Event.RemoveListener(Invoke);
    }

    private void Invoke()
    {
        reponse.Invoke();
    }
}