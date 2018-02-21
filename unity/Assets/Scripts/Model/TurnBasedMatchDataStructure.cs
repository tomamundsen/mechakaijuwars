
using System;
using UnityEngine;
using System.Collections.Generic;

public class TurnBasedMatchDataStructure
{
	private Dictionary<string, System.Object> dictionary;

	public TurnBasedMatchDataStructure (Dictionary<string, System.Object> d)
	{
		dictionary = d;
	}

	public Dictionary<string, System.Object> GetParent(string p)
	{
		Dictionary<string, System.Object> dict;
		if (p != null && p != "")
		{
			System.Object o;
			dictionary.TryGetValue(p, out o);
			return o as Dictionary<string, System.Object>;
		}
		else
		{
			throw new Exception();
			return null;
		}
	}

	public int GetInt(string k, string parent = null)
	{
		Dictionary<string, System.Object> root;

		if (parent != null && parent != "")
		{
			root = GetParent(parent);
		}
		else
		{
			root = dictionary;
		}

		System.Object o;
		root.TryGetValue(k, out o);
		if (o != null)
		{
			return int.Parse((string) o);
		}
		else
		{
			Debug.Log("WARN!!! TurnBasedMatchDataStructure:GetInt null");
			throw new Exception(); // TODO: create a new exception type
		}
	}

	public Point GetPoint(string k, string parent = null)
	{
		Dictionary<string, System.Object> root;
		
		if (parent != null && parent != "")
		{
			root = GetParent(parent);
		}
		else
		{
			root = dictionary;
		}

		System.Object o;
		root.TryGetValue(k, out o);
		Dictionary<string, System.Object> d;
		d = (Dictionary<string, System.Object>) o;
		if (o != null)
		{
			System.Object oo;
			d.TryGetValue("x", out oo);
			int x;
			if (oo != null)
			{
				x = int.Parse((string) oo);
			}
			else
			{
				Debug.Log("WARN!!! TurnBasedMatchDataStructure:GetPoint null");
				x = 0;
			}

			System.Object ooo;
			d.TryGetValue("y", out ooo);
			int y;
			if (ooo != null)
			{
				y = int.Parse((string) ooo);
			}
			else
			{
				Debug.Log("WARN!!! TurnBasedMatchDataStructure:GetPoint null");
				y = 0;
			}

			return new Point(x, y);
		}
		else
		{
			Debug.Log("WARN!!! TurnBasedMatchDataStructure:GetPoint null");
			throw new Exception(); // TODO: create a new exception type
		}
	}

	public CardPile GetCardPile(string k, string parent = null)
	{
		Dictionary<string, System.Object> root;

		if (parent != null && parent != "")
		{
			root = GetParent(parent);
		}
		else
		{
			root = dictionary;
		}

		Stack<CardLevelSheet> sheets = new Stack<CardLevelSheet>();

		System.Object o;
		root.TryGetValue(k, out o);
		if (o != null)
		{
			Debug.Log("found it");
			List<System.Object> l = (List<System.Object>) o;
			foreach (System.Object s in l)
			{
				sheets.Push(GameManager.Instance.CardData[(string)s]);
			}
		}
		else
		{
			Debug.Log("!!! Warn - no CardPile found: " + k);
		}

		return new CardPile(sheets);
	}

    public List<Unit> GetUnits(string s, string parent = null)
    {
        Dictionary<string, System.Object> root;

        if (parent != null && parent != "")
        {
            root = GetParent(parent);
        }
        else
        {
            root = dictionary;
        }

        List<Unit> units = new List<Unit>();

        System.Object o;
        root.TryGetValue(s, out o);
        if (o != null)
        {
            Debug.Log("found it");
            List<System.Object> l = (List<System.Object>)o;
            int i = 0;
            foreach (System.Object u in l)
            {
                units.Add(new Unit(i, u as Dictionary<string, System.Object>));
                i++;
            }
        }
        else
        {
            Debug.Log("!!! Warn - no units found: " + s);
        }

        return units;
    }
}
