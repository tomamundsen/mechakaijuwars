using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LargeCardView : MonoBehaviour {

	public Text Name;
	public Text Type;
	public Text Subtype;
	public Text Text;
	public Text Cost;
	public Text Level;
	public string Id;
	public Image Image;
	public Image RarityImage;
	public Image FactionImage;

	public delegate void SyncMouseDelegate(LargeCardView v);
	public SyncMouseDelegate PreviousDel;
	public SyncMouseDelegate NextDel;
	public SyncMouseDelegate BackDel;

	public void SetName(string s)
	{
		Name.text = s;
	}

	public void SetType(string s)
	{
		Type.text = s;
	}

	public void SetSubtype(string s)
	{
		Subtype.text = s;
	}

	public void SetText(string s)
	{
		Text.text = s;
	}

	public void SetCost(string s)
	{
		Cost.text = s;
	}

	public void SetLevel(string s)
	{
		Level.text = s;
	}

	// todo: refactor into shared with CardView
	private Sprite GetSprite(Rarity r)
	{
		switch (r)
		{
		case Rarity.Common:
			return Resources.Load <Sprite> ("Icon/Rarity/Common");
			break;
		case Rarity.Uncommon:
			return Resources.Load <Sprite> ("Icon/Rarity/Uncommon");
			break;
		case Rarity.Rare:
			return Resources.Load <Sprite> ("Icon/Rarity/Rare");
			break;
		case Rarity.MegaRare:
			return Resources.Load <Sprite> ("Icon/Rarity/MegaRare");
			break;
		default:
			Debug.Log("!!! Error - no rarity sprite found for rarity: " + r.ToString());
			return null;
			break;
		}
	}

	// todo: refactor into shared with CardView
	private Sprite GetSprite(Faction f)
	{
		switch (f)
		{
		case Faction.Offensive:
			return Resources.Load <Sprite> ("Icon/Faction/Offensive");
			break;
		case Faction.Death:
			return Resources.Load <Sprite> ("Icon/Faction/Death");
			break;
		case Faction.Defensive:
			return Resources.Load <Sprite> ("Icon/Faction/Defensive");
			break;
		case Faction.Growth:
			return Resources.Load <Sprite> ("Icon/Faction/Growth");
			break;
		case Faction.Technology:
			return Resources.Load <Sprite> ("Icon/Faction/Technology");
			break;
		default:
			Debug.Log("!!! Error - no faction sprite found for faction: " + f.ToString());
			return null;
			break;
		}
	}

	// todo: refactor into shared with CardView
	public void SetRarity(string s)
	{
		Rarity r = Rarity.Unknown;
		switch (s)
		{
		case "Common":
			r = Rarity.Common;
			break;
		case "Uncommon":
			r = Rarity.Uncommon;
			break;
		case "Rare":
			r = Rarity.Rare;
			break;
		case "MegaRare":
			r = Rarity.MegaRare;
			break;
		default:
			Debug.Log("!!! Error couldn't find rarity: " + s);
			break;
		}
		RarityImage.sprite = GetSprite(r);
	}

	// todo: refactor into shared with CardView
	public void SetFaction(string s)
	{
		Faction f = Faction.Unknown;
		switch (s)
		{
		case "Offensive":
			f = Faction.Offensive;
			break;
		case "Death":
			f = Faction.Death;
			break;
		case "Defensive":
			f = Faction.Defensive;
			break;
		case "Growth":
			f = Faction.Growth;
			break;
		case "Technology":
			f = Faction.Technology;
			break;
		default:
			Debug.Log("!!! Error unknown faction: " + s);
			break;
		}
		
		FactionImage.sprite = GetSprite(f);
	}

	public void Previous()
	{
		PreviousDel(this);
	}

	public void Next()
	{
		NextDel(this);
	}

	public void Back()
	{
		Debug.Log("LargeUnitView:Back");
		BackDel(this);
	}
}
