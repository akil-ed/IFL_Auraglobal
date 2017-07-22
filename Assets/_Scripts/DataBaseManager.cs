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

public class DataBaseManager : MonoBehaviour {
	public GameObject MatchPrefab;
	public DateTime testtime;
	public List<TournamentData> TournamentList;

	public GameObject MatchContent;

	// Use this for initialization
	void Start () {
		//print (System.DateTime.Now.ToString ());
		//testtime = DateTime.Parse ("7/20/2017 5:02:22 AM");

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://incrediblefl-affdd.firebaseio.com/");
		FirebaseApp.DefaultInstance.SetEditorP12FileName("IncredibleFL-2439fb8aec19.p12");
		FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("akil03@incrediblefl-affdd.iam.gserviceaccount.com");
		FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");

		//ReadTournamentData ();
		//Incredible Fantasy League-bc7f14f186df.p12
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	Sprite test;
	public void DebugLog(string s) {
		UIManager.ShowNotification("Example_1_Notification_4", 0.5f, true, s, test);
	}

	public void ReadTournamentData(){
		UIManager.ShowNotification("Example_1_Notification_4", 0.5f, true, "Loading Server Data", test);
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
					//print(JsonValue);
					SaveTournamentData(JsonValue);
				}
			});

	}

	public void SaveTournamentData(string JsonValue){
		var Tournaments = JsonConvert.DeserializeObject<Dictionary<string, object>> (JsonValue);
		for (int i = 0; i < Tournaments.Count; i++) { // Tournament Loop
			TournamentData _TournamentData = new TournamentData();
			_TournamentData.TournamentName = Tournaments.ElementAt (i).Key.ToString ();
			var Matches = JsonConvert.DeserializeObject<Dictionary<string, object>> (Tournaments.ElementAt(i).Value.ToString());
			for (int j = 0; j < Matches.Count; j++) { // Games in each Tournament Loop
				MatchData _MatchData = new MatchData ();
				_MatchData.MatchName = Matches.ElementAt (j).Key.ToString ();
				_MatchData.TournamentName = _TournamentData.TournamentName;
				var internalData = JsonConvert.DeserializeObject<Dictionary<string,object>> (Matches.ElementAt(j).Value.ToString()); 
				_MatchData.Team1 = internalData.Where (a=>a.Key.Contains("Team1")).First().Value.ToString();
				_MatchData.Team2 = internalData.Where (a=>a.Key.Contains("Team2")).First().Value.ToString();
				_MatchData.Date = DateTime.Parse (internalData.Where (a=>a.Key.Contains("Date")).First().Value.ToString());
				try{
				var TotalUserData = JsonConvert.DeserializeObject<Dictionary<string,object>> (internalData.Where (a => a.Key.Contains ("Users")).First ().Value.ToString ());
					var UserData = JsonConvert.DeserializeObject<Dictionary<string,object>> (TotalUserData.Where (a => a.Key.Contains (AuthenticationManager.UserEmail)).First ().Value.ToString ());
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
				//print (UserData.Count);
				//print (UserData.ToString ());
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
					_PlayerData.PlayerID = Team1Data.ElementAt (k).Key.ToString ();
					var internalPlayerData = JsonConvert.DeserializeObject<Dictionary<string,string>> (Team2Data.ElementAt(k).Value.ToString()); 
					_PlayerData.Name = internalPlayerData.Where (a=>a.Key.Contains("Name")).First().Value.ToString();
					_PlayerData.Credit = float.Parse (internalPlayerData.Where (a=>a.Key.Contains("Credit")).First().Value.ToString());
					_PlayerData.Position = internalPlayerData.Where (a=>a.Key.Contains("Position")).First().Value.ToString();

					_MatchData.Team2Players.Add (_PlayerData);
				}
				_TournamentData.Tournaments.Add (_MatchData);
				//CreateObjects (_MatchData);
			}
			TournamentList.Add (_TournamentData);
		}
		CreateObjects();
		AppUIManager.instance.Loading (false);
		UIManager.ShowNotification("Example_1_Notification_4", 0.5f, true, "Data retrieved", test);
	}

	public void UserTeam(string JsonValue){

	}


	public void CreateObjects(){
		foreach (TournamentData TD in TournamentList) {
			foreach (MatchData _MatchData in TD.Tournaments) {
				GameObject GO = Instantiate (MatchPrefab);
				GO.transform.parent = MatchContent.transform;
				GO.GetComponent <MatchItem> ().ItemDetails = _MatchData;
				GO.GetComponent <MatchItem> ().TournamentName = TD.TournamentName;
				GO.GetComponent <MatchItem> ().AssignValues ();
				GO.transform.localScale = Vector3.one;
				MatchContent.GetComponent <RectTransform> ().sizeDelta = new Vector2 (563, MatchContent.GetComponent <RectTransform> ().rect.height + 350);
				//MatchContent.GetComponent <RectTransform> ().rect = new Rect (MatchContent.GetComponent <RectTransform> ().rect.x,MatchContent.GetComponent <RectTransform> ().rect.y,MatchContent.GetComponent <RectTransform> ().rect.width,MatchContent.GetComponent <RectTransform> ().rect.height+340);
			}
		}
	}

}

[System.Serializable]
public class TournamentData
{
	public string TournamentName;
	public List<MatchData> Tournaments = new List<MatchData>();
}

[System.Serializable]
public class MatchData
{
	public string MatchName;
	public string TournamentName;
	public string Team1;
	public string Team2;
	public List<PlayerData> Team1Players = new List<PlayerData>();
	public List<PlayerData> Team2Players = new List<PlayerData>();
	public List<PlayerData> MyTeam = new List<PlayerData>();
	public DateTime Date;
}

[System.Serializable]
public class PlayerData
{
	public string Name;
	public string PlayerID;
	public float Credit;
	public string Position;
	public bool isCaptain=false;
	public bool isViceCaptain=false;
	public int Score=0;
	public float FantasyPoints=0;
}
