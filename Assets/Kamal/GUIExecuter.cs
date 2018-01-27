using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIExecuter : MonoBehaviour 
{
	IUIExecutable uiObject;

	void Start()
	{
		uiObject = GetComponent<IUIExecutable> ();
	}

	public void Execute()
	{
		uiObject.Execute ();
	}

	public void Execute(IUIExecutable obj)
	{
		obj.Execute ();
	}
}
