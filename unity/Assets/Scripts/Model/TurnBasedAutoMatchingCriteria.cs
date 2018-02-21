
using System;

public class TurnBasedAutoMatchingCriteria
{
	public string Kind { get; private set; }
	public int MinAutoMatchingPlayers { get; private set; }
	public int MaxAutoMatchingPlayers { get; private set; }
	public int ExclusiveBitmask { get; private set; }

	public TurnBasedAutoMatchingCriteria (string kind, int minAutoMatchingPlayers, int maxAutoMatchingPlayers, int exclusiveBitmask)
	{
		Kind = kind;
		MinAutoMatchingPlayers = minAutoMatchingPlayers;
		MaxAutoMatchingPlayers = maxAutoMatchingPlayers;
		ExclusiveBitmask = exclusiveBitmask;
	}
}
