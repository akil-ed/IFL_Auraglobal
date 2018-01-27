using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OpenEntryPage : UIBaseClass,IUIExecutable 
{
	#region IUIExecutable implementation

	public void Execute ()
	{
		SetActive (DataEntryUIManager.instance.EntryPage,false);
		SetActive (DataEntryUIManager.instance.MainEntry,true);
		DataEntryUIManager.instance.selectedTeam1 = DataEntryUIManager.instance.Team1Players.Contains (this.GetComponent<EditScoreItem>());
		DataEntryUIManager.instance.selectedPlayerName.text = this.GetComponent<EditScoreItem> ().PlayerNameTXT.text;
		DataEntryUIManager.instance.selectedIndex = DataEntryUIManager.instance.selectedTeam1 ? DataEntryUIManager.instance.Team1Players.IndexOf(this.GetComponent<EditScoreItem>()) : DataEntryUIManager.instance.Team2Players.IndexOf(this.GetComponent<EditScoreItem>());
	}

	#endregion



}
