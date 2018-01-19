using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Newtonsoft.Json;

public class TeamManager : MonoBehaviour {
	public MatchData SelectedMatch;
	public int SelectedTournamentIndex,SelectedMatchIndex;
	public GameObject PlayerPrefab,LeaguePrefab;
	public static TeamManager instance = null;
	public GameObject[] Games;
	public GameObject BA_Content, BL_Content, AR_Content, W_Content,FreeLeague_Content,PaidLeague_Content,JoinedLeague_Content;
	public GameObject Fwd_Content, Mid_Content, Def_Content, GK_Content, R_Content, A_Content, D_Content;
	public List<PlayerData> MyList = new List<PlayerData>();
	public int BA_Count, BL_Count, AR_Count, WK_Count, TeamCount;
	public int Fwd_Count, Mid_Count,Def_Count, GK_Count, R_Count, A_Count, D_Count;
	public bool isCaptainSelected, isVCSelected;
	public float CreditsRemaining;
	public List<PlayerReviewItem> _PlayerReviewItems = new List<PlayerReviewItem>();
	void Awake () 
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		//Setup();

	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddPlayer (PlayerData _PlayerData){
		if (_PlayerData.Position == "BA"){
			BA_Count++;
		}
		if (_PlayerData.Position == "BL"){
			BL_Count++;
		}
		if (_PlayerData.Position == "AR"){
			AR_Count++;
		}
		if (_PlayerData.Position == "W") {
			WK_Count++;
		}

		if (_PlayerData.Position == "Forward"){
			Fwd_Count++;
		}
		if (_PlayerData.Position == "Midfielder"){
			Mid_Count++;
		}
		if (_PlayerData.Position == "Defender"){
			Def_Count++;
		}
		if (_PlayerData.Position == "GoalKeeper") {
			GK_Count++;
		}

		if (_PlayerData.Position == "Raider"){
			R_Count++;
		}
		if (_PlayerData.Position == "Allrounder"){
			A_Count++;
		}
		if (_PlayerData.Position == "Def"){
			D_Count++;
		}

		CreditsRemaining -= _PlayerData.Credit;
		TeamCount++;
		if (AppUIManager.GameID == 0)
			AppUIManager.instance.UpdatePlayerCounts (BA_Count, BL_Count, AR_Count, WK_Count, TeamCount);
		else if (AppUIManager.GameID == 1)
			AppUIManager.instance.UpdateFootBallCounts (Fwd_Count, Mid_Count, Def_Count, GK_Count, TeamCount);
		else
			AppUIManager.instance.UpdateKabaddiCounts (R_Count, A_Count, D_Count, TeamCount);
		MyList.Add (_PlayerData);
		AppUIManager.instance.UpdateCredits (CreditsRemaining.ToString ());
		AppUIManager.instance.UpdatePlayerCount (TeamCount.ToString ());
	}

	public void RemovePlayer (PlayerData _PlayerData){
		if (_PlayerData.Position == "BA"){
			BA_Count--;
		}
		if (_PlayerData.Position == "BL"){
			BL_Count--;
		}
		if (_PlayerData.Position == "AR"){
			AR_Count--;
		}
		if (_PlayerData.Position == "W") {
			WK_Count--;
		}

		if (_PlayerData.Position == "Forward"){
			Fwd_Count--;
		}
		if (_PlayerData.Position == "Midfielder"){
			Mid_Count--;
		}
		if (_PlayerData.Position == "Defender"){
			Def_Count--;
		}
		if (_PlayerData.Position == "GoalKeeper") {
			GK_Count--;
		}

		if (_PlayerData.Position == "Raider"){
			R_Count--;
		}
		if (_PlayerData.Position == "Allrounder"){
			A_Count--;
		}
		if (_PlayerData.Position == "Def"){
			D_Count--;
		}

		CreditsRemaining += _PlayerData.Credit;
		TeamCount--;
		if (AppUIManager.GameID == 0)
			AppUIManager.instance.UpdatePlayerCounts (BA_Count, BL_Count, AR_Count, WK_Count, TeamCount);
		else if (AppUIManager.GameID == 1)
			AppUIManager.instance.UpdateFootBallCounts (Fwd_Count, Mid_Count, Def_Count, GK_Count, TeamCount);
		else
			AppUIManager.instance.UpdateKabaddiCounts (R_Count, A_Count, D_Count, TeamCount);
		MyList.Remove (_PlayerData);
		AppUIManager.instance.UpdateCredits (CreditsRemaining.ToString ());
		AppUIManager.instance.UpdatePlayerCount (TeamCount.ToString ());
	}

