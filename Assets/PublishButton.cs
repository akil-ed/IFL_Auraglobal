using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublishButton : UIBaseClass,IUIExecutable
{
	#region IUIExecutable implementation

	public void Execute ()
	{
		SetActive (DataEntryUIManager.instance.MainEntry,false);
		SetActive (DataEntryUIManager.instance.EntryPage,true);
		DataEntryUIManager.instance.SetPlayerScoreAndFP ();
	}

	#endregion


}
