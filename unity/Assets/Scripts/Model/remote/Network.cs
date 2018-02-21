
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SimpleJSON;

public class Network : MonoBehaviour
{
	private static Network mInstance = null;
	
	public static Network Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = (new GameObject("NetworkContainer")).AddComponent<Network>();
			}
			return mInstance;
		}
	}

	public IEnumerator Post(string host, string iface, string method, Dictionary<string, string> headers, Hashtable data, Action<bool, string, string> callback)
	{
		Debug.Log("Network:Call() " + host + iface + "/" + method);

		if (headers == null)
		{
			headers = new Dictionary<string, string>();
		}
		headers["Content-Type"] = "application/json";

		if (data == null)
		{
			data = new Hashtable();
		}
		data["access_token"] = Authenticator.Instance.GetAccessToken();

		string url = host + iface + "/" + method;
		string json = MiniJSON.Json.Serialize(data);
		byte[] bytes = Encoding.UTF8.GetBytes(json);
		WWW www = new WWW(url, bytes, headers);
		yield return www;		
		
		string results = "";
		string errors = "";
		
		if (www.error == null)
		{
			Debug.Log("Network:Call() - WWW Ok!: " + www.data);
			var N = JSON.Parse(www.data);
			
			if (N["errors"] != null)
			{
				errors = N["errors"].ToString ();
				Debug.Log ("Network:Call() - MKW Error: " + errors);
			}
			else if (N["results"] != null)
			{
				results = N["results"].ToString();
			}
		}
		else
		{
			string error = www.error;
			Debug.Log("Network:Call() - WWW Error: " + error);
			Hashtable e = new Hashtable();
			e["message"] = error;
			errors = MiniJSON.Json.Serialize(e);
		} 
		
		callback(errors == "" && results != "", results, errors);
	}

	public IEnumerator Get(string host, string endpoint, string queryString, Hashtable headers, Action<bool, string, string> callback)
	{
		Debug.Log("Network:Call() " + host + endpoint + queryString);
		
		string url = host + endpoint + "?access_token=" + Authenticator.Instance.GetAccessToken() + queryString;
		WWW www = new WWW(url);
		yield return www;
		
		string results = "";
		string errors = "";
		
		if (www.error == null)
		{
			Debug.Log("Network:Call() - WWW Ok!: " + www.data);
			var N = JSON.Parse(www.data);
			
			if (N["errors"] != null)
			{
				errors = N["errors"].ToString ();
				Debug.Log ("Network:Call() - MKW Error: " + errors);
			}
			else if (N["results"] != null)
			{
				results = N["results"].ToString();
			}
		}
		else
		{
			string error = www.error;
			Debug.Log("Network:Call() - WWW Error: " + error);
			Hashtable e = new Hashtable();
			e["message"] = error;
			errors = MiniJSON.Json.Serialize(e);
		} 
		
		callback(errors == "" && results != "", results, errors);
	}
}
