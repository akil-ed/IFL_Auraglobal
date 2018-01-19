using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFantasyCalculate : MonoBehaviour {
	public PlayerData _PlayerData;
	public string Key;
	public Text PlayerNameTXT, ScoreTypeTXT, MultiplierTXT, FantasyPointTXT,TotalFantasyPoints;
	public InputField ScoreTXT;
	public float Multiplier,Points;
	public float TotalPoints;
	// Use this for initialization
	void Start () {
		Multiplier = float.Parse (MultiplierTXT.text);
		Key = ScoreTypeTXT.text;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Assign(){

	}

	public void CalculatePoints(string ScoreTxt){
		TotalPoints = float.Parse (TotalFantasyPoints.text);
		TotalPoints -= Points;
		Points = Multiplier*float.Parse (ScoreTxt);
		TotalPoints += Points;

		FantasyPointTXT.text = Points.ToString ();
		TotalFantasyPoints.text = TotalPoints.ToString ();
	}
}
