using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using DG.Tweening;
using DoozyUI;
using Facebook.Unity;
using Firebase.Messaging;

using Newtonsoft.Json;


public class AuthenticationManager : MonoBehaviour {
	Firebase.Auth.FirebaseAuth auth;
	public static Firebase.Auth.FirebaseUser user;

	Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

	public InputField RegisterUserName, RegisterEmail, RegisterPassword, RegisterCPassword;
	public InputField Email, Password;
	public InputField ResetEmail;
	public Text logText;
	public static string UserEmail="Editor";
	public static string TeamName="EditorTeam";

	public UIElement HomePage, SignInPage, SignUpPage, MainPage, LoadingPage;
	public DisplayInfo _DisplayInfo;

	void Start() {
		
		dependencyStatus = Firebase.FirebaseApp.CheckDependencies();
		if (dependencyStatus != Firebase.DependencyStatus.Available) {
			Firebase.FirebaseApp.FixDependenciesAsync().ContinueWith(task => {
				dependencyStatus = Firebase.FirebaseApp.CheckDependencies();
				if (dependencyStatus == Firebase.DependencyStatus.Available) {
					InitializeFirebase();
				} else {
					Debug.LogError(
						"Could not resolve all Firebase dependencies: " + dependencyStatus);
				}
			});
		} else {
			InitializeFirebase();
		}


	}

	void InitializeFirebase() {

		StartCoroutine (LoadPage ());
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.StateChanged += AuthStateChanged;
		AuthStateChanged(this, null);

	}


	void OnDestroy() {
		auth.StateChanged -= AuthStateChanged;
		auth = null;
	}
	Sprite test;
	public void DebugLog(string s) {
		UIManager.ShowNotification("Example_1_Notification_4", 1.5f, true, s, test);
	}
	bool isLoading;
	IEnumerator LoadPage()
	{

		isLoading = true;
		LoadingPage.Show (false);

		while(isLoading)
			yield return null;
		LoadingPage.Hide (false);
	}

