
using System;
using System.Collections.Generic;

public class TurnBasedMatchData
{
	public string Kind { get; private set; }
	public bool DataAvailable { get; private set; }
	public TurnBasedMatchDataStructure Data { get; private set; }

	public TurnBasedMatchData (string kind, bool dataAvailable, TurnBasedMatchDataStructure data)
	{
		Kind = kind;
		DataAvailable = dataAvailable;
		Data = data;
	}
}
