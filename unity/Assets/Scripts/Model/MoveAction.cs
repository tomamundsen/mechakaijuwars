
using System;
using System.Collections.Generic;

public class MoveAction : PlayerAction
{
    public MoveAction(CardLevelSheet s, Point p)
        : base()
    {

    }

    override public Dictionary<string, System.Object> GetActionDictionary()
    {
        Dictionary<string, System.Object> d = new Dictionary<string, System.Object>();
        return d;
    }
}
