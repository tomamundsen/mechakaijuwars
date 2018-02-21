using UnityEngine;
using System;
using System.Collections;

public class BackButton : MonoBehaviour {

	public string sceneName;

	public void GoBack()
	{
		if (sceneName != "")
		{
			Application.LoadLevel(sceneName);
		}
	}
}
