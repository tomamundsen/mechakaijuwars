using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

public class DeckSelectionController : MonoBehaviour
{
	public ComboBox comboBox;
	public Sprite image;
	public Text errorLabel;
	
	private void Start() 
	{
		List<ComboBoxItem> items = new List<ComboBoxItem>();
		ComboBoxItem selectItem = new ComboBoxItem("Select");
		items.Add(selectItem);
		comboBox.AddItems(items.ToArray());

		Action<bool, Dictionary<string, string>, string> callback =
			(success, decks, errorMessage) =>
		{
			if (success)
			{
				List<ComboBoxItem> newItems = new List<ComboBoxItem>();

				foreach (KeyValuePair<string, string> entry in decks)
				{
					Debug.Log("found deck: " + entry.Key);
					ComboBoxItem newItem = new ComboBoxItem(entry.Value);
					newItem.OnSelect += () =>
					{
						Debug.Log("selected deck: " + entry.Value);
						errorLabel.text = "";

						GameManager.Instance.CurrentDeckId = entry.Value;
						Application.LoadLevel("OpponentSelection");
					};
					newItems.Add(newItem);
				}

//				comboBox.ClearItems();
				comboBox.AddItems(newItems.ToArray());
			}
			else
			{
				Debug.Log("WARN!!! didn't get decks");
				errorLabel.text = "didn't get decks";
			}
		};
		
		StartCoroutine(Platform.Instance.ListDecks(GameManager.Instance.CurrentPlayerId, callback));

		List<ComboBoxItem> gDriveItems = new List<ComboBoxItem>();

		foreach (KeyValuePair<string, Deck> kvp in GameManager.Instance.DeckData)
		{
			ComboBoxItem gDriveItem = new ComboBoxItem(kvp.Key);
			gDriveItem.OnSelect += () =>
			{
				errorLabel.text = "";
				
				GameManager.Instance.CurrentDeckId = kvp.Key;
				Application.LoadLevel("OpponentSelection");
			};
			gDriveItems.Add(gDriveItem);
		}

		comboBox.AddItems(gDriveItems.ToArray());
	}

	public void GoBack()
	{
		Application.LoadLevel("Arena");
	}
}
