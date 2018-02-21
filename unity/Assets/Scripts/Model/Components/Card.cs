
using System;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
	private Dictionary<int, CardLevelSheet> cardLevelSheets = new Dictionary<int, CardLevelSheet>();

	public Card (CardLevelSheet l1, CardLevelSheet l2, CardLevelSheet l3)
	{
		cardLevelSheets.Add(1, l1);
		cardLevelSheets.Add(2, l2);
		cardLevelSheets.Add(3, l3);
	}

	public CardLevelSheet GetLevelSheet(int i)
	{
		if (cardLevelSheets.ContainsKey(i))
		{
			return cardLevelSheets[i];
		}
		else
		{
			Debug.Log("!!! Warn - Card:GetLevelSheet - unknown sheet index: " + i);
			return null;
		}
	}
}
