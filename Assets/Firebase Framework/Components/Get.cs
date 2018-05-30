using System;
using Firebase.Database;

public class Get
{
    public Get(string Path, EventHandler<ChildChangedEventArgs> OnChildAdded, EventHandler<ValueChangedEventArgs> OnValueChanged = null)
    {
        FirebaseDatabase.DefaultInstance.GetReference(Path).ChildAdded += OnChildAdded;
        if (OnValueChanged != null)
            FirebaseDatabase.DefaultInstance.GetReference(Path).ValueChanged += OnValueChanged;
    }
}