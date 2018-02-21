
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormatSelectionController : MonoBehaviour
{
	public Text errorLabel;
	public ComboBox comboBox;

	public void Start()
	{
		List<ComboBoxItem> items = new List<ComboBoxItem>();

		ComboBoxItem selectItem = new ComboBoxItem("Select");
		selectItem.OnSelect += () =>
		{
			Debug.Log("info: click select. no-op.");
		};
		items.Add(selectItem);

		ComboBoxItem timedItem = new ComboBoxItem("Timed");
		timedItem.OnSelect += () =>
		{
			errorLabel.text = "Invalid selection.";
		};
		items.Add(timedItem);

		ComboBoxItem untimedItem = new ComboBoxItem("Untimed");
		untimedItem.OnSelect += () =>
		{
			GameManager.Instance.CurrentTimeFormat = "Untimed";
			MatchCriteria criteria = new MatchCriteria(GameManager.Instance.CurrentDeckId,
			                                           GameManager.Instance.CurrentOpponentType,
			                                           GameManager.Instance.CurrentTimeFormat);
			Debug.Log ("RandomOnlineTimeFormatDropdown:SelectTime() - CurrentPlayerID: " + GameManager.Instance.CurrentPlayerId);
			Debug.Log("RandomOnlineTimeFormatDropdown:SelectTime() - match criteria: " + criteria.ToString());
			GameManager.Instance.QuickMatchCriteria = criteria;
			
			Action<bool, TurnBasedMatch, string> callback =
				(success, m, errors) =>
			{
				if (success)
				{
					//Debug.Log("ArenaStartMatch:StartMatch success: " + m.Status);
					GameManager.Instance.QuickMatchCriteria = criteria;
					Application.LoadLevel("Arena");
				}
				else
				{
					Debug.Log("ArenaStarMatch:StartMatch WARN!!!! failed to create match");
				}
			};

			StartCoroutine(Platform.Instance.CreateMatch(criteria, callback));
		};
		items.Add(untimedItem);

		comboBox.ClearItems();
		comboBox.AddItems(items.ToArray());
	}

	public void GoBack()
	{
		Application.LoadLevel("OpponentSelection");
	}
}
