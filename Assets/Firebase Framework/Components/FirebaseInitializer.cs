using Firebase;
using Firebase.Unity.Editor;
using UnityEngine;

public class FirebaseInitializer : MonoBehaviour
{
    [SerializeField]
    FirebaseEditorSettings settings = null;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(settings.databaseUrl);
        FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail(settings.serviceAccountEmail);
        FirebaseApp.DefaultInstance.SetEditorP12FileName(settings.p12FileName);
        FirebaseApp.DefaultInstance.SetEditorP12Password(settings.p12Password);
    }



}
