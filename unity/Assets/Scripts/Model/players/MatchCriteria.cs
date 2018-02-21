
using System;

public class MatchCriteria
{
	public string DeckId { get; private set; }
	public string Opponent { get; private set; }
	public string TimeFormat { get; private set; }

	public MatchCriteria (string deckId, string opponent, string timeFormat)
	{
		DeckId = deckId;
		Opponent = opponent;
		TimeFormat = timeFormat;
	}

	public override string ToString()
	{
		return string.Format("deckID: {0}, opponent: {1}, timeFormat: {2}", DeckId, Opponent, TimeFormat);
	}
}
