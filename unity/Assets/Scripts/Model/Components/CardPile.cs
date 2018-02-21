
using System;
using UnityEngine;
using System.Collections.Generic;

public class CardPile
{
	private Stack<CardLevelSheet> cards;

	public CardPile()
	{
		cards = new Stack<CardLevelSheet>();
	}
	
	public CardPile (Stack<CardLevelSheet> l)
	{
		cards = l;
	}

	public CardPile (string d)
	{
		cards = new Stack<CardLevelSheet>();

		try
		{
			Deck deck;
			d = "Decks_1x94Ww7sAsh4S79qsU-xkFg7O01F3mZohTsfMNzIaeiw"; // todo: use deckId for given match chosen by player
			GameManager.Instance.DeckData.TryGetValue(d, out deck);

			foreach (string c in deck.Data.Keys)
			{
				for (int i = 0; i < deck.Data[c]; i++)
				{
					cards.Push(new CardLevelSheet(GameManager.Instance.CardData[c]));
				}
			}
		}
		catch
		{
			Debug.Log("!!! Warn - " + d + " deck couldn't be found");
		}
	}

	public void Shuffle()
	{
		if (cards.Count > 0)
		{
			Stack<CardLevelSheet> shuffledCards = new Stack<CardLevelSheet>();
			List<CardLevelSheet> cardsList = new List<CardLevelSheet>();
			foreach (CardLevelSheet c in this.cards)
			{
				cardsList.Add(c);
			}

			while (cardsList.Count > 0)
			{
				int i = UnityEngine.Random.Range(0, cardsList.Count);
				shuffledCards.Push(new CardLevelSheet(cardsList[i]));
				cardsList.Remove(cardsList[i]);
			}	
			this.cards = shuffledCards;
		}
		else
		{
			Debug.Log("!!! Warn - CardPile:Shuffle() - no cards to shuffle");
		}

	}

	public CardLevelSheet DrawFromTop()
	{
		return cards.Pop();
	}

	public CardLevelSheet DrawFromBottom()
	{
		// todo (need data structure other than Stack?)
		return new CardLevelSheet(new Dictionary<string, System.Object>());
	}

	public void PutOnTop(CardLevelSheet c)
	{
		cards.Push(c);
	}

	public void PutOnBottom(CardLevelSheet c)
	{
		// todo
	}

	public CardLevelSheet Get(int i)
	{
		return cards.ToArray()[i];
	}

}
