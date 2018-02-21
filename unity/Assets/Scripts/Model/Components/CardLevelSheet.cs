
using System;
using System.Collections.Generic;

using UnityEngine;

public class CardLevelSheet
{
	public string Art { get; private set; }
	public string Type { get; private set; }
	public string Cost { get; private set; }
	public string Faction { get; private set; }
	public string ID { get; private set; }
	public string Keyword { get; private set; }
	public string Level { get; private set; }
	public string Name { get; private set; }
	public string NextID { get; private set; }
	public string PrevID { get; private set; }
	public string Rarity { get; private set; }
	public string Set { get; private set; }
	public string Subtype { get; private set; }
	public string Text { get; private set; }

	public CardLevelSheet (CardLevelSheet c)
	{
		Art = c.Art;
		Type = c.Type;
		Cost = c.Cost;
		Faction = c.Faction;
		ID = c.ID;
		Keyword = c.Keyword;
		Level = c.Level;
		Name = c.Name;
		NextID = c.NextID;
		PrevID = c.PrevID;
		Rarity = c.Rarity;
		Set = c.Set;
		Subtype = c.Subtype;
		Text = c.Text;
	}

	public CardLevelSheet (Dictionary<string, System.Object> d)
	{
		System.Object obj;

		d.TryGetValue("Art", out obj);
		Art = (string) obj;

		d.TryGetValue("CardType", out obj);
		Type = (string) obj;

		d.TryGetValue("Cost", out obj);
		Cost = (string) obj;

		d.TryGetValue("Faction", out obj);
		Faction = (string) obj;

		d.TryGetValue("ID", out obj);
		ID = (string) obj;

		d.TryGetValue("Keyword", out obj);
		Keyword = (string) obj;

		d.TryGetValue("Level", out obj);
		Level = (string) obj;

		d.TryGetValue("Name", out obj);
		Name = (string) obj;

		d.TryGetValue("NextID", out obj);
		NextID = (string) obj;

		d.TryGetValue("PrevID", out obj);
		PrevID = (string) obj;

		d.TryGetValue("Rarity", out obj);
		Rarity = (string) obj;

		d.TryGetValue("Set", out obj);
		Set = (string) obj;

		d.TryGetValue("Subtype", out obj);
		Subtype = (string) obj;

		d.TryGetValue("Text", out obj);
		Text = (string) obj;
	}
}
