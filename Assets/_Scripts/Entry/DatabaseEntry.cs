using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using DoozyUI;

using Newtonsoft.Json;
using System.Linq;
using System;
using UnityEngine.UI;
public class DatabaseEntry : MonoBehaviour {
	public List<TournamentData> CricketList,Football_List,Kabaddi_List;
	public GameObject MatchPrefab,CricketContent,FootBallContent,KabaddiContent;

	Firebase.Auth.FirebaseAuth auth;
	// Use this for initialization
//	void Start () {
//		auth.SignInAnonymouslyAsync().ContinueWith(HandleSigninResult);
//		ReadTournamentData ();
//	}
//
//	void HandleSigninResult(Task<Firebase.Auth.FirebaseUser> authTask) {
//		LogTaskCompletion(authTask, "Sign-in");
//	}
//
//	bool LogTaskCompletion(Task task, string operation) {
//		bool complete = false;
//	//	isLoading = false;
//		if (task.IsCanceled) {
//			DebugLog(operation + " canceled.");
//		} else if (task.IsFaulted) {
//			DebugLog(operation + " encounted an error.");
//			DebugLog(task.Exception.ToString());
//		} else if (task.IsCompleted) {
//
//			complete = true;
//
//
//			//OpenMainPage ();
//		}
//		return complete;
//	}
	// Update is called once per frame
	public void GetDatabaseEntry(){
		ReadCricketData ();
		ReadFootBallData ();
		ReadKabaddiData ();

	}



	Sprite test;
	public void DebugLog(string s) {
		UIManager.ShowNotification("Example_1_Notification_4", 0.5f, true, s, test);
	}

