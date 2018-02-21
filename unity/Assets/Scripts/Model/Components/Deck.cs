
using System;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
	public Dictionary<string, int> Data { get; private set; }
	public string Name { get; private set; }

	public Deck(string name, List<System.Object> dict)
	{
		Name = name;
		Data = new Dictionary<string, int>();
		foreach (System.Object o in dict)
		{
			try
			{
				Dictionary<string, System.Object> d = (Dictionary<string, System.Object>) o;

				System.Object obj;

				d.TryGetValue("ID", out obj);
				string id = (string) obj;

				d.TryGetValue("Quantity", out obj);
				string quantity = (string) obj;
				quantity = quantity;
				int quant;
				int.TryParse(quantity, out quant);
				Data.Add(id, quant);
			}
			catch (Exception e)
			{
				Debug.Log(e);
			}
		}
	}
}
