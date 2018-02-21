
using System;
using System.Collections.Generic;

public class Battlefield : Zone
{
    public Map Map { get; private set; }
    public List<Item> Items { get; private set; }
    public List<Unit> Units { get; private set; }

    public Battlefield(Dictionary<string, System.Object> d)
    {
        System.Object o;
        d.TryGetValue("map", out o);
        //List<Dictionary<string, System.Object>> map = o as List<Dictionary<string, System.Object>>;
        List<System.Object> map = o as List<System.Object>;
        //foreach (Dictionary<string, System.Object> dict in map)
        foreach (System.Object dict in map)
        {
            Dictionary<string, System.Object> dictDict = dict as Dictionary<string, System.Object>;
        }

        d.TryGetValue("items", out o);
        //List<Dictionary<string, System.Object>> items = o as List<Dictionary<string, System.Object>>;
        List<System.Object> items = o as List<System.Object>;
        foreach (System.Object dict in items)
        {
            //Dictionary<string, System.Object> dictDict = dict as Dictionary<string, System.Object>;
            //Items.Add(new Item(dictDict));
        }

        d.TryGetValue("units", out o);
        //List<Dictionary<string, System.Object>> units = o as List<Dictionary<string, System.Object>>;
        List<System.Object> units = o as List<System.Object>;
        foreach (System.Object dict in units)
        {
            //Dictionary<string, System.Object> dictDict = dict as Dictionary<string, System.Object>;
            //Units.Add(new Unit(dictDict));
        }

        //Map = new Map();
        //Items = new List<Item>();
        //Units = new List<Unit>();
    }
}