	public void ReadCricketData(){
		//UIManager.ShowNotification("Example_1_Notification_4", 0.5f, true, "Loading Server Data", test);

		//AppUIManager.instance.Loading (true);
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		FirebaseDatabase.DefaultInstance
			.GetReference("Cricket/Tournament")
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					DebugLog(task.Exception.ToString ());
				}
				else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					string JsonValue = snapshot.GetRawJsonValue ();
					SaveTournamentData(JsonValue,CricketList,CricketContent);
				}
			});

	}

	public void ReadFootBallData(){
		//UIManager.ShowNotification("Example_1_Notification_4", 0.5f, true, "Loading Server Data", test);
	//	AppUIManager.instance.Loading (true);
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		FirebaseDatabase.DefaultInstance
			.GetReference("Football/Tournament")
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					DebugLog(task.Exception.ToString ());
				}
				else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					string JsonValue = snapshot.GetRawJsonValue ();
					SaveTournamentData(JsonValue,Football_List,FootBallContent);
				}
			});

	}

	public void ReadKabaddiData(){
		//UIManager.ShowNotification("Example_1_Notification_4", 0.5f, true, "Loading Server Data", test);
	//	AppUIManager.instance.Loading (true);
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		FirebaseDatabase.DefaultInstance
			.GetReference("Kabaddi/Tournament")
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					DebugLog(task.Exception.ToString ());
				}
				else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					string JsonValue = snapshot.GetRawJsonValue ();
					SaveTournamentData(JsonValue,Kabaddi_List,KabaddiContent);
				}
			});

	}


	public void SaveTournamentData(string JsonValue,List<TournamentData> TournamentList,GameObject MatchContent){
		TournamentList.Clear ();
		print (JsonValue);
		var Tournaments = JsonConvert.DeserializeObject<Dictionary<string, object>> (JsonValue);
		for (int i = 0; i < Tournaments.Count; i++) { // Tournament Loop
			TournamentData _TournamentData = new TournamentData();
			_TournamentData.TournamentName = Tournaments.ElementAt (i).Key.ToString ();
			_TournamentData.index = i;
			var Matches = JsonConvert.DeserializeObject<Dictionary<string, object>> (Tournaments.ElementAt(i).Value.ToString());
			for (int j = 0; j < Matches.Count; j++) { // Games in each Tournament Loop
				MatchData _MatchData = new MatchData ();
				_MatchData.MatchName = Matches.ElementAt (j).Key.ToString ();
				_MatchData.TournamentName = _TournamentData.TournamentName;
				_MatchData.index = j;
				var internalData = JsonConvert.DeserializeObject<Dictionary<string,object>> (Matches.ElementAt(j).Value.ToString()); 
				_MatchData.Team1 = internalData.Where (a=>a.Key.Contains("Team1")).First().Value.ToString();
				_MatchData.Team2 = internalData.Where (a=>a.Key.Contains("Team2")).First().Value.ToString();
				_MatchData.Date = DateTime.Parse (internalData.Where (a=>a.Key.Contains("Date")).First().Value.ToString());
				try{
					var TotalUserData = JsonConvert.DeserializeObject<Dictionary<string,object>> (internalData.Where (a => a.Key.Contains ("Users")).First ().Value.ToString ());
					var UserData = JsonConvert.DeserializeObject<Dictionary<string,object>> (TotalUserData.Where (a => a.Key.Contains (AuthenticationManager.TeamName)).First ().Value.ToString ());
					for (int k = 0; k < UserData.Count; k++) { // User team loop if exists
						PlayerData _PlayerData = new PlayerData ();
						_PlayerData.PlayerID = UserData.ElementAt (k).Key.ToString ();
						var internalPlayerData = JsonConvert.DeserializeObject<Dictionary<string,string>> (UserData.ElementAt(k).Value.ToString()); 
						_PlayerData.Name = internalPlayerData.Where (a=>a.Key.Contains("Name")).First().Value.ToString();
						_PlayerData.Credit = float.Parse (internalPlayerData.Where (a=>a.Key.Contains("Credit")).First().Value.ToString());
						_PlayerData.Position = internalPlayerData.Where (a=>a.Key.Contains("Position")).First().Value.ToString();
						if(internalPlayerData.Where (a=>a.Key.Contains("isCaptain")).First().Value.ToString()=="true")
							_PlayerData.isCaptain=true;
						if(internalPlayerData.Where (a=>a.Key.Contains("isViceCaptain")).First().Value.ToString()=="true")
							_PlayerData.isViceCaptain=true;
						_MatchData.MyTeam.Add (_PlayerData);
					}
				}
				catch{

				}
				var Team1Data = JsonConvert.DeserializeObject<Dictionary<string,object>> (internalData.Where (a => a.Key.Contains ("Team1Players")).First ().Value.ToString ());
				for (int k = 0; k < Team1Data.Count; k++) {
					PlayerData _PlayerData = new PlayerData ();
					_PlayerData.PlayerID = Team1Data.ElementAt (k).Key.ToString ();
					var internalPlayerData = JsonConvert.DeserializeObject<Dictionary<string,string>> (Team1Data.ElementAt(k).Value.ToString()); 
					_PlayerData.Name = internalPlayerData.Where (a=>a.Key.Contains("Name")).First().Value.ToString();
					_PlayerData.Credit = float.Parse (internalPlayerData.Where (a=>a.Key.Contains("Credit")).First().Value.ToString());
					_PlayerData.Position = internalPlayerData.Where (a=>a.Key.Contains("Position")).First().Value.ToString();
					_MatchData.Team1Players.Add (_PlayerData);
				}
				var Team2Data = JsonConvert.DeserializeObject<Dictionary<string,object>> (internalData.Where (a => a.Key.Contains ("Team2Players")).First ().Value.ToString ());
				for (int k = 0; k < Team2Data.Count; k++) {
					PlayerData _PlayerData = new PlayerData ();
					_PlayerData.PlayerID = Team2Data.ElementAt (k).Key.ToString ();
					var internalPlayerData = JsonConvert.DeserializeObject<Dictionary<string,string>> (Team2Data.ElementAt(k).Value.ToString()); 
					_PlayerData.Name = internalPlayerData.Where (a=>a.Key.Contains("Name")).First().Value.ToString();
					_PlayerData.Credit = float.Parse (internalPlayerData.Where (a=>a.Key.Contains("Credit")).First().Value.ToString());
					_PlayerData.Position = internalPlayerData.Where (a=>a.Key.Contains("Position")).First().Value.ToString();

					_MatchData.Team2Players.Add (_PlayerData);
				}
				var FreeLeagueData = JsonConvert.DeserializeObject<Dictionary<string,object>> (internalData.Where (a => a.Key.Contains ("FreeLeagues")).First ().Value.ToString ());
				for (int k = 0; k < FreeLeagueData.Count; k++) {
					LeagueData _LeagueData = new LeagueData ();
					var internalLeagueData = JsonConvert.DeserializeObject<Dictionary<string,object>> (FreeLeagueData.ElementAt(k).Value.ToString()); 
					_LeagueData.EntryFee = int.Parse (internalLeagueData.Where (a=>a.Key.Contains("EntryFee")).First().Value.ToString());
					_LeagueData.TotalTeams = int.Parse (internalLeagueData.Where (a=>a.Key.Contains("TotalTeams")).First().Value.ToString());
					bool isJoined=false;
					try{
						var EnteredTeamsData = JsonConvert.DeserializeObject<Dictionary<string,object>> (internalLeagueData.Where (a => a.Key.Contains ("EnteredTeams")).First ().Value.ToString ());
						for(int l=0;l<EnteredTeamsData.Count;l++){
							TeamData _TeamData = new TeamData();
							var TeamData = JsonConvert.DeserializeObject<Dictionary<string, object>> (EnteredTeamsData.ElementAt(l).Value.ToString());
							_TeamData.TeamName = EnteredTeamsData.ElementAt (l).Key.ToString ();
							if(_TeamData.TeamName==AuthenticationManager.TeamName)
								isJoined=true;
							for (int b = 0; b < TeamData.Count; b++) { // Entered teams loop if exists
								PlayerData _PlayerData = new PlayerData ();
								_PlayerData.PlayerID = TeamData.ElementAt (b).Key.ToString ();
								var internalPlayerData = JsonConvert.DeserializeObject<Dictionary<string,string>> (TeamData.ElementAt(b).Value.ToString()); 
								_PlayerData.Name = internalPlayerData.Where (a=>a.Key.Contains("Name")).First().Value.ToString();
								_PlayerData.Credit = float.Parse (internalPlayerData.Where (a=>a.Key.Contains("Credit")).First().Value.ToString());
								_PlayerData.Position = internalPlayerData.Where (a=>a.Key.Contains("Position")).First().Value.ToString();
								if(internalPlayerData.Where (a=>a.Key.Contains("isCaptain")).First().Value.ToString()=="true")
									_PlayerData.isCaptain=true;
								if(internalPlayerData.Where (a=>a.Key.Contains("isViceCaptain")).First().Value.ToString()=="true")
									_PlayerData.isViceCaptain=true;
								_TeamData.PlayerList.Add (_PlayerData);

							}
							_LeagueData.EnteredTeams.Add (_TeamData);
						}
					}
					catch{

					}
					_MatchData.FreeLeagues.Add (_LeagueData);
					if(isJoined)
						_MatchData.MyLeagues.Add (_LeagueData);
				}
				var PaidLeagueData = JsonConvert.DeserializeObject<Dictionary<string,object>> (internalData.Where (a => a.Key.Contains ("PaidLeagues")).First ().Value.ToString ());
				for (int k = 0; k < PaidLeagueData.Count; k++) {
					LeagueData _LeagueData = new LeagueData ();
					var internalLeagueData = JsonConvert.DeserializeObject<Dictionary<string,object>> (PaidLeagueData.ElementAt(k).Value.ToString()); 
					_LeagueData.EntryFee = int.Parse (internalLeagueData.Where (a=>a.Key.Contains("EntryFee")).First().Value.ToString());
					_LeagueData.TotalTeams = int.Parse (internalLeagueData.Where (a=>a.Key.Contains("TotalTeams")).First().Value.ToString());
					bool isJoined=false;
					try{
						var EnteredTeamsData = JsonConvert.DeserializeObject<Dictionary<string,object>> (internalLeagueData.Where (a => a.Key.Contains ("EnteredTeams")).First ().Value.ToString ());
						for(int l=0;l<EnteredTeamsData.Count;l++){
							TeamData _TeamData = new TeamData();
							var TeamData = JsonConvert.DeserializeObject<Dictionary<string, object>> (EnteredTeamsData.ElementAt(l).Value.ToString());
							_TeamData.TeamName = EnteredTeamsData.ElementAt (l).Key.ToString ();
							if(_TeamData.TeamName==AuthenticationManager.TeamName)
								isJoined=true;
							for (int b = 0; b < TeamData.Count; b++) { // Entered teams loop if exists
								PlayerData _PlayerData = new PlayerData ();
								_PlayerData.PlayerID = TeamData.ElementAt (b).Key.ToString ();
								var internalPlayerData = JsonConvert.DeserializeObject<Dictionary<string,string>> (TeamData.ElementAt(b).Value.ToString()); 
								_PlayerData.Name = internalPlayerData.Where (a=>a.Key.Contains("Name")).First().Value.ToString();
								_PlayerData.Credit = float.Parse (internalPlayerData.Where (a=>a.Key.Contains("Credit")).First().Value.ToString());
								_PlayerData.Position = internalPlayerData.Where (a=>a.Key.Contains("Position")).First().Value.ToString();
								if(internalPlayerData.Where (a=>a.Key.Contains("isCaptain")).First().Value.ToString()=="true")
									_PlayerData.isCaptain=true;
								if(internalPlayerData.Where (a=>a.Key.Contains("isViceCaptain")).First().Value.ToString()=="true")
									_PlayerData.isViceCaptain=true;
								_TeamData.PlayerList.Add (_PlayerData);

							}
							_LeagueData.EnteredTeams.Add (_TeamData);
						}
					}
					catch{

					}
					_MatchData.PaidLeagues.Add (_LeagueData);
					if(isJoined)
						_MatchData.MyLeagues.Add (_LeagueData);
				}
				_TournamentData.Tournaments.Add (_MatchData);

			}
			_TournamentData.Tournaments.Sort((x, y) => x.Date.CompareTo(y.Date));
			TournamentList.Add (_TournamentData);
		}
		CreateObjects(TournamentList,MatchContent);
	//	AppUIManager.instance.Loading (false);
		//	UIManager.ShowNotification("Example_1_Notification_4", 0.5f, true, "Data retrieved", test);
	}

	public void CreateObjects(List<TournamentData> TournamentList,GameObject MatchContent){
		ClearListing (MatchContent.transform);

		foreach (TournamentData TD in TournamentList) {
			foreach (MatchData _MatchData in TD.Tournaments) {
				GameObject GO = Instantiate (MatchPrefab);
				GO.transform.SetParent (MatchContent.transform);
				GO.GetComponent <MatchItem> ().ItemDetails = _MatchData;
				GO.GetComponent <MatchItem> ().TournamentName = TD.TournamentName;
			//	if(i==0)
			//		GO.GetComponent <MatchItem> ().isSelected = true;
				GO.GetComponent <MatchItem> ().isDataEntry = true;
				GO.GetComponent <MatchItem> ().AssignValues ();
				//i++;
				GO.transform.localScale = Vector3.one;
				MatchContent.GetComponent <RectTransform> ().sizeDelta = new Vector2 (563, MatchContent.GetComponent <RectTransform> ().rect.height + 350);
			}
		}
	}

	public void ClearListing(Transform Content){
		//i = 0;

		foreach (Transform Child in Content)
			Destroy (Child.gameObject);
		Content.GetComponent <RectTransform> ().sizeDelta =  new Vector2 (563, 0);
	}
}