	void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo, int indentLevel) {
		string indent = new String(' ', indentLevel * 2);
		var userProperties = new Dictionary<string, string> {
			{"Display Name", userInfo.DisplayName},
			{"Email", userInfo.Email},
			{"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null},
			{"Provider ID", userInfo.ProviderId},
			{"User ID", userInfo.UserId}
		};

		foreach (var property in userProperties) {
			if (!String.IsNullOrEmpty(property.Value)) {
				//DebugLog(String.Format("{0}{1}: {2}", indent, property.Key, property.Value));
			}
		}

	}

	// Track state changes of the auth object.
	void AuthStateChanged(object sender, System.EventArgs eventArgs) {
		isLoading = false;
		if (auth.CurrentUser != user) {
			bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
			if (!signedIn && user != null) {
				//DebugLog("Signed out " + user.UserId);
			}
			user = auth.CurrentUser;
			if (signedIn) {

				DisplayUserInfo(user, 1);

				if(!FB.IsLoggedIn)
					OpenMainPage ();

				var providerDataList = new List<Firebase.Auth.IUserInfo>(user.ProviderData);
				if (providerDataList.Count > 0) {
					//	DebugLog("  Provider Data:");
					foreach (var providerData in user.ProviderData) {
						DisplayUserInfo(providerData, 2);
					}
				}
			}
		}
	}

	bool LogTaskCompletion(Task task, string operation) {
		bool complete = false;
		isLoading = false;
		if (task.IsCanceled) {
			DebugLog(operation + " canceled.");
		} else if (task.IsFaulted) {
			DebugLog(operation + " encounted an error.");
			DebugLog(task.Exception.ToString());
		} else if (task.IsCompleted) {
	
			complete = true;


			//OpenMainPage ();
		}
		return complete;
	}

	public void CreateUser() {
		StartCoroutine (LoadPage ());

		DebugLog(String.Format("Attempting to create user {0}...", RegisterEmail.text));
		string newDisplayName = RegisterUserName.text;
		auth.CreateUserWithEmailAndPasswordAsync(RegisterEmail.text, RegisterPassword.text)
			.ContinueWith((task) => HandleCreateResult(task, newDisplayName: newDisplayName));
	}

	void HandleCreateResult(Task<Firebase.Auth.FirebaseUser> authTask,
		string newDisplayName = null) {
		if (LogTaskCompletion(authTask, "User Creation")) {
			if (auth.CurrentUser != null) {
				//	DebugLog(String.Format("User Info: {0}  {1}", auth.CurrentUser.Email,
				//		auth.CurrentUser.ProviderId));
				UpdateUserProfile(newDisplayName: newDisplayName);
			}
		}
	}

	// Update the user's display name with the currently selected display name.
	public void UpdateUserProfile(string newDisplayName = null) {
		if (user == null) {
			DebugLog("Not signed in, unable to update user profile");
			return;
		}
		//RegisterUserName.text = newDisplayName ?? RegisterUserName.text;
		//	DebugLog("Updating user profile");
		user.UpdateUserProfileAsync(new Firebase.Auth.UserProfile {
			DisplayName = RegisterUserName.text,
			PhotoUrl = user.PhotoUrl,
		}).ContinueWith(HandleUpdateUserProfile);

		UserData UData = new UserData ();

		UData.DisplayName = RegisterUserName.text;
		UData.Email = user.Email;
		UData.UserID = user.UserId;
		UData.isEmailVerified = user.IsEmailVerified;
		UData.Balance = 500;

		UpdateUserDb (UData);
	}

	public void UpdateUserDb(UserData UData){
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		reference.Child ("Users").Child (UData.UserID)
			.SetRawJsonValueAsync (JsonConvert.SerializeObject (UData));
	
	}


	void HandleUpdateUserProfile(Task authTask) {

		if (LogTaskCompletion(authTask, "User profile")) {
			DisplayUserInfo(user, 1);
		}
	}

	public void Signin() {
		StartCoroutine (LoadPage ());

		DebugLog(String.Format("Attempting to sign in as {0}...", Email.text));
		auth.SignInWithEmailAndPasswordAsync(Email.text, Password.text)
			.ContinueWith(HandleSigninResult);
	}

	public void LoginFB(){
		StartCoroutine (LoadPage ());
		StartCoroutine (FBSignIn ());
	}

	IEnumerator FBSignIn(){
		FB.Init ();

		while (!FB.IsInitialized)
			yield return null;

		if(!FB.IsLoggedIn)
			FB.LogInWithPublishPermissions ();
		while (!FB.IsLoggedIn)
			yield return null;
		FB.API ("me?fields=id,name,picture,email", HttpMethod.GET, GotMyInformationCallback);
	}
	MyFacebookInfo profile;
	void GotMyInformationCallback(IGraphResult result)
	{
		print (result.RawResult);
		if(result.Error!=null)
		{
			print (result.Error);
		}
		else
		{
			profile = JsonUtility.FromJson<MyFacebookInfo>(result.RawResult);

			/*
			FB.API("/me?fields=email", HttpMethod.GET, graphResult =>
				{
					if (string.IsNullOrEmpty(graphResult.Error) == false)
					{
						Debug.Log("could not get email address");
						return;
					}

					profile.email = graphResult.ResultDictionary["email"] as string;
					DebugLog (profile.email);
				});

*/
			//StartCoroutine (LoadImage (profile.picture.data.url));
			//Login (profile.id);
			SigninWithFB (AccessToken.CurrentAccessToken.TokenString);
		}
	}


	void SigninWithFB(string accessToken){
		isLoading = false;
		Firebase.Auth.Credential credential =
			Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);
		auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
			if (task.IsCanceled) {
				DebugLog("SignInWithCredentialAsync was canceled.");
				return;
			}
			if (task.IsFaulted) {
				DebugLog("SignInWithCredentialAsync encountered an error: " + task.Exception);
				Debug.Log(task.Exception);
				return;
			}
			Firebase.Auth.FirebaseUser newUser = task.Result;
			//FBUser();


			//OpenMainPage();

			UpdateFBUser();

			Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.UserId, newUser.DisplayName);
		});
	}

	void SaveFBUSerData(){
		UserData UData = new UserData ();

		UData.DisplayName = profile.name;
		UData.Email = "Facebook account";
		UData.UserID = user.UserId;
		UData.isEmailVerified = user.IsEmailVerified;
		UData.Balance = 500;

		UpdateUserDb (UData);
	}

	void UpdateFBUser(){
		Firebase.Auth.FirebaseUser user = auth.CurrentUser;
		Firebase.Auth.UserProfile test;
		string imageURL=profile.picture.data.url;
		/*
		user.UpdateEmailAsync (profile.email).ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.LogError("UpdateUserProfileAsync was canceled.");
				return;
			}
			if (task.IsFaulted) {
				Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
				return;
			}
		});

*/
		if (user != null) {
			Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile {
				DisplayName = user.DisplayName,
				//	Email = user.Email,
				PhotoUrl = new System.Uri(imageURL),
			};
			user.UpdateUserProfileAsync(profile).ContinueWith(task => {
				if (task.IsCanceled) {
					Debug.LogError("UpdateUserProfileAsync was canceled.");
					return;
				}
				if (task.IsFaulted) {
					Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
					return;
				}
				SaveFBUSerData();
				OpenMainPage();
				Debug.Log("User profile updated successfully.");
			});
		}

	}

	public void InviteFriends(){
		FB.Mobile.AppInvite(new Uri("https://fb.me/1594202067539799"), callback: this.HandleResult);
	}

	void HandleResult(IAppInviteResult Result){
		Debug.Log (Result.RawResult);
	}

	// This is functionally equivalent to the Signin() function.  However, it
	// illustrates the use of Credentials, which can be aquired from many
	// different sources of authentication.
	public void SigninWithCredential() {
		DebugLog(String.Format("Attempting to sign in as {0}...", Email.text));
		//DisableUI();
		Firebase.Auth.Credential cred = Firebase.Auth.EmailAuthProvider.GetCredential(Email.text, Password.text);
		auth.SignInWithCredentialAsync(cred).ContinueWith(HandleSigninResult);
	}

	// Attempt to sign in anonymously.
	public void SigninAnonymously() {
		StartCoroutine (LoadPage ());
		DebugLog("Attempting to sign anonymously...");
		auth.SignInAnonymouslyAsync().ContinueWith(HandleSigninResult);
	}

	void HandleSigninResult(Task<Firebase.Auth.FirebaseUser> authTask) {
		LogTaskCompletion(authTask, "Sign-in");
	}

	public void ReloadUser() {
		if (user == null) {
			DebugLog("Not signed in, unable to reload user.");
			return;
		}
		DebugLog("Reload User Data");
		user.ReloadAsync().ContinueWith(HandleReloadUser);
	}

	void HandleReloadUser(Task authTask) {
		if (LogTaskCompletion(authTask, "Reload")) {
			DisplayUserInfo(user, 1);
		}
	}

	public void GetUserToken() {
		if (user == null) {
			DebugLog("Not signed in, unable to get token.");
			return;
		}
		DebugLog("Fetching user token");
		user.TokenAsync(false).ContinueWith(HandleGetUserToken);
	}

	void HandleGetUserToken(Task<string> authTask) {
		if (LogTaskCompletion(authTask, "User token fetch")) {
			//DebugLog("Token = " + authTask.Result);
		}
	}

	public void SignOut() {
		DebugLog("Signing out.");
		auth.SignOut();

		if (FB.IsLoggedIn)
			FB.LogOut ();

		//MainPage.Hide (false);
		//	SignUpPage.Hide (false);
		//	SignInPage.Hide (false);
		//HomePage.Show (false);
		HomePage.gameObject.SetActive(true);
		HomePage.Show (false);
	}


	public void DeleteUser() {
		if (auth.CurrentUser != null) {
			DebugLog(String.Format("Attempting to delete user {0}...", auth.CurrentUser.UserId));
			auth.CurrentUser.DeleteAsync().ContinueWith(HandleDeleteResult);
		} else {
			DebugLog("Sign-in before deleting user.");
		}
	}

	void HandleDeleteResult(Task authTask) {
		LogTaskCompletion(authTask, "Delete user");
	}


	// Send a password reset email to the current email address.
	public void SendPasswordResetEmail() {
		auth.SendPasswordResetEmailAsync(ResetEmail.text).ContinueWith((authTask) => {
			if (LogTaskCompletion(authTask, "Send Password Reset Email")) {
				DebugLog("Password reset email sent to " + ResetEmail.text);
			}
		});
	}


	public void WriteData(){
		//float test = 0.2f;
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://financial-app-cff5d.firebaseio.com");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		FirebaseDatabase.DefaultInstance
			.GetReference ("Value")
			.Child ("google")
			.SetValueAsync (0.2);

	}
	public void ReadTestData(string StockName) {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://next-2-percent.firebaseio.com");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		FirebaseDatabase.DefaultInstance
			.GetReference("Predictions/"+StockName)		
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					// Handle the error...
					DebugLog(task.Exception.ToString ());
				}
				else if (task.IsCompleted) {

					DataSnapshot snapshot = task.Result;
					DebugLog("Past Value: "+snapshot.Child ("Past").Value);
					DebugLog("Current Value: "+snapshot.Child ("Current").Value);
				}
			});
	}


	public void ReadData(){

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://financial-app-cff5d.firebaseio.com");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		FirebaseDatabase.DefaultInstance
			.GetReference("Value")
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					// Handle the error...
					DebugLog(task.Exception.ToString ());
				}
				else if (task.IsCompleted) {

					DataSnapshot snapshot = task.Result;
					DebugLog("Google: "+snapshot.Child ("google").Value);
				}
			});

	}

	IEnumerator OpenURL(){

		string url = "https://financial-app-cff5d.firebaseio.com/Value.json";
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null) {
			print (www.data);
			DebugLog (www.data);
		} else {
			DebugLog ("ERROR: " + www.error);
		}
	}

	void OpenMainPage(){

		HomePage.Hide (false);
		HomePage.gameObject.SetActive (false);
		SignUpPage.Hide (false);
		SignInPage.Hide (false);
		MainPage.gameObject.SetActive (true);
		MainPage.Show (false);

		//if (user.Email!="")
		//	UserEmail = user.Email;
		//else 
			if(user.UserId!="")
			UserEmail = user.UserId;

		//WriteEditorData ();

		//DebugLog (UserEmail+"ID");
		//DisplayInfo ();
		GetComponent <DataBaseManager>().ReadDataBase ();
		//_DisplayInfo.OnEnable ();
	}

	void WriteEditorData(){
		UserData temp = new UserData();

		temp.DisplayName = "Editor";
		temp.Email = "Editor@gmail.com";
		temp.UserID = UserEmail;
		temp.Balance = 10000;


		UpdateUserDb (temp);
	}



	void DisplayInfo(){
		_DisplayInfo.UserName.text = user.DisplayName;
		_DisplayInfo.Email.text = user.Email;
		//	if (!user.IsEmailVerified)
		//		DebugLog ("Please verify your Email ID");
		//	if (user.PhotoUrl.ToString ()!="")
		//		StartCoroutine (LoadImage (user.PhotoUrl.ToString ()));
	}

	IEnumerator LoadImage(string photo_url){

		WWW www = new WWW(photo_url);
		yield return www;
		_DisplayInfo.UserPic.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
	} 

}

[System.Serializable]
public class UserData
{
	public string DisplayName;
	public string Email;
	public string UserID;
	public string ImgURL="URL";
	public string TeamName;
	public float Balance = 0.0f;
	public bool isEmailVerified = false;
	public bool isMobileVerified = false;
	public bool isProofVerified = false;
}