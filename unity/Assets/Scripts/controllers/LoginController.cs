using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class LoginController : MonoBehaviour {

	public InputField usernameTextbox;
	public InputField passwordTextbox;
	public Text errorLabel;
	public Button submitButton;

	public void Wrapper()
	{
		Debug.Log ("Test");
		StartCoroutine(Login ());
	}

	public IEnumerator Login ()
	{
		errorLabel.text = "";

		Action<bool, string, string> callback =
			(success, playerID, errorMessage) =>
		{
			if (success)
			{
				Debug.Log ("SubmitButton:Login() - success");
				GameManager.Instance.CurrentPlayerId = playerID;
				Application.LoadLevel("Main");
			}
			else
			{
				Debug.Log ("SubmitButton:Login() - failure: " + errorMessage);
				errorLabel.text = errorMessage;
			}
		};

		yield return StartCoroutine(Authenticator.Instance.Login(usernameTextbox.text, passwordTextbox.text, callback));
	}
}
