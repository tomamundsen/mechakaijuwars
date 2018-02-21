using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class ImportJSON : MonoBehaviour
{
	Dictionary<string, CardLevelSheet> cards;
	List<Deck> decks;

	void Start ()
	{
		Debug.Log("ImportJSON:Start()");

		cards = new Dictionary<string, CardLevelSheet>();
		decks = new List<Deck>();

		TextAsset txt = (TextAsset)Resources.Load("cards", typeof(TextAsset));
		string content = txt.text;

		List<System.Object> dict = MiniJSON.Json.Deserialize(JSON.Parse(content).ToString()) as List<System.Object>;

		foreach (var c in dict)
		{
			Debug.Log(c);
			CardLevelSheet card = new CardLevelSheet((Dictionary<string, System.Object>)c);
			cards.Add(card.ID, card);
		}

		txt = (TextAsset)Resources.Load("decks", typeof(TextAsset));
		content = txt.text;
		
		Dictionary<string, System.Object> deckDictionary = MiniJSON.Json.Deserialize(JSON.Parse(content).ToString()) as Dictionary<string, System.Object>;

		foreach (KeyValuePair<string, System.Object> kvp in deckDictionary) {
			Deck d = new Deck(kvp.Key, (List<System.Object>)kvp.Value);
			Debug.Log(kvp.Key);
			decks.Add(d);
		}
	}
}
