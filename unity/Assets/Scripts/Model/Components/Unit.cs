using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Unit : GridObject
{
	public bool IsSelected { get; set; }
	public string Name { get; private set; }
	public string Id { get; private set; }
    public int MatchId { get; private set; }
	public string Type { get; private set; }
	public string Subtype { get; private set; }
	public string LevelOneText { get; private set; }
	public string LevelTwoText { get; private set; }
	public string LevelThreeText { get; private set; }
	private readonly static int UNIT_LAYER = -10;

	public Unit(string name, string id, string type, string subtype, string levelOneText, string levelTwoText, string levelThreeText, Point location):
		base(location, UNIT_LAYER)
	{
		Name = name;
		Id = id;
		Type = type;
		Subtype = subtype;
		LevelOneText = levelOneText;
		LevelTwoText = levelTwoText;
		LevelThreeText = levelThreeText;
		IsSelected = false;
	}

    public Unit(int i, Dictionary<string, System.Object> d):
        base(new Point(1, 1), UNIT_LAYER)
    {
        Point location = new Point(int.MinValue, int.MinValue);
        foreach (KeyValuePair<string, System.Object> e in d)
        {
            Debug.Log("hello");
            if (e.Key == "Id")
            {
                // todo: make sure Id is Match Unit Id and not the CardLevelSheet Id
                MatchId = i;
                Id = (string)e.Value;
                CardLevelSheet s = GameManager.Instance.CardData[Id];
                Name = s.Name;
                Type = s.Type;
                Subtype = s.Subtype;
                LevelOneText = s.Text;
                LevelTwoText = s.Text;
                LevelThreeText = s.Text;
                IsSelected = false;
            }
            else if (e.Key == "Location")
            {
                int x = int.MinValue;
                int y = int.MinValue;

                Dictionary<string, System.Object> d2 = (Dictionary<string, System.Object>)e.Value;
                foreach (KeyValuePair<string, System.Object> e2 in d2)
                {
                    if (e2.Key == "x")
                    {
                        x = int.Parse((string)e2.Value);
                    }
                    else if (e2.Key == "y")
                    {
                        y = int.Parse((string)e2.Value);
                    }
                }

                location = new Point(x, y);
            }
        }

        base.Location = location;
    }
}
