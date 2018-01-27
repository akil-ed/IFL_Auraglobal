using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoozyUI;

public class UIBaseClass : MonoBehaviour 
{
	protected void Show(UIElement element)
	{
		element.gameObject.SetActive (true);
		element.Show (false);
	}

	protected void Hide(UIElement element)
	{
		element.Hide (false);
	}

	protected void SetActive(GameObject obj,bool val)
	{
		obj.SetActive (val);
	}
}
