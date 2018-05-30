using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Object/Firebase Settings")]
public class FirebaseEditorSettings : ScriptableObject
{
	public string databaseUrl;
	public string serviceAccountEmail;
	public string p12FileName;
	public string p12Password;
}
