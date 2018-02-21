using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ArenaController : MonoBehaviour
{
	public Text errorLabel;
	public ComboBox comboBox;
	public Sprite image;

	private readonly static int MATCH_REFRESH_RATE = 5;
	private Dictionary<string, TurnBasedMatch> matchData = new Dictionary<string, TurnBasedMatch>();
	
	private void Start() 
	{
		var select = new ComboBoxItem("Select");
		comboBox.AddItems(select);
		StartCoroutine(RefreshMatches());
	}

	private IEnumerator RefreshMatches()
	{
		while (true)
		{
			List<string> matches = null;

			Action<bool, List<string>, string> callback =
				(success, result, errorMessage) =>
			{
				if (success)
				{
					matches = result;
				}
			};
			
			yield return StartCoroutine(Platform.Instance.ListMatches(callback));

			if (matches != null)
			{
				List<ComboBoxItem> newItems = new List<ComboBoxItem>();

				ComboBoxItem selectItem = new ComboBoxItem("Select");
				newItems.Add(selectItem);

				ComboBoxItem newMatchItem = new ComboBoxItem("New Match");
				newMatchItem.OnSelect += () =>
				{
					Debug.Log("Start new match");
					Application.LoadLevel("DeckSelection");
				};
				newItems.Add(newMatchItem);

				ComboBoxItem quickMatchItem = new ComboBoxItem("Quick Match");
				quickMatchItem.OnSelect += () =>
				{
					Debug.Log("Start quick match");
					MatchCriteria criteria = GameManager.Instance.QuickMatchCriteria;
					if (criteria != null)
					{
						Debug.Log ("ArenaStartMatch:StartMatch() - criteria: " + criteria);
						// TODO: Platform.JoinQueue
					}
					else
					{
						Debug.Log("ArenaStartMatch:StartMatch() - no quick match criteria to load");
					}
				};
				newItems.Add(quickMatchItem);

				foreach (string m in matches)
				{
					Debug.Log("found match: " + m);

					Action<bool, TurnBasedMatch, string> cb =
						(success, matchInfo, errors) =>
					{
						if (success)
						{
							string matchId = matchInfo.MatchId;
							string status = matchInfo.Status;

							Debug.Log("ArenaStartMatch:RefreshMatches() status: " + status);
							if (status == "MATCH_ACTIVE" || status == "MATCH_AUTO_MATCHING")
							{
								try
								{
									matchData.Remove(matchId);
									matchData.Add(matchId, matchInfo);
									ComboBoxItem newItem = new ComboBoxItem(matchId + " " +  status);
									newItem.OnSelect += () =>
									{
										Debug.Log("item " + matchId);
										string[] stringSeparators = new string[] {" "};
										string[] data;
										string matchCaption = newItem.Caption;
										data = matchCaption.Split(stringSeparators, StringSplitOptions.None);
										string selectedMatch = data[0];
										
										if (matchId == selectedMatch)
										{
											Debug.Log("ArenaStartMatch:StartMatch() - starting: " + matchCaption);
											GameManager.Instance.CurrentMatchId = matchId;
											// TODO: Platform:JoinMatch ??? not needed with current architecture
											Application.LoadLevel("Match");
										}
									};
									newItems.Add(newItem);
								} catch (ArgumentException e) {
									// 6nothing
									Debug.Log("warn!!! " + e);
								}
							}
						}
						else
						{
							Debug.Log("WARN!!! match info null");
						}
					};
					yield return StartCoroutine(Platform.Instance.GetMatchInfo(m, cb));
				}
				comboBox.ClearItems();
				comboBox.AddItems(newItems.ToArray());
			}
			yield return new WaitForSeconds(MATCH_REFRESH_RATE);
		}
	}

	public void GoBack()
	{
		Application.LoadLevel("Main");
	}
}
