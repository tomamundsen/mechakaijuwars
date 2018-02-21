
using System;
using System.Collections.Generic;

public class TurnBasedMatch
{
	public string Kind { get; private set; }
	public string MatchId { get; private set; }
	public string ApplicationId { get; private set; }
	public string Variant { get; private set; }
	public string Status { get; private set; }
	public string UserMatchStatus { get; private set; }
	public List<TurnBasedMatchParticipant> Participants { get; private set; }
	public TurnBasedMatchModification CreationDetails { get; private set; }
	public TurnBasedMatchModification LastUpdateDetails { get; private set; }
	public TurnBasedAutoMatchingCriteria AutoMatchingCriteria { get; private set; }
	public TurnBasedMatchData Data { get; private set; }
	public ParticipantResult Results { get; private set; }
	public string InviterId { get; private set; }
	public string WithParticipantId { get; private set; }
	public string Description { get; private set; }
	public string PendingParticipantId { get; private set; }
	public string MatchVersion { get; private set; }
	public string RematchId { get; private set; }
	public string MatchNumber { get; private set; }
	public TurnBasedMatchData PreviousMatchData { get; private set; }

	public TurnBasedMatch (string kind, string matchId, string applicationId, string variant, string status,
	                       string userMatchStatus, List<TurnBasedMatchParticipant> participants,
	                       TurnBasedMatchModification creationDetails, TurnBasedMatchModification lastUpdateDetails,
	                       TurnBasedAutoMatchingCriteria autoMatchingCriteria, TurnBasedMatchData data, ParticipantResult results,
	                       string inviterId, string withParticipantId, string description, string pendingParticipantId, string matchVersion,
	                       string rematchId, string matchNumber, TurnBasedMatchData previousMatchData)
	{
		Kind = kind;
		MatchId = matchId;
		ApplicationId = applicationId;
		Variant = variant;
		Status = status;
		UserMatchStatus = userMatchStatus;
		Participants = participants;
		CreationDetails = creationDetails;
		LastUpdateDetails = lastUpdateDetails;
		AutoMatchingCriteria = autoMatchingCriteria;
		Data = data;
		Results = results;
		InviterId = inviterId;
		WithParticipantId = withParticipantId;
		Description = description;
		PendingParticipantId = pendingParticipantId;
		MatchVersion = matchVersion;
		RematchId = rematchId;
		MatchNumber = matchNumber;
		PreviousMatchData = previousMatchData;
	}
}
