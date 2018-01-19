using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewItem : MonoBehaviour {
	
	public TextMesh PlayerName;
	public GameObject Model;
	// Use this for initialization
	void Start () {
		PlayerName = GetComponentInChildren <TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
