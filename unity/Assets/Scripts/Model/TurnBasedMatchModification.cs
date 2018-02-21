
using System;

public class TurnBasedMatchModification
{
	public string Kind { get; private set; }
	public string ParticipantId { get; private set; }
	public long ModifiedTimestampMillis { get; private set; }

	public TurnBasedMatchModification (string kind, string participantId, long modifiedTimestampMillis)
	{
		Kind = kind;
		ParticipantId = participantId;
		ModifiedTimestampMillis = modifiedTimestampMillis;
	}
}