	public void ReviewPlayers(){
		if (AppUIManager.GameID == 0) {
			if (BA_Count < 3) {
				AppUIManager.instance.DebugLog ("Pick Atleast 3 Batsmen");
				AppUIManager.instance.SetRoleState (1);
				return;
			} else if (BL_Count < 3) {
				AppUIManager.instance.DebugLog ("Pick Atleast 3 Bowlers");
				AppUIManager.instance.SetRoleState (2);
				return;
			} else if (AR_Count < 1) {
				AppUIManager.instance.DebugLog ("Pick Atleast 1 All Rounder");
				AppUIManager.instance.SetRoleState (3);
				return;
			} else if (WK_Count < 1) {
				AppUIManager.instance.DebugLog ("Pick 1 WicketKeeper");
				AppUIManager.instance.SetRoleState (4);
				return;
			} 
			if (MyList.Count < 11) {
				AppUIManager.instance.DebugLog ("Pick 11 Players");
				return;
			}
		}
		if (AppUIManager.GameID == 1) {
			if (Fwd_Count < 3) {
				AppUIManager.instance.DebugLog ("Pick Atleast 3 Forward");
				AppUIManager.instance.SetRoleState (1);
				return;
			} else if (Mid_Count < 3) {
				AppUIManager.instance.DebugLog ("Pick Atleast 3 Mid Fielders");
				AppUIManager.instance.SetRoleState (2);
				return;
			} else if (Def_Count < 3) {
				AppUIManager.instance.DebugLog ("Pick Atleast 3 Defenders");
				AppUIManager.instance.SetRoleState (3);
				return;
			} else if (GK_Count < 1) {
				AppUIManager.instance.DebugLog ("Pick 1 Goal Keeper");
				AppUIManager.instance.SetRoleState (4);
				return;
			}
			if (MyList.Count < 11) {
				AppUIManager.instance.DebugLog ("Pick 11 Players");
				return;
			}
		}
		if (AppUIManager.GameID == 2) {
			if (R_Count < 1) {
				AppUIManager.instance.DebugLog ("Pick Atleast 1 Raider");
				AppUIManager.instance.SetRoleState (1);
				return;
			} else if (A_Count < 1) {
				AppUIManager.instance.DebugLog ("Pick Atleast 1 All-Rounder");
				AppUIManager.instance.SetRoleState (2);
				return;
			} else if (D_Count < 2) {
				AppUIManager.instance.DebugLog ("Pick Atleast 2 Defenders");
				AppUIManager.instance.SetRoleState (3);
				return;
			}
			if (MyList.Count < 7) {
				AppUIManager.instance.DebugLog ("Pick 7 Players");
				return;
			}
		}



		CreateReview ();
		AppUIManager.instance.OpenReview ();
	}

	public void CreateReview(){
		for(int i=0;i<11;i++) 
			_PlayerReviewItems [i].gameObject.SetActive (false);

		if (AppUIManager.GameID == 2) {
			for(int i=0;i<7;i++) {
				_PlayerReviewItems [i].gameObject.SetActive (true);
				_PlayerReviewItems [i]._PlayerData = MyList [i];
				_PlayerReviewItems [i].Assign ();
			}
			return;
		}

		for(int i=0;i<11;i++) {
			_PlayerReviewItems [i].gameObject.SetActive (true);
			_PlayerReviewItems [i]._PlayerData = MyList [i];
			_PlayerReviewItems [i].Assign ();
		}
	}
	public void ClearAllCap(){
		for (int i = 0; i < 11; i++) {
			//_PlayerReviewItems [i]._PlayerData.isCaptain = false;
			_PlayerReviewItems [i].ClearCap ();
		}
	}
	public void ClearAllVC(){
		for (int i = 0; i < 11; i++) {
			_PlayerReviewItems [i].ClearVC ();
			//_PlayerReviewItems [i]._PlayerData.isViceCaptain = false;
		}
	}

