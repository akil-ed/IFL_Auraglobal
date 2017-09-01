using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerItem : MonoBehaviour {
	public PlayerData _PlayerData;
	public Slider _Slider;
	public Text NameTxt,ValueTxt;
	public Image SliderImage;
	public GameObject isActive;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AssignValues(){
		NameTxt.text = _PlayerData.Name.ToUpper ();
		ValueTxt.text = _PlayerData.Credit.ToString ("0.0");
		_Slider.value = _PlayerData.Credit;
		if (_PlayerData.Credit < 5)
			SliderImage.color = Color.red;
		else if (_PlayerData.Credit < 7.9)
			SliderImage.color = Color.yellow;
		else
			SliderImage.color = Color.green;
	}

	public void OnClick(){
		//print(TeamManager.instance.BA_Count);


		if (!isActive.activeSelf) {
			if (_PlayerData.Position == "BA" && TeamManager.instance.BA_Count > 4) {
				AppUIManager.instance.DebugLog ("Only 5 Batsmen allowed");
				return;
			}
			if (_PlayerData.Position == "BL" && TeamManager.instance.BL_Count > 4) {
				AppUIManager.instance.DebugLog ("Only 5 Bowlers allowed");
				return;
			}
			if (_PlayerData.Position == "AR" && TeamManager.instance.AR_Count > 2) {
				AppUIManager.instance.DebugLog ("Only 1-3 All-Rounders allowed");
				return;
			}
			if (_PlayerData.Position == "W" && TeamManager.instance.WK_Count > 0) {
				AppUIManager.instance.DebugLog ("Only 1 WicketKeeper allowed");
				return;
			}

			if (_PlayerData.Position == "Forward" && TeamManager.instance.Fwd_Count > 2) {
				AppUIManager.instance.DebugLog ("Only 3 Forward players allowed");
				return;
			}
			if (_PlayerData.Position == "Midfielder" && TeamManager.instance.Mid_Count > 4) {
				AppUIManager.instance.DebugLog ("Only 3-5 Midfielders allowed");
				return;
			}
			if (_PlayerData.Position == "Defender" && TeamManager.instance.Def_Count > 4) {
				AppUIManager.instance.DebugLog ("Only 3-5 All-Rounders allowed");
				return;
			}
			if (_PlayerData.Position == "GoalKeeper" && TeamManager.instance.GK_Count > 0) {
				AppUIManager.instance.DebugLog ("Only 1 Goal Keeper allowed");
				return;
			}

			if (_PlayerData.Position == "Raider" && TeamManager.instance.R_Count > 2) {
				AppUIManager.instance.DebugLog ("Only 1-3 Raiders allowed");
				return;
			}
			if (_PlayerData.Position == "Allrounder" && TeamManager.instance.A_Count > 1) {
				AppUIManager.instance.DebugLog ("Only 1-2 All-Rounders allowed");
				return;
			}
			if (_PlayerData.Position == "Def" && TeamManager.instance.D_Count > 3) {
				AppUIManager.instance.DebugLog ("Only 2-4 Defenders allowed");
				return;
			}

			if (AppUIManager.GameID < 2) {
				if (TeamManager.instance.TeamCount == 11) {
					AppUIManager.instance.DebugLog ("Only 11 Players allowed");
					return;
				}
			} 
			else {
				if (TeamManager.instance.TeamCount == 7) {
					AppUIManager.instance.DebugLog ("Only 7 Players allowed");
					return;
				}
			}
			if (TeamManager.instance.CreditsRemaining < _PlayerData.Credit){
				AppUIManager.instance.DebugLog ("Not enough credits remaining");
				return;
			}
			TeamManager.instance.AddPlayer (_PlayerData);
		} else {
			TeamManager.instance.RemovePlayer (_PlayerData);
		}
		isActive.SetActive (!isActive.activeSelf);
		
	}

	public void ForceAdd(){
		TeamManager.instance.AddPlayer (_PlayerData);
		isActive.SetActive (true);
	}
}
