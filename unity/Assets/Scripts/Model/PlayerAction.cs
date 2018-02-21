
using System;
using System.Collections.Generic;

abstract public class PlayerAction
{
    public Guid Id { get; private set; }

    public PlayerAction()
    {
        Id = Guid.NewGuid();
    }

    abstract public Dictionary<string, System.Object> GetActionDictionary();
}
