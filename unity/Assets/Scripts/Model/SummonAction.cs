
using System;
using System.Collections.Generic;

public class SummonAction : PlayerAction
{
    private CardLevelSheet LevelSheet;
    private Point Location;

    public SummonAction(CardLevelSheet s, Point p)
        : base()
    {
        LevelSheet = s;
        Location = p;
    }

    override public Dictionary<string, System.Object> GetActionDictionary()
    {
        Dictionary<string, System.Object> d = new Dictionary<string, System.Object>();
        d.Add("Type", "Summon");
        d.Add("Id", LevelSheet.ID);
        Dictionary<string, System.Object> l = new Dictionary<string, System.Object>();
        l.Add("X", Location.X);
        l.Add("Y", Location.Y);
        d.Add("Location", l);
        return d;
    }
}
