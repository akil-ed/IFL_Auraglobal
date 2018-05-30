using Firebase.Database;
using UnityEngine;

public static class Extensions
{
    public static T ToClass<T>(this DataSnapshot snapshot)
    {
        return JsonUtility.FromJson<T>(snapshot.GetRawJsonValue());
    }
}
