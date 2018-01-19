using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewManager : MonoBehaviour {
	public TeamData _TeamData;
	public PreviewItem[] PlayerPositions;
	public GameObject Batsman, Bowler, AR, WK;
	// Use this for initialization
	public static PreviewManager instance;
	void Awake(){
		instance = this;
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		  
	}

	public void AssignPlayers(){




		for (int i = 0; i < 11; i++) {
			
			PlayerPositions [i].PlayerName.text = _TeamData.PlayerList [i].Name;
			PlayerPositions [i].transform.rotation = Quaternion.identity;
			PlayerPositions [i].PlayerName.transform.parent.localPosition = new Vector3 (0, 0.03f, -0.09f);
			PlayerPositions [i].PlayerName.transform.rotation = Quaternion.Euler (35, 0, 0);
			GameObject GO;
			if (_TeamData.PlayerList [i].Position == "BA") {
				GO = Instantiate (Batsman) as GameObject;
				GO.transform.SetParent (PlayerPositions [i].transform);
				GO.transform.localPosition = Vector3.zero;
				GO.transform.localRotation = Quaternion.identity;
				GO.transform.localScale = Vector3.one;
				Destroy (PlayerPositions [i].Model);
				PlayerPositions [i].Model = GO;
				PlayerPositions [i].PlayerName.text += " (BA)";
				//return;
			}
			if (_TeamData.PlayerList [i].Position == "BL") {
				GO = Instantiate (Bowler) as GameObject;
				GO.transform.SetParent (PlayerPositions [i].transform);
				GO.transform.localPosition = Vector3.zero;
				GO.transform.localRotation = Quaternion.identity;
				GO.transform.localScale = Vector3.one;
				Destroy (PlayerPositions [i].Model);
				PlayerPositions [i].Model = GO;
				PlayerPositions [i].PlayerName.text += " (BL)";
				//return;
			}
			if (_TeamData.PlayerList [i].Position == "AR") {
				GO = Instantiate (AR) as GameObject;
				GO.transform.SetParent (PlayerPositions [i].transform);
				GO.transform.localPosition = Vector3.zero;
				GO.transform.localRotation = Quaternion.identity;
				GO.transform.localScale = Vector3.one;
				Destroy (PlayerPositions [i].Model);
				PlayerPositions [i].Model = GO;
				PlayerPositions [i].PlayerName.text += " (AR)";
				//return;
			}
			if (_TeamData.PlayerList [i].Position == "W") {
				GO = Instantiate (WK) as GameObject;
				GO.transform.SetParent (PlayerPositions [i].transform);
				GO.transform.localPosition = Vector3.zero;
				GO.transform.localRotation = Quaternion.identity;
				GO.transform.localScale = Vector3.one;
				Destroy (PlayerPositions [i].Model);
				PlayerPositions [i].Model = GO;
				PlayerPositions [i].PlayerName.text += " (WK)";
				//return;
			}
		}

	}
}
