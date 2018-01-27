using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditScoreItem : MonoBehaviour {
	public PlayerData _PlayerData;
	public Text PlayerNameTXT, PositionTXT, FantasyPointTXT,ScoreTXT;
	//public InputField ScoreTXT;
	public float currentpoints;
	public InputField scoreIF;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AssignValues(){
		currentpoints = _PlayerData.FantasyPoints;
		PlayerNameTXT.text = _PlayerData.Name;
		PositionTXT.text = _PlayerData.Position;
		FantasyPointTXT.text = _PlayerData.FantasyPoints.ToString ("0.00");
		ScoreTXT.text = _PlayerData.Score.ToString ();
		//scoreIF.text = _PlayerData.Score.ToString();
	}

	public void UpdateScore()
	{
//		float newpoints = 0f;
//		if(!string.IsNullOrEmpty(ScoreTXT.text))
//		newpoints = float.Parse (ScoreTXT.text);
//		if (newpoints > 0) {
//			currentpoints += newpoints;
//			FantasyPointTXT.text = currentpoints.ToString ("0.00");
//			//ScoreTXT.text = "0";
//		}

		if(!string.IsNullOrEmpty(ScoreTXT.text))
		_PlayerData.Score = int.Parse(ScoreTXT.text);
		else
			_PlayerData.Score = 0;
		if (!string.IsNullOrEmpty (FantasyPointTXT.text))
			_PlayerData.FantasyPoints = float.Parse (FantasyPointTXT.text);
		else
			_PlayerData.FantasyPoints = 0;
	}
}
