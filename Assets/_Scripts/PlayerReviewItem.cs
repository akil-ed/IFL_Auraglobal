using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReviewItem : MonoBehaviour {
	public PlayerData _PlayerData;
	public Text NameTxt,RoleTxt;
	public GameObject CapText,VCText;
	public bool isCaptain, isViceCaptain;
	public Image CapBtn, VCBtn;
	public Sprite ActiveImg,InActiveImg;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Assign(){
		NameTxt.text = _PlayerData.Name;
		RoleTxt.text = _PlayerData.Position;
		if (_PlayerData.isCaptain)
			OnCap ();
		if (_PlayerData.isViceCaptain)
			OnVC ();
	}

	public void ClearCap(){
		_PlayerData.isCaptain = false;
		CapText.SetActive (false);
		CapBtn.sprite = InActiveImg;
	}

	public void ClearVC(){
		_PlayerData.isViceCaptain = false;
		VCText.SetActive (false);
		VCBtn.sprite = InActiveImg;
	}

	public void OnCap(){
		TeamManager.instance.ClearAllCap ();
		if (_PlayerData.isViceCaptain)
			ClearVC ();
		_PlayerData.isCaptain = true;
		CapBtn.sprite = ActiveImg;
		CapText.SetActive (true);
	}

	public void OnVC(){
		TeamManager.instance.ClearAllVC ();
		if (_PlayerData.isCaptain)
			ClearCap ();
		_PlayerData.isViceCaptain = true;
		VCBtn.sprite = ActiveImg;
		VCText.SetActive (true);
	}
}