	public void FinalReview(){
		if (!isCaptainSelected || !isVCSelected) {
			AppUIManager.instance.DebugLog ("Pick Captain and Vice Captain");
			return;
		}

		SelectedMatch.MyTeam.Clear ();
		MyList.Clear ();
		string gameType;
		if (AppUIManager.GameID == 0) {
			gameType = "Cricket";
			for (int i = 0; i < 11; i++)
				MyList.Add (_PlayerReviewItems [i]._PlayerData);
		} else if (AppUIManager.GameID == 1) {
			gameType = "Football";
			for (int i = 0; i < 11; i++)
				MyList.Add (_PlayerReviewItems [i]._PlayerData);
		} else {
			gameType = "Kabaddi";
			for (int i = 0; i < 7; i++)
				MyList.Add (_PlayerReviewItems [i]._PlayerData);
		}


		
		foreach(PlayerData PD in MyList){
		SelectedMatch.MyTeam.Add (PD);
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
			reference.Child (gameType).Child ("Tournament")
			.Child (SelectedMatch.TournamentName)
			.Child (SelectedMatch.MatchName)
			.Child ("Users")
			.Child (AuthenticationManager.TeamName)
			.Child (PD.PlayerID)
			.SetRawJsonValueAsync (JsonConvert.SerializeObject (PD));
		}
		if(AppUIManager.GameID==0)
			DataBaseManager.instance.TournamentList [SelectedTournamentIndex].Tournaments [SelectedMatchIndex] = SelectedMatch;
		else if(AppUIManager.GameID==1)
			DataBaseManager.instance.Football_List [SelectedTournamentIndex].Tournaments [SelectedMatchIndex] = SelectedMatch;
		else
			DataBaseManager.instance.Kabaddi_List [SelectedTournamentIndex].Tournaments [SelectedMatchIndex] = SelectedMatch;
		
		AppUIManager.instance.DebugLog ("Updated Team");
		AppUIManager.instance.PlayerReview.Hide (true);
		AppUIManager.instance.LeaguesPage.Show(true);
	}

	public void Show3D(){
		if (BA_Count < 3) {
			AppUIManager.instance.DebugLog ("Pick Atleast 3 Batsmen");
			AppUIManager.instance.SetRoleState (1);
			return;
		} else if (BL_Count < 3) {
			AppUIManager.instance.DebugLog ("Pick Atleast 3 Bowlers");
			AppUIManager.instance.SetRoleState (2);
			return;
		} else if (AR_Count < 1) {
			AppUIManager.instance.DebugLog ("Pick Atleast 1 All Rounder");
			AppUIManager.instance.SetRoleState (3);
			return;
		} else if (WK_Count < 1) {
			AppUIManager.instance.DebugLog ("Pick 1 WicketKeeper");
			AppUIManager.instance.SetRoleState (4);
			return;
		} else if (MyList.Count < 11) {
			AppUIManager.instance.DebugLog ("Pick 11 Players");
			return;
		}

		AppUIManager.instance.ShowPreview ();
	}

	public void BackPreview(){

	}

	public void DisplayListings(){
//		if (SelectedMatch.MyTeam.Count > 0) {
//			MyList = SelectedMatch.MyTeam;
//			CreateReview ();
//			AppUIManager.instance.OpenReview ();
//			return;
//		}

		ResetListings ();



		List<PlayerData> EntirePlayerList = new List<PlayerData>();
		if (SelectedMatch.MyTeam.Count < 1) {
			
			AppUIManager.instance.PlayerSelection.Show(false);
			foreach (PlayerData PD in SelectedMatch.Team1Players)
				EntirePlayerList.Add (PD);
			foreach (PlayerData PD in SelectedMatch.Team2Players)
				EntirePlayerList.Add (PD);
			EntirePlayerList.Sort ((x, y) => -1 * x.Credit.CompareTo (y.Credit));
		} else {
			//CreateReview ();

			//add later new UI
//			AppUIManager.instance.LeaguesPage.Show(false);
//			AppUIManager.instance.ContestJoinedTxt.text = "Contests Joined (" +SelectedMatch.MyLeagues.Count + ")";
//			foreach (PlayerData PD in SelectedMatch.MyTeam)
//				EntirePlayerList.Add (PD);
//			foreach (PlayerData PD in SelectedMatch.Team1Players)
//				if(!EntirePlayerList.Exists (x => x.PlayerID == PD.PlayerID))
//					EntirePlayerList.Add (PD);
//			foreach (PlayerData PD in SelectedMatch.Team2Players)
//				if(!EntirePlayerList.Exists (x => x.PlayerID == PD.PlayerID))
//					EntirePlayerList.Add (PD);

		}
		foreach (PlayerData PD in EntirePlayerList) {

			GameObject GO = Instantiate (PlayerPrefab);

			switch (PD.Position) {
			case "BA":
				GO.transform.SetParent (BA_Content.transform);
				break;
			case "BL":
				GO.transform.SetParent (BL_Content.transform);
				break;
			case "AR":
				GO.transform.SetParent (AR_Content.transform);
				break;
			case "W":
				GO.transform.SetParent (W_Content.transform);
				break;

			case "Forward":
				GO.transform.SetParent (Fwd_Content.transform);
				break;
			case "Midfielder":
				GO.transform.SetParent (Mid_Content.transform);
				break;
			case "Defender":
				GO.transform.SetParent (Def_Content.transform);
				break;
			case "GoalKeeper":
				GO.transform.SetParent (GK_Content.transform);
				break;
			
			case "Raider":
				GO.transform.SetParent (R_Content.transform);
				break;
			case "Allrounder":
				GO.transform.SetParent (A_Content.transform);
				break;
			case "Def":
				GO.transform.SetParent (D_Content.transform);
				break;
			
			}

			GO.GetComponent <PlayerItem> ()._PlayerData = PD;
			GO.GetComponent <PlayerItem> ().AssignValues ();
			GO.transform.localScale = Vector3.one;
			if (SelectedMatch.MyTeam.Exists (x => x.PlayerID == PD.PlayerID))
				GO.GetComponent <PlayerItem> ().ForceAdd ();
			

		}

	}

