using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Newtonsoft.Json;

public class TeamManager : MonoBehaviour {
	public MatchData SelectedMatch;
	public GameObject PlayerPrefab;
	public static TeamManager instance = null;
	public GameObject BA_Content, BL_Content, AR_Content, W_Content;
	public List<PlayerData> MyList = new List<PlayerData>();
	public int BA_Count, BL_Count, AR_Count, WK_Count, TeamCount;
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
		CreditsRemaining -= _PlayerData.Credit;
		TeamCount++;
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
		CreditsRemaining += _PlayerData.Credit;
		TeamCount--;
		MyList.Remove (_PlayerData);
		AppUIManager.instance.UpdateCredits (CreditsRemaining.ToString ());
		AppUIManager.instance.UpdatePlayerCount (TeamCount.ToString ());
	}

	public void ReviewPlayers(){
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
		CreateReview ();
		AppUIManager.instance.OpenReview ();
	}

	public void CreateReview(){
		for(int i=0;i<11;i++) {
			if (SelectedMatch.MyTeam.Count > 0)
				_PlayerReviewItems [i]._PlayerData = SelectedMatch.MyTeam[i];
			else
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
		print ("final review");
		MyList.Clear ();
		for (int i = 0; i < 11; i++)
			MyList.Add (_PlayerReviewItems [i]._PlayerData);
		//print (JsonConvert.SerializeObject (MyList));
		foreach(PlayerData PD in MyList){
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		reference.Child ("Cricket").Child ("Tournament")
			.Child (SelectedMatch.TournamentName)
			.Child (SelectedMatch.MatchName)
			.Child ("Users")
			.Child (AuthenticationManager.UserEmail)
			.Child (PD.PlayerID)
			.SetRawJsonValueAsync (JsonConvert.SerializeObject (PD));
		}
		AppUIManager.instance.DebugLog ("Updated Team");
		AppUIManager.instance.OpenHomePage ();
	}

	public void UpdateTeamFirebase(){

	}

	public void DisplayListings(MatchData SelectedMatch){
//		if (SelectedMatch.MyTeam.Count > 0) {
//			MyList = SelectedMatch.MyTeam;
//			CreateReview ();
//			AppUIManager.instance.OpenReview ();
//			return;
//		}
		AppUIManager.instance.PlayerSelection.Show(false);
		foreach (PlayerData PD in SelectedMatch.Team1Players) {
			if (PD.Position == "BA") {
				GameObject GO = Instantiate (PlayerPrefab);
				GO.transform.SetParent (BA_Content.transform);
				GO.GetComponent <PlayerItem> ()._PlayerData = PD;

				GO.GetComponent <PlayerItem> ().AssignValues ();
				GO.transform.localScale = Vector3.one;
				BA_Content.GetComponent <RectTransform> ().sizeDelta =  new Vector2 (BA_Content.GetComponent <RectTransform> ().rect.width + 220, 180);
				if (SelectedMatch.MyTeam.Exists (x => x.PlayerID == PD.PlayerID))
					GO.GetComponent <PlayerItem> ().OnClick ();

			}
			else if (PD.Position == "BL") {
				GameObject GO = Instantiate (PlayerPrefab);
				GO.transform.SetParent (BL_Content.transform);
				GO.GetComponent <PlayerItem> ()._PlayerData = PD;

				GO.GetComponent <PlayerItem> ().AssignValues ();
				GO.transform.localScale = Vector3.one;
				BL_Content.GetComponent <RectTransform> ().sizeDelta = new Vector2 (BL_Content.GetComponent <RectTransform> ().rect.width + 220, 180);
				if (SelectedMatch.MyTeam.Exists(x=>x.PlayerID==PD.PlayerID))
					GO.GetComponent <PlayerItem> ().OnClick ();
			}
			else if (PD.Position == "AR") {
				GameObject GO = Instantiate (PlayerPrefab);
				GO.transform.SetParent (AR_Content.transform);
				GO.GetComponent <PlayerItem> ()._PlayerData = PD;

				GO.GetComponent <PlayerItem> ().AssignValues ();
				GO.transform.localScale = Vector3.one;
				AR_Content.GetComponent <RectTransform> ().sizeDelta = new Vector2 (AR_Content.GetComponent <RectTransform> ().rect.width + 220, 180);
				if (SelectedMatch.MyTeam.Exists(x=>x.PlayerID==PD.PlayerID))
					GO.GetComponent <PlayerItem> ().OnClick ();
			}
			else if (PD.Position == "W") {
				GameObject GO = Instantiate (PlayerPrefab);
				GO.transform.SetParent (W_Content.transform);
				GO.GetComponent <PlayerItem> ()._PlayerData = PD;

				GO.GetComponent <PlayerItem> ().AssignValues ();
				GO.transform.localScale = Vector3.one;
				W_Content.GetComponent <RectTransform> ().sizeDelta =  new Vector2 (W_Content.GetComponent <RectTransform> ().rect.width + 220, 180);
				if (SelectedMatch.MyTeam.Exists(x=>x.PlayerID==PD.PlayerID))
					GO.GetComponent <PlayerItem> ().OnClick ();
			}
				
		}

		foreach (PlayerData PD in SelectedMatch.Team2Players) {
			if (PD.Position == "BA") {
				GameObject GO = Instantiate (PlayerPrefab);
				GO.transform.SetParent (BA_Content.transform);
				GO.GetComponent <PlayerItem> ()._PlayerData = PD;

				GO.GetComponent <PlayerItem> ().AssignValues ();
				GO.transform.localScale = Vector3.one;
				BA_Content.GetComponent <RectTransform> ().sizeDelta =  new Vector2 (BA_Content.GetComponent <RectTransform> ().rect.width + 220, 180);
				if (SelectedMatch.MyTeam.Exists(x=>x.PlayerID==PD.PlayerID))
					GO.GetComponent <PlayerItem> ().OnClick ();
			}
			else if (PD.Position == "BL") {
				GameObject GO = Instantiate (PlayerPrefab);
				GO.transform.SetParent (BL_Content.transform);
				GO.GetComponent <PlayerItem> ()._PlayerData = PD;

				GO.GetComponent <PlayerItem> ().AssignValues ();
				GO.transform.localScale = Vector3.one;
				BL_Content.GetComponent <RectTransform> ().sizeDelta = new Vector2 (BL_Content.GetComponent <RectTransform> ().rect.width + 220, 180);
				if (SelectedMatch.MyTeam.Exists(x=>x.PlayerID==PD.PlayerID))
					GO.GetComponent <PlayerItem> ().OnClick ();
			}
			else if (PD.Position == "AR") {
				GameObject GO = Instantiate (PlayerPrefab);
				GO.transform.SetParent (AR_Content.transform);
				GO.GetComponent <PlayerItem> ()._PlayerData = PD;

				GO.GetComponent <PlayerItem> ().AssignValues ();
				GO.transform.localScale = Vector3.one;
				AR_Content.GetComponent <RectTransform> ().sizeDelta = new Vector2 (AR_Content.GetComponent <RectTransform> ().rect.width + 220, 180);
				if (SelectedMatch.MyTeam.Exists(x=>x.PlayerID==PD.PlayerID))
					GO.GetComponent <PlayerItem> ().OnClick ();
			}
			else if (PD.Position == "W") {
				GameObject GO = Instantiate (PlayerPrefab);
				GO.transform.SetParent (W_Content.transform);
				GO.GetComponent <PlayerItem> ()._PlayerData = PD;

				GO.GetComponent <PlayerItem> ().AssignValues ();
				GO.transform.localScale = Vector3.one;
				W_Content.GetComponent <RectTransform> ().sizeDelta =  new Vector2 (W_Content.GetComponent <RectTransform> ().rect.width + 220, 180);
				if (SelectedMatch.MyTeam.Exists(x=>x.PlayerID==PD.PlayerID))
					GO.GetComponent <PlayerItem> ().OnClick ();
			}

		}
	}
}
