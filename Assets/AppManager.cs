using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Messaging;
//using UnityEditor;

public class AppManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//PlayerSettings.statusBarHidden = false;
		Screen.fullScreen = false;

		if (!PlayerPrefs.HasKey ("isPush")) {
			FirebaseMessaging.Subscribe ("Notification");
			PlayerPrefs.SetInt ("isPush", 1);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
