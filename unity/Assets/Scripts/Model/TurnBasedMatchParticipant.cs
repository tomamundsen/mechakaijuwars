
using System;

public class TurnBasedMatchParticipant
{
	public string Kind { get; private set; }
	public string Id { get; private set; }
	public Player Player { get; private set; }
	public bool AutoMatched { get; private set; }
	public string Status { get; private set; }

	public TurnBasedMatchParticipant (string kind, string id, Player player, bool autoMatched, string status)
	{
		Kind = kind;
		Id = id;
		Player = player;
		AutoMatched = autoMatched;
		Status = status;
	}
}
