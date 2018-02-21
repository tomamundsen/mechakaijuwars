using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public class MainController : MonoBehaviour
{
	private static readonly string Sandbox = "firsttest";
	private static readonly string Version = "8bfcb86d7fd59afb56b05061b8fb248c3c59c96b";

	private void LoadCardData(List<string> files)
	{
		Dictionary<string, CardLevelSheet> cards = new Dictionary<string, CardLevelSheet>();
		foreach (String s in files)
		{
			string path = "data/" + Sandbox + "/" + Version + "/" + s;
			TextAsset txt = (TextAsset)Resources.Load(path, typeof(TextAsset));
			string content = txt.text;
			string d = JSON.Parse(content).ToString();
			Dictionary<string, System.Object> dict = MiniJSON.Json.Deserialize(JSON.Parse(content).ToString()) as Dictionary<string, System.Object>;

			foreach (string cardId in dict.Keys)
			{
				CardLevelSheet card = new CardLevelSheet((Dictionary<string, System.Object>)dict[cardId]);
				cards.Add(card.ID, card);
			}
		}

		Dictionary<string, Card> cardContainers = new Dictionary<string, Card>();

		foreach (string c in cards.Keys)
		{
			try
			{
				CardLevelSheet cSheet = cards[c];
				CardLevelSheet level1 = cards[c];
				CardLevelSheet level2 = null;
				CardLevelSheet level3 = null;

				if (cSheet.Level == "1")
				{
					foreach (string s2 in cards.Keys)
					{
						CardLevelSheet c2 = cards[s2];
						if (level1.NextID == c2.ID)
						{
							level2 = c2;
							break;
						}
					}
					foreach (string s3 in cards.Keys)
					{
						CardLevelSheet c3 = cards[s3];
						if (level2.NextID == c3.ID)
						{
							level3 = c3;
							break;
						}
					}
					cardContainers.Add(level1.ID, new Card(level1, level2, level3));
				}
			}
			catch (Exception e)
			{
				Debug.Log(e.ToString());
			}
		}
		
		GameManager.Instance.CardData = cards;
		GameManager.Instance.Cards = cardContainers;
	}

	private void LoadDeckData(List<string> files)
	{
		Dictionary<string, Deck> gDriveDecks = new Dictionary<string, Deck>();

		foreach (String s in files)
		{
			TextAsset txt = (TextAsset)Resources.Load("data/" + Sandbox + "/" + Version + "/" + s, typeof(TextAsset));
			string content = txt.text;

			List<System.Object> deckList = MiniJSON.Json.Deserialize(JSON.Parse(content).ToString()) as List<System.Object>;
			Deck d = new Deck(s, deckList);
			gDriveDecks.Add(s, d);
		}

		GameManager.Instance.DeckData = gDriveDecks;
	}

	void Start()
	{
		// unzip
		string zipfilePath = "Assets/Resources/data/" + Sandbox + "/" + Version + ".tgz";
		string exportLocation = "Assets/Resources/data/" + Sandbox;
		try
		{
			ZipUtil.Unzip(zipfilePath, exportLocation);
		} catch (Exception e)
		{
			Debug.Log(e.ToString());
		}
		string path = "zip path: data/" + Sandbox + "/" + Version + "/manifest";
		TextAsset txt = (TextAsset)Resources.Load("data/" + Sandbox + "/" + Version + "/manifest", typeof(TextAsset));
		string content = txt.text;
		Dictionary<string, System.Object> manifest = MiniJSON.Json.Deserialize(JSON.Parse(content).ToString()) as Dictionary<string, System.Object>;

		Dictionary<string, System.Object> cards = manifest ["Cards"] as Dictionary<string, System.Object>;
		List<System.Object> cardsCards = cards ["Cards"] as List<System.Object>;

		List<string> cardsFiles = new List<string> ();
		foreach (string s in cardsCards)
		{
			cardsFiles.Add(s);
		}

		LoadCardData(cardsFiles);
		List<System.Object> decks = manifest ["Decks"] as List<System.Object>;
		List<string> decksFiles = new List<string> ();
		foreach (System.Object o in decks)
		{
			string deckName = (string)o;
			decksFiles.Add(deckName);
		}
		LoadDeckData(decksFiles);
	}

	public void LoadArena()
	{
		Application.LoadLevel("Arena");
	}

	public void Logout()
	{
		Action<bool> callback =
			(success) =>
		{
			if (success)
			{
				Debug.Log ("LogoutScript:Logout() succcess");
				//				Application.LoadLevel("login"); #TODO: only load here when we can display an error logging out???
			}
			else
			{
				Debug.Log("LogoutScript:Logout() failure");
			}
			Application.LoadLevel("Login");
		};
		
		StartCoroutine(Authenticator.Instance.Logout(callback));
	}
}
