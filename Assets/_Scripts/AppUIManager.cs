using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DoozyUI;

public class AppUIManager : MonoBehaviour {
	public Sprite MainPageHighLightActive,MainPageHighLightInactive;
	public GameObject Cam3D,UIBG;
	public Text Name,TeamName,Wallet,ConfirmationText;
	public static int MainPageState,RoleState,LeaguePageState;
	public Image[] MainPageButtons,LeaguePageButtons;
	public GameObject[] MainPageItems, FootBallPageItems, KabaddiPageItems, LeaguePageItems;

	public Image[] RoleButtons;
	public GameObject[] RoleViews;

	public UIElement HomePage,PlayerSelection, PlayerPreview, PlayerReview,LeaguesPage,ConfirmationWindow,WalletWindow;
	public static bool isConfirmed;
	public Text PlayerCount, CreditsRemaining, ContestJoinedTxt;
	public Text[] PlayerPositionTxt;
	public UIElement LoadingPage;

	public LeagueItem SelectedLeague;

	public Dropdown GameType;
	public Text teamSelection_TeamName,myProfile_Name, verify_Email, myAccount_Bal, personalDetails_Name, personalDetails_Email,personalDetails_TeamName;

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
		SetMainPageState (2);
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

	public void UpdateUserData(){
		Name.text = myProfile_Name.text = personalDetails_Name.text = DataBaseManager.instance.Udata.DisplayName;
		TeamName.text = teamSelection_TeamName.text = personalDetails_TeamName.text = DataBaseManager.instance.Udata.TeamName;
		Wallet.text = myAccount_Bal.text = "Rs "+DataBaseManager.instance.Udata.Balance;
		personalDetails_Email.text = DataBaseManager.instance.Udata.Email;
	}
	public GameObject[] Games;
	public void ShowGame(){

		HideGames ();
		Games[GameType.value].SetActive (true);


	}

	public void HideGames(){
		foreach (GameObject game in Games)
			game.SetActive (false);
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
			RoleButtons [1].sprite = MainPageHighLightActive;
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

	public void SetLeaguePageState(int state){
		LeaguePageState = state;
		CheckLeaguePageState ();
	}

	public void CheckLeaguePageState(){
		switch (LeaguePageState) {
		case 0:
			LeaguePageInactive ();
			LeaguePageButtons [0].sprite = MainPageHighLightActive;
			LeaguePageItems [0].SetActive (true);
			break;
		case 1:
			LeaguePageInactive ();
			LeaguePageButtons [1].sprite = MainPageHighLightActive;
			LeaguePageItems [1].SetActive (true);
			break;
		case 2:
			LeaguePageInactive ();
			TeamManager.instance.JoinedLeague ();
			LeaguePageButtons [2].sprite = MainPageHighLightActive;
			LeaguePageItems [2].SetActive (true);
			break;
		}
	}

	public void LeaguePageInactive(){

		LeaguePageButtons [0].sprite = MainPageHighLightInactive;
		LeaguePageButtons [1].sprite = MainPageHighLightInactive;
		LeaguePageButtons [2].sprite = MainPageHighLightInactive;

		LeaguePageItems [0].SetActive (false);
		LeaguePageItems [1].SetActive (false);
		LeaguePageItems [2].SetActive (false);

	}

	public void UpdatePlayerCounts(int BA,int BL, int AR,int WK,int Total){
		PlayerPositionTxt [0].text = "(" + BA + ")";
		PlayerPositionTxt [1].text = "(" + BL + ")";
		PlayerPositionTxt [2].text = "(" + AR + ")";
		PlayerPositionTxt [3].text = "(" + WK + ")";

		if(Total==11&&WK>0){
			PlayerPositionTxt [0].color = Color.green;
			PlayerPositionTxt [1].color = Color.green;
			PlayerPositionTxt [2].color = Color.green;
			PlayerPositionTxt [3].color = Color.green;
			return;
		}

		if (BA == 0)
			PlayerPositionTxt [0].color = Color.red;
		else
			PlayerPositionTxt [0].color = Color.yellow;
		if (BL == 0)
			PlayerPositionTxt [1].color = Color.red;
		else
			PlayerPositionTxt [1].color = Color.yellow;
		if (AR == 0)
			PlayerPositionTxt [2].color = Color.red;
		else
			PlayerPositionTxt [2].color = Color.yellow;
		if (WK == 0)
			PlayerPositionTxt [3].color = Color.red;
		else
			PlayerPositionTxt [3].color = Color.yellow;
				
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
	//	Cam3D.SetActive (false);
		HidePreview();
		PlayerSelection.Hide (false);
		PlayerReview.Show (false);
	}

	public void ShowSelectedLeague(){
		SelectedLeague.AssignValues ();
		SelectedLeague.ShowTeams ();
	}

	public void ShowPreview(){
		UIBG.SetActive (false);
		Cam3D.SetActive (true);
		PlayerPreview.Show (false);
		PlayerSelection.Hide (false);

		PreviewManager.instance._TeamData.PlayerList.Clear ();
		foreach (PlayerData PD in TeamManager.instance.MyList) {
			PreviewManager.instance._TeamData.PlayerList.Add (PD);
		}
		PreviewManager.instance.AssignPlayers ();
	}

	public void HidePreview(){
		UIBG.SetActive (true);
		Cam3D.SetActive (false);
		PlayerPreview.Hide (false);
		PlayerSelection.Show (false);
	}


	public void CheckWallet(){
		Application.OpenURL ("https://test.payu.in/_payment?key=udSc4tKs&txnid=001&amount=100&productinfo=testpurchase&firstname=akil&email=akil@test.com&phone=9789344663&surl=https://incrediblefl-affdd.firebaseapp.com/SuccessURL.html&furl=https://incrediblefl-affdd.firebaseapp.com/SuccessURL.html&hash=b31a0a26035431d447c1b7f1ed15f32d3534615eb83cbcc05a3ee04a04ec791a");
	}


	public void OpenConfirmation(int cost){
		isConfirmed = false;
		ConfirmationText.text = "You will be charged Rs " + cost + ". Do you want to proceed ?";
		ConfirmationWindow.Show (true);

	}

	public void ConfirmPayment(){
		isConfirmed = true;
	}

	public void DeclinePayment(){

	}

	public void InsufficientBalance(){
		isConfirmed = false;
		ConfirmationText.text = "Insufficient Balance. Do you want to proceed ?";
		ConfirmationWindow.Show (true);

	}

	public void OpenWallet(){
		WalletWindow.Show (false);

	}



}