	public void CreateLeagueListing(){
		foreach (Transform Child in FreeLeague_Content.transform)
			Destroy (Child.gameObject);
		foreach (Transform Child in PaidLeague_Content.transform)
			Destroy (Child.gameObject);

		int i = 1;
		foreach (LeagueData LD in SelectedMatch.FreeLeagues) {
			GameObject GO = Instantiate (LeaguePrefab);
			GO.transform.SetParent (FreeLeague_Content.transform);
			GO.GetComponent <LeagueItem> ()._LeagueData = LD;
			GO.GetComponent <LeagueItem> ().AssignValues ();
			GO.GetComponent <LeagueItem> ().LeagueNo = i;
			GO.transform.localScale = Vector3.one;
			i++;
		}
		i = 1;
		foreach (LeagueData LD in SelectedMatch.PaidLeagues) {
			GameObject GO = Instantiate (LeaguePrefab);
			GO.transform.SetParent (PaidLeague_Content.transform);
			GO.GetComponent <LeagueItem> ()._LeagueData = LD;
			GO.GetComponent <LeagueItem> ().AssignValues ();
			GO.GetComponent <LeagueItem> ().LeagueNo = i;
			GO.transform.localScale = Vector3.one;
			i++;
		}
	}

	public void JoinedLeague(){
		foreach (Transform Child in JoinedLeague_Content.transform)
			Destroy (Child.gameObject);

		foreach (LeagueData LD in SelectedMatch.MyLeagues) {
			GameObject GO = Instantiate (LeaguePrefab);
			GO.transform.SetParent (JoinedLeague_Content.transform);
			GO.GetComponent <LeagueItem> ()._LeagueData = LD;
			GO.GetComponent <LeagueItem> ().AssignValues ();
			GO.transform.localScale = Vector3.one;
		}
	}

	public void ResetListings(){
		CreditsRemaining = 100;
		AR_Count = 0;
		BA_Count = 0;
		BL_Count = 0;
		WK_Count = 0;
		TeamCount = 0;
		Fwd_Count = 0;
		Mid_Count = 0;
		Def_Count = 0;
		GK_Count = 0;
		R_Count = 0;
		A_Count = 0;
		D_Count = 0;

		MyList.Clear ();
		ClearListing(BA_Content.transform);
		ClearListing(BL_Content.transform);
		ClearListing(AR_Content.transform);
		ClearListing(W_Content.transform);

		ClearListing(Fwd_Content.transform);
		ClearListing(Mid_Content.transform);
		ClearListing(Def_Content.transform);
		ClearListing(GK_Content.transform);

		ClearListing(A_Content.transform);
		ClearListing(R_Content.transform);
		ClearListing(D_Content.transform);

		AppUIManager.instance.UpdatePlayerCounts (0, 0, 0, 0, 0);
		AppUIManager.instance.UpdateFootBallCounts (0, 0, 0, 0, 0);
		AppUIManager.instance.UpdateKabaddiCounts (0, 0, 0, 0);
	}

	public void ClearListing(Transform Content){
		foreach (Transform Child in Content)
			Destroy (Child.gameObject);
		Content.GetComponent <RectTransform> ().sizeDelta =  new Vector2 (0, 180);
	}
}
