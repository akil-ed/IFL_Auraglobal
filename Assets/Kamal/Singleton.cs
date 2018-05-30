using UnityEngine;

public class Singleton<T> : UIBaseClass where T : MonoBehaviour
{
    public static T Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
