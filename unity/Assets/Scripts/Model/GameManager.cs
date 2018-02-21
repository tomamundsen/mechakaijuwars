
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	private static GameManager mInstance = null;
	private MatchCriteria MatchCriteria = null;
	public MatchCriteria QuickMatchCriteria { get; set; }
	public string CurrentDeckId { get; set; }
	public string CurrentOpponentType { get; set; }
	public string CurrentTimeFormat { get; set; }
	public string CurrentPlayerId { get; set; }
	public string CurrentMatchId { get; set; }
	public string CurrentMatchPlayerId { get; set; }
	public string CurrentPendingParticipantId { get; set; }
	public string CurrentMatchStatus { get; set; }
	public int CurrentMatchTurnNumber { get; set; }
	public string CurrentMatchOpponentPlayerId { get; set; }
	public Dictionary<string, CardLevelSheet> CardData { get; set; }
	public Dictionary<string, Card> Cards { get; set; }
	public Dictionary<string, Deck> DeckData { get; set; }
	public CardPile Hand { get; set; }
	public CardPile DiscardPile { get; set; }
	public CardPile DrawPile { get; set; }
    public List<Unit> Units { get; set;  }

	public virtual void Awake ()
	{
		DontDestroyOnLoad (this.gameObject);
		if (mInstance == null) {
			mInstance = this;
		} else {
			Destroy (gameObject);
		}
	}

	public static GameManager Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = (new GameObject("GameManagerContainer")).AddComponent<GameManager>();
			}
			return mInstance;
		}
	}

	void Start ()
	{
	
	}

	void Update ()
	{
	
	}

	public MatchCriteria GetMatchCriteria()
	{
		return MatchCriteria;
	}
}
