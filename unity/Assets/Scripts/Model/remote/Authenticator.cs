using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class Authenticator : MonoBehaviour {

	private static Authenticator mInstance = null;
	private static string access_token = "";
	private static readonly int TOKEN_REFRESH_RATE = 14 * 60;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject); // don't break KeepAuthentication co-routine
	}
	
	public static Authenticator Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = (new GameObject("AuthenticatorContainer")).AddComponent<Authenticator>();
			}
			return mInstance;
		}
	}

	private IEnumerator KeepAuthentication(string username, string password, Action<bool, string, string> callback)
	{
		Debug.Log("Authenticator:KeepAuthentication()");

		while (true)
		{
			Debug.Log("Authenticator:KeepAuthentication refreshing token");
			access_token = "";
			
			Action<bool, string, string, string> cb = 
				(success, token, playerID, errorMessage) =>
			{
				if (success)
				{
					access_token = token;
					callback(true, playerID, "");
				}
				else
				{
					callback(false, "", errorMessage);
				}
			};
			
			yield return StartCoroutine(Platform.Instance.Login(username, password, cb));
			yield return new WaitForSeconds(TOKEN_REFRESH_RATE);
		}
	}

	public IEnumerator Login(string username, string password, Action<bool, string, string> callback)
	{
		Debug.Log("Authenticator:Login()");

		access_token = "";

		Action<bool, string, string, string> cb = 
			(success, token, playerID, errorMessage) =>
		{
			if (success)
			{
				access_token = token;
				callback(true, playerID, "");
			}
			else
			{
				callback(false, "", errorMessage);
			}
		};

		yield return StartCoroutine(KeepAuthentication(username, password, callback));
	}

	public IEnumerator Logout(Action<bool> callback)
	{
		Debug.Log("Authenticator:Logout()");
		yield return StartCoroutine(Platform.Instance.Logout(callback));
	}

	public string GetAccessToken()
	{
		return access_token;
	}
}
