
using System;
using System.Collections.Generic;

public class Command : Zone
{
    private List<MKWObject> objects;

    public Command()
    {
        objects = new List<MKWObject>();
    }

    public Command(List<string> l)
    {
        foreach (string s in l)
        {
            // todo: s can be PermanentMatchId, CardMatchId, TokenMatchId, SpellMatchId, EmblemMatchId
            if (GameManager.Instance.Cards.ContainsKey(s))
            {
                objects.Add(new Card(GameManager.Instance.Cards[s]));
            }
            else if (GameManager.Instance.Permanents.ContainsKey(s))
            {
                objects.Add(new Permanent(GameManager.Instance.Permanents[s]));
            }
            else if (GameManager.Instance.Tokens.ContainsKey(s))
            {
                objects.Add(new Token(GameManager.Instance.Tokens[s]));
            }
            else if (GameManager.Instance.Spells.ContainsKey(s))
            {
                objects.Add(new Spell(GameManager.Instance.Spells[s]));
            }
        }
    }
}
