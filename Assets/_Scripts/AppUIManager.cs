using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DoozyUI;

public class AppUIManager : MonoBehaviour {
	public Sprite MainPageHighLightActive,MainPageHighLightInactive;
	public static int MainPageState,RoleState;
	public Image[] MainPageButtons;
	public GameObject[] MainPageItems;

	public Image[] RoleButtons;
	public GameObject[] RoleViews;

	public UIElement HomePage,PlayerSelection, PlayerReview;
	public Text PlayerCount, CreditsRemaining;

	public UIElement LoadingPage;
	// Use this for initialization
	public static AppUIManager instance = null;
	void Awake () 
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		Application.targetFrameRate = 60;
		//Setup();


	}
	void Start () {
		SetMainPageState (1);
		SetRoleState (1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Loading(bool state){
		if (state)
			LoadingPage.Show (false);
		else
			LoadingPage.Hide (false);

	}

	Sprite test;
	public void DebugLog(string s) {
		UIManager.ShowNotification("Example_1_Notification_4", 0.5f, true, s, test);
	}
	public void SetMainPageState(int state){
		MainPageState = state;
		CheckMainPageState ();
	}

	public void CheckMainPageState(){
		switch (MainPageState) {
		case 1:
			MainPageInactive ();
			MainPageButtons [0].sprite = MainPageHighLightActive;
			MainPageItems [0].SetActive (true);
			break;
		case 2:
			MainPageInactive ();
			MainPageButtons [1].sprite = MainPageHighLightActive;
			MainPageItems [1].SetActive (true);
			break;
		case 3:
			MainPageInactive ();
			MainPageButtons [2].sprite = MainPageHighLightActive;
			MainPageItems [2].SetActive (true);
			break;
		case 4:
			MainPageInactive ();
			MainPageButtons [3].sprite = MainPageHighLightActive;
			MainPageItems [3].SetActive (true);
			break;
		}
	}

	public void MainPageInactive(){
		
		MainPageButtons [0].sprite = MainPageHighLightInactive;
		MainPageButtons [1].sprite = MainPageHighLightInactive;
		MainPageButtons [2].sprite = MainPageHighLightInactive;
		MainPageButtons [3].sprite = MainPageHighLightInactive;

		MainPageItems [0].SetActive (false);
		MainPageItems [1].SetActive (false);
		MainPageItems [2].SetActive (false);
		MainPageItems [3].SetActive (false);

	}


	public void SetRoleState(int state){
		RoleState = state;
		CheckRoleState ();
	}

	public void CheckRoleState(){
		switch (RoleState) {
		case 1:
			RoleInactive ();
			RoleButtons [0].sprite = MainPageHighLightActive;
			RoleButtons [0].color = new Color (1,1,1,1);
			RoleViews [0].SetActive (true);
			break;
		case 2:
			RoleInactive ();
			MainPageButtons [1].sprite = MainPageHighLightActive;
			RoleButtons [1].color = new Color (1,1,1,1);
			RoleViews [1].SetActive (true);
			break;
		case 3:
			RoleInactive ();
			RoleButtons [2].sprite = MainPageHighLightActive;
			RoleButtons [2].color = new Color (1,1,1,1);
			RoleViews [2].SetActive (true);
			break;
		case 4:
			RoleInactive ();
			RoleButtons [3].sprite = MainPageHighLightActive;
			RoleButtons [3].color = new Color (1,1,1,1);
			RoleViews [3].SetActive (true);
			break;
		}
	}

	public void RoleInactive(){

		foreach (Image img in RoleButtons) {
			img.sprite = MainPageHighLightInactive;
			img.color = new Color (0, 0, 0, 0);
		}

		foreach (GameObject go in RoleViews)
			go.SetActive (false);

	}

	public void UpdateCredits(string s){
		CreditsRemaining.text = "Credits remaining : "+s;
	}

	public void UpdatePlayerCount(string s){
		PlayerCount.text = "Players : "+s;
	}

	public void OpenHomePage(){
		PlayerReview.Hide (false);
		HomePage.Show (false);
	}

	public void OpenReview(){
		PlayerSelection.Hide (false);
		PlayerReview.Show (false);
	}

}
