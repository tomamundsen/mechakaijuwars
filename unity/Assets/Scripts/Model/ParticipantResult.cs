
using System;

public class ParticipantResult
{
	public string Kind { get; private set; }
	public string ParticipantId { get; private set; }
	public string Result { get; private set; }
	public string Placing { get; private set; }

	public ParticipantResult (string kind, string participantId, string result, string placing)
	{
		Kind = kind;
		ParticipantId = participantId;
		Result = result;
		Placing = placing;
	}
}
