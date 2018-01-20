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
	public GameObject SelectedImage;
	TimeSpan TimeDifference;
	public bool isLive,isSelected,isDataEntry;
	public int TournamentIndex,MatchIndex;
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
		if (isSelected) {
			//TimerTXT.text = "";

			Join ();
		}
	}

	public void Join(){
		if (isDataEntry) {
			DataEntryUIManager.instance.SelectedMatch = ItemDetails;
			DataEntryUIManager.instance.CreateMatchListing ();
			DataEntryUIManager.instance.SwapPage ();
			return;
		}



		//AppUIManager.instance.HomePage.Hide (true);
		SelectedImage.SetActive (true);
		isSelected = true;
		TeamManager.instance.SelectedTournamentIndex = TournamentIndex;
		TeamManager.instance.SelectedMatchIndex = MatchIndex;
		TeamManager.instance.SelectedMatch = ItemDetails;
		TeamManager.instance.CreateLeagueListing ();
		//TeamManager.instance.DisplayListings ();

	}
}
