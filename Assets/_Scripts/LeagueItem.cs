using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Firebase.Database;
using Newtonsoft.Json;


public class LeagueItem : MonoBehaviour {
	public LeagueData _LeagueData;
	public Slider _Slider;
	public Text WinningsTxt,CostTxt,TeamTxt,JoinTxt;
	public int LeagueNo;
	public string LeagueType="FreeLeagues";
	public GameObject TeamGrid,TeamNamePrefab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AssignValues(){
		_Slider.maxValue = _LeagueData.TotalTeams;
		_Slider.value = _LeagueData.EnteredTeams.Count;
		if (_LeagueData.EntryFee > 0) {
			WinningsTxt.text = "Win Rs " + _LeagueData.TotalTeams * _LeagueData.EntryFee + " !!";
			LeagueType = "PaidLeagues";
		}
		CostTxt.text = "Rs " + _LeagueData.EntryFee;
		TeamTxt.text = _LeagueData.EnteredTeams.Count+"/"+_LeagueData.TotalTeams+" Teams Joined";
		if (_LeagueData.EnteredTeams.Exists (x => x.TeamName == AuthenticationManager.TeamName)) {
			JoinTxt.transform.parent.GetComponent <Button> ().interactable = false;
			JoinTxt.text = "Joined";
		}
	}

	public void OnClick(){
		AppUIManager.instance.SelectedLeague._LeagueData = _LeagueData;
		AppUIManager.instance.ShowSelectedLeague ();
	}

	public void JoinLeague(){
		int index = DataBaseManager.instance.TournamentList [0].Tournaments.IndexOf (TeamManager.instance.SelectedMatch);
		TeamData TD = new TeamData ();
		TD.TeamName = AuthenticationManager.TeamName;

		foreach(PlayerData PD in TeamManager.instance.MyList){
			TD.PlayerList.Add (PD);

			DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
			reference.Child ("Cricket").Child ("Tournament")
				.Child (TeamManager.instance.SelectedMatch.TournamentName)
				.Child (TeamManager.instance.SelectedMatch.MatchName)
				.Child (LeagueType)
				.Child ("League"+LeagueNo)
				.Child ("EnteredTeams")
				.Child (AuthenticationManager.TeamName)
				.Child (PD.PlayerID)
				.SetRawJsonValueAsync (JsonConvert.SerializeObject (PD));
		}

		_Slider.value = _LeagueData.EnteredTeams.Count+1;
		TeamTxt.text = (_LeagueData.EnteredTeams.Count+1)+"/"+_LeagueData.TotalTeams+" Teams Joined";
		JoinTxt.text = "Joined";
		JoinTxt.transform.parent.GetComponent <Button> ().interactable = false;


		if(LeagueType=="FreeLeagues")
			TeamManager.instance.SelectedMatch.FreeLeagues [LeagueNo - 1].EnteredTeams.Add (TD);
		else
			TeamManager.instance.SelectedMatch.PaidLeagues [LeagueNo - 1].EnteredTeams.Add (TD);

		TeamManager.instance.SelectedMatch.MyLeagues.Add (_LeagueData);
		AppUIManager.instance.ContestJoinedTxt.text = "Contests Joined (" + TeamManager.instance.SelectedMatch.MyLeagues.Count + ")";
		DataBaseManager.instance.TournamentList [TeamManager.instance.SelectedTournamentIndex].Tournaments [TeamManager.instance.SelectedMatchIndex] = TeamManager.instance.SelectedMatch;

		//TeamManager.instance.CreateLeagueListing ();
	}

	public void ShowTeams(){
		foreach (Transform Child in TeamGrid.transform)
			Destroy (Child.gameObject);

	
		foreach (TeamData TD in _LeagueData.EnteredTeams) {
			GameObject GO = Instantiate (TeamNamePrefab);
			GO.transform.SetParent (TeamGrid.transform);
			GO.GetComponent <Text> ().text = TD.TeamName;
			GO.transform.localScale = Vector3.one;

		}
	}
}
