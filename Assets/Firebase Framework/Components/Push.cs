using System;
using Firebase.Database;
using UnityEngine;

public class Push
{
    public Push(string Path, IData data, Action Success = null, Action<string> Failed = null)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(Path).Push();
        data.ID = reference.Key;
        reference.SetRawJsonValueAsync(JsonUtility.ToJson(data)).ContinueWith(result =>
        {
            if (result.IsCompleted)
            {
                Success();
            }
            else
            {
                Failed(result.Exception.Message);
            }
        });
    }
}
