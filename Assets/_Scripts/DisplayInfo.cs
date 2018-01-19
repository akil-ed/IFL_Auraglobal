using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInfo : MonoBehaviour {
	Firebase.Auth.FirebaseAuth auth;
	public Text UserName, Email;
	public Image UserPic;
	public System.Uri photo_url;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnEnable2(){
		Firebase.Auth.FirebaseUser user = auth.CurrentUser;
		if (user != null) {
			UserName.text = user.DisplayName;
			Email.text = user.Email;
			photo_url = user.PhotoUrl;

			//StartCoroutine (LoadImage ());
			// The user's Id, unique to the Firebase project.
			// Do NOT use this value to authenticate with your backend server, if you
			// have one; use User.TokenAsync() instead.
			string uid = user.UserId;

		}
	}



}
