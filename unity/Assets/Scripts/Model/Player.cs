
using System;

public class Player
{
	public string Kind { get; private set; }
	public string ParticipantId { get; private set; }
	public string AvatarImageUrl { get; private set; }

	public Player (string kind, string participantId, string avatarImageUrl)
	{
		Kind = kind;
		ParticipantId = participantId;
		AvatarImageUrl = avatarImageUrl;
	}
}
