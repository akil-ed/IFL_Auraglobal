using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DoozyUI;
public class MatchItem : MonoBehaviour {
	public MatchData ItemDetails;
	public string TournamentName;
	public Text MatchNameTXT,TournamentNameTXT,TimerTXT;
	public GameObject LiveImage;
	TimeSpan TimeDifference;
	public bool isLive;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLive) {
			TimeDifference = ItemDetails.Date.Subtract (DateTime.Now);
			TimerTXT.text = string.Format("{0:00}:{1:00}:{2:00}",
				(int)TimeDifference.TotalHours,
				TimeDifference.Minutes,
				TimeDifference.Seconds);// TimeDifference.ToString ();
//			if (TimeDifference.TotalSeconds < 60) {
//				TimerTXT.text = "";
//				LiveImage.SetActive (true);
//				isLive = true;
//			}
		}

	}

	public void AssignValues(){
		MatchNameTXT.text = ItemDetails.Team1.ToUpper () + "\nVS\n" + ItemDetails.Team2.ToUpper ();
		TournamentNameTXT.text = TournamentName;
		if (isLive) {
			TimerTXT.text = "";
			LiveImage.SetActive (true);
		}
	}

	public void Join(){
		AppUIManager.instance.HomePage.Hide (true);

		TeamManager.instance.SelectedMatch = ItemDetails;
		TeamManager.instance.DisplayListings (ItemDetails);
	}
}
