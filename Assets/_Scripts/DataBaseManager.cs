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

public class DataBaseManager : MonoBehaviour {
	public GameObject MatchPrefab,TeamNameWindow;
	public DateTime testtime;
	public List<TournamentData> TournamentList,Football_List,Kabaddi_List;

	public UserData Udata = new UserData();
	public GameObject CricketContent,FootBallContent,KabaddiContent;
	public InputField TeamNameTxt;

	public static DataBaseManager instance = null;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://incrediblefl-affdd.firebaseio.com/");
		FirebaseApp.DefaultInstance.SetEditorP12FileName("IncredibleFL-2439fb8aec19.p12");
		FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("akil03@incrediblefl-affdd.iam.gserviceaccount.com");
		FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	Sprite test;
	public void DebugLog(string s) {
		UIManager.ShowNotification("Example_1_Notification_4", 0.5f, true, s, test);
	}

	public void ReadDataBase(){
		ReadUserData ();
	}

	public void ReadUserData(){

		FirebaseDatabase.DefaultInstance
			.GetReference("Users/"+AuthenticationManager.UserEmail)
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					DebugLog(task.Exception.ToString ());
				}
				else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					string JsonValue = snapshot.GetRawJsonValue ();
					//print(JsonValue);
					SaveUserData(JsonValue);
				}
			}); 
	}

	public void SaveUserData(string JsonValue){
		//print (JsonValue);
		var User = JsonConvert.DeserializeObject<Dictionary<string, object>> (JsonValue);
		Udata.DisplayName = User.Where (a=>a.Key.Contains("DisplayName")).First().Value.ToString();
		try{
		Udata.TeamName = User.Where (a=>a.Key.Contains("TeamName")).First().Value.ToString();
		}
		catch{
			TeamNameWindow.SetActive (true);
		}
		Udata.Email = User.Where (a=>a.Key.Contains("Email")).First().Value.ToString();
		Udata.UserID = User.Where (a=>a.Key.Contains("UserID")).First().Value.ToString();
		Udata.Balance = float.Parse (User.Where (a=>a.Key.Contains("Balance")).First().Value.ToString());
		AuthenticationManager.TeamName = Udata.TeamName;
		AppUIManager.instance.UpdateUserData ();
		ReadTournamentData ();
		ReadFootBallData ();
		ReadKabaddiData ();
	}

	public void SaveTeamName(){
		FirebaseDatabase.DefaultInstance
			.GetReference ("Users/"+AuthenticationManager.UserEmail)
			.Child ("TeamName")
			.SetValueAsync (TeamNameTxt.text);

		Udata.TeamName = TeamNameTxt.text;
		AuthenticationManager.TeamName = Udata.TeamName;
		TeamNameWindow.SetActive (false);
		AppUIManager.instance.UpdateUserData ();
		ReadTournamentData ();
		ReadFootBallData ();
		ReadKabaddiData ();
	}

	public void UpdateBalance(){
		FirebaseDatabase.DefaultInstance
			.GetReference ("Users/"+AuthenticationManager.UserEmail)
			.Child ("Balance")
			.SetValueAsync (Udata.Balance);

		AppUIManager.instance.Wallet.text = "Rs " + Udata.Balance;
	}

	public void ReadTournamentData(){
		//UIManager.ShowNotification("Example_1_Notification_4", 0.5f, true, "Loading Server Data", test);
		AppUIManager.instance.Loading (true);
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
					SaveTournamentData(JsonValue,TournamentList,CricketContent);
				}
			});

	}

	public void ReadFootBallData(){
		//UIManager.ShowNotification("Example_1_Notification_4", 0.5f, true, "Loading Server Data", test);
		AppUIManager.instance.Loading (true);
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
		AppUIManager.instance.Loading (true);
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
		CreateObjects(MatchContent);
		AppUIManager.instance.Loading (false);
	//	UIManager.ShowNotification("Example_1_Notification_4", 0.5f, true, "Data retrieved", test);
	}


	public void CreateObjects(GameObject MatchContent){
		ClearListing (MatchContent.transform);

		foreach (TournamentData TD in TournamentList) {
			foreach (MatchData _MatchData in TD.Tournaments) {
				GameObject GO = Instantiate (MatchPrefab);
				GO.transform.SetParent (MatchContent.transform);
				GO.GetComponent <MatchItem> ().ItemDetails = _MatchData;
				GO.GetComponent <MatchItem> ().TournamentName = TD.TournamentName;
				GO.GetComponent <MatchItem> ().AssignValues ();
				GO.transform.localScale = Vector3.one;
				MatchContent.GetComponent <RectTransform> ().sizeDelta = new Vector2 (563, MatchContent.GetComponent <RectTransform> ().rect.height + 350);
			}
		}
	}

	public void ClearListing(Transform Content){
		foreach (Transform Child in Content)
			Destroy (Child.gameObject);
		Content.GetComponent <RectTransform> ().sizeDelta =  new Vector2 (563, 0);
	}

}

[System.Serializable]
public class TournamentData
{
	public string TournamentName;
	public int index;
	public List<MatchData> Tournaments = new List<MatchData>();
}

[System.Serializable]
public class MatchData
{
	public string MatchName;
	public int index;
	public string TournamentName;
	public string Team1;
	public string Team2;
	public List<PlayerData> Team1Players = new List<PlayerData>();
	public List<PlayerData> Team2Players = new List<PlayerData>();
	public List<PlayerData> MyTeam = new List<PlayerData>();
	public List<LeagueData> FreeLeagues = new List<LeagueData> ();
	public List<LeagueData> PaidLeagues = new List<LeagueData> ();
	public List<LeagueData> MyLeagues = new List<LeagueData> ();
	public DateTime Date;
}

[System.Serializable]
public class PlayerData
{
	public string Name;
	public string TeamName;
	public string PlayerID;
	public float Credit;
	public string Position;
	public bool isCaptain=false;
	public bool isViceCaptain=false;
	public int Score=0;
	public float FantasyPoints=0;
}

[System.Serializable]
public class LeagueData
{
	public int EntryFee;
	public int TotalTeams;
	public int Winnings;
	public List<TeamData> EnteredTeams = new List<TeamData>();
}

[System.Serializable]
public class TeamData
{
	public string TeamName;
	public List<PlayerData> PlayerList = new List<PlayerData>();
}