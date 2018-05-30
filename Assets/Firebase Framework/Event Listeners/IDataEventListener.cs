using UnityEngine;

public class IDataEventListener : MonoBehaviour
{
    public IDataEvent Event;
    public IDataUnityEvent response;

    private void OnEnable()
    {
        Event.AddListener(Invoke);
    }

    private void OnDisable()
    {
        Event.RemoveListener(Invoke);
    }

    private void Invoke(IData data)
    {
        response.Invoke(data);
    }
}