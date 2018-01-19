using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataEntryUIManager : MonoBehaviour {
	public Image[] EntryPageButtons;
	public GameObject MainPage, EntryPage, MainEntry;
	public GameObject[]  EntryPageItems;//, FootBallPageItems, KabaddiPageItems;
	public Color Selected,Unselected;
	public MatchData SelectedMatch;
	public string SelectedLeague;

	public static DataEntryUIManager instance;

	public Text MatchNameTxt, Team1NameTxt, Team2NameTxt;
	public GameObject PlayerItem;
	public GameObject[] TeamContents;
	public List<EditScoreItem> Team1Players,Team2Players;

	void OnEnable(){
		instance = this;
		MainPage.SetActive (true);
		EntryPage.SetActive (false);
	}


	// Use this for initialization
	void Start () {
		SetEntryPageGame (1);
	}
	
	public void SetEntryPageGame(int page){

		MainPageInactive ();
		EntryPageButtons [page-1].color = Selected;
		EntryPageButtons [page-1].GetComponentInChildren <Text> ().color = Color.black;
		EntryPageItems [page-1].SetActive (true);

		return;



		switch (page) {
		case 1:
			MainPageInactive ();
			EntryPageButtons [0].color = Selected;
			EntryPageButtons [0].GetComponentInChildren <Text> ().color = Color.black;
			EntryPageItems [0].SetActive (true);
		//	FootBallPageItems [0].SetActive (true);
		//	KabaddiPageItems [0].SetActive (true);
			break;
		case 2:
			MainPageInactive ();
			EntryPageButtons [1].color = Selected;
			EntryPageButtons [1].GetComponentInChildren <Text> ().color = Color.black;
			EntryPageItems [1].SetActive (true);
			//	FootBallPageItems [0].SetActive (true);
			//	KabaddiPageItems [0].SetActive (true);
			break;
		case 3:
			MainPageInactive ();
			EntryPageButtons [2].color = Selected;
			EntryPageButtons [2].GetComponentInChildren <Text> ().color = Color.black;
			EntryPageItems [2].SetActive (true);
			//	FootBallPageItems [0].SetActive (true);
			//	KabaddiPageItems [0].SetActive (true);
			break;
			//		case 4:
			//			MainPageInactive ();
			//			MainPageButtons [3].sprite = MainPageHighLightActive;
			//			MainPageItems [3].SetActive (true);
			//			FootBallPageItems [3].SetActive (true);
			//			KabaddiPageItems [3].SetActive (true);
			//			break;
		}
	}

	public void MainPageInactive(){

		for (int i = 0; i < EntryPageButtons.Length; i++) {
			EntryPageButtons [i].color = Unselected;
			EntryPageButtons [i].GetComponentInChildren <Text> ().color = Color.white;
			EntryPageItems [i].SetActive (false);
		}

	}

	public void SwapPage(){
		if (MainPage.activeSelf) {
			MainPage.SetActive (false);
			EntryPage.SetActive (true);
			SetEntryPageGame (4);
		} else {
			MainPage.SetActive (true);
			EntryPage.SetActive (false);
			SetEntryPageGame (1);
		}
	}

	public void OpenEntry(){
		EntryPage.SetActive (false);
		MainEntry.SetActive (true);
	}

	public void CloseEntry(){
		EntryPage.SetActive (true);
		MainEntry.SetActive (false);
	}


	public void CreateMatchListing(){
		MatchNameTxt.text = SelectedMatch.MatchName;
		Team1NameTxt.text = SelectedMatch.Team1;
		Team2NameTxt.text = SelectedMatch.Team2;

		ClearListing(TeamContents [0].transform);
		ClearListing(TeamContents [1].transform);
		Team1Players.Clear ();
		Team2Players.Clear ();

		foreach (PlayerData PD in SelectedMatch.Team1Players) {
			GameObject GO = Instantiate (PlayerItem);
			GO.GetComponent <EditScoreItem> ()._PlayerData = PD;
			GO.GetComponent <EditScoreItem> ().AssignValues ();
			GO.transform.SetParent (TeamContents [0].transform);
			GO.transform.localScale = Vector3.one;
			Team1Players.Add (GO.GetComponent <EditScoreItem> ());
			TeamContents [0].GetComponent <RectTransform> ().sizeDelta = new Vector2 (565, TeamContents [0].GetComponent <RectTransform> ().rect.height + 80);
		}


		foreach (PlayerData PD in SelectedMatch.Team2Players) {
			GameObject GO = Instantiate (PlayerItem);
			GO.GetComponent <EditScoreItem> ()._PlayerData = PD;
			GO.GetComponent <EditScoreItem> ().AssignValues ();
			GO.transform.SetParent (TeamContents [1].transform);
			GO.transform.localScale = Vector3.one;
			Team2Players.Add (GO.GetComponent <EditScoreItem> ());
			TeamContents [1].GetComponent <RectTransform> ().sizeDelta = new Vector2 (565, TeamContents [1].GetComponent <RectTransform> ().rect.height + 80);
		}

	}

	public void ClearListing(Transform Content){
		//i = 0;

		foreach (Transform Child in Content)
			Destroy (Child.gameObject);
		Content.GetComponent <RectTransform> ().sizeDelta =  new Vector2 (563, 0);
	}


	public void PublishChanges(){
		foreach (EditScoreItem GO in Team1Players) {
			GO.UpdateScore ();
		}
		foreach (EditScoreItem GO in Team2Players) {
			GO.UpdateScore ();
		}
	}
}
