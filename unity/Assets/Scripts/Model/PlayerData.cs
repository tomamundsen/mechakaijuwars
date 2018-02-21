
using System;
using System.Collections.Generic;

public class PlayerData
{
    public string PlayerId { get; private set; }
    public int Mana { get; private set; }
    public CardPile DrawPile { get; private set; }
    public CardPile Hand { get; private set; }
    public CardPile DiscardPile { get; private set; }

    public PlayerData(Dictionary<string, System.Object> d)
    {
        System.Object o;
        d.TryGetValue("player_id", out o);
        PlayerId = o as string;

        d.TryGetValue("mana", out o);
        Mana = int.Parse(o as string);

        d.TryGetValue("draw_pile", out o);
        List<System.Object> drawPileList = o as List<System.Object>;
        List<string> drawPileListList = new List<string>();
        foreach (System.Object so in drawPileList)
        {
            drawPileListList.Add(so as string);
        }
        DrawPile = new CardPile(drawPileListList);

        d.TryGetValue("hand", out o);
        List<System.Object> handList = o as List<System.Object>;
        List<string> handListList = new List<string>();
        foreach (System.Object so in handList)
        {
            handListList.Add(so as string);
        }

        Hand = new CardPile(handListList);

        d.TryGetValue("discard_pile", out o);
        List<System.Object> discardPile = o as List<System.Object>;
        List<string> discardPileList = o as List<string>;
        foreach (System.Object so in discardPile)
        {
            discardPileList.Add(so as string);
        }
        DiscardPile = new CardPile(discardPileList);
    }
}
