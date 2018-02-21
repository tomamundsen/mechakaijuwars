
using UnityEngine;
using System.Collections;
using System.Collections.Generic;   
using UnityEngine.UI;
using SimpleJSON;

public class ChangeTile : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject Tile;
    public Canvas Canvas;
    public GameObject Panel;
    public Camera Camera;
	
	void Start ()
    {
        string path = "map";
        TextAsset txt = (TextAsset)Resources.Load(path, typeof(TextAsset));
        string content = txt.text;
        List<System.Object> list = MiniJSON.Json.Deserialize(JSON.Parse(content).ToString()) as List<System.Object>;
        Dictionary<Point, NewTile> tiles = new Dictionary<Point, NewTile>();

        foreach (System.Object o in list)
        {
            Dictionary<string, System.Object> dict = o as Dictionary<string, System.Object>;
            System.Object obj;
            dict.TryGetValue("type", out obj);
            int type = int.Parse(obj as string);

            dict.TryGetValue("location", out obj);
            Dictionary<string, System.Object> ldict = obj as Dictionary<string, System.Object>;
            ldict.TryGetValue("x", out obj);
            int x = int.Parse(obj as string);
            ldict.TryGetValue("y", out obj);
            int y = int.Parse(obj as string);

            tiles.Add(new Point(x, y), new NewTile(type - 1)); // todo: why is type off by one???
        }

        sprites = Resources.LoadAll<Sprite>("grass-tiles-2-small");

        foreach (KeyValuePair<Point, NewTile> p in tiles)
        {
            Point point = p.Key;
            NewTile tile = p.Value;
            
            GameObject o = GameObject.Instantiate(Tile);
            //o.transform.SetParent(Canvas.gameObject.transform, false);
            o.transform.SetParent(Panel.transform, false);
            o.GetComponent<Image>().sprite = sprites[tile.Type];
            Image i = o.GetComponent<Image>();

            int SCREEN_WIDTH = Camera.pixelWidth;
            int SCREEN_HEIGHT = Camera.pixelHeight;
            float x_coord = point.X * (SCREEN_WIDTH / 10.0f);
            //float y_coord = point.Y * (SCREEN_HEIGHT / 10);
            float y_coord = point.Y * (SCREEN_WIDTH / 10.0f);
            y_coord = SCREEN_HEIGHT - y_coord;
            //y_coord -= SCREEN_HEIGHT / 2;
            y_coord -= SCREEN_WIDTH / 2;
            x_coord -= SCREEN_WIDTH / 2;
            x_coord += 32.0f;
            y_coord -= 32.0f;
            Debug.Log(new Vector3(((32.0f * 10.0f) / SCREEN_WIDTH), ((32.0f * 10.0f) / SCREEN_HEIGHT), 1.0f));
            float scale = ((32.0f * 10.0f) / SCREEN_WIDTH) + 0.08f;
            //i.rectTransform.localScale = new Vector3(((32.0f * 10.0f) / SCREEN_WIDTH), ((32.0f * 10.0f) / SCREEN_WIDTH), 1.0f);
            i.rectTransform.localScale = new Vector3(scale, scale, 1.0f);
            i.rectTransform.anchoredPosition = new Vector2(x_coord - (1.0f / (SCREEN_WIDTH / (32.0f * 10.0f * 2.0f))), y_coord - (1.0f / (SCREEN_HEIGHT / (32.0f * 10.0f * 2.0f))));
            //Vector3 v = camera.ScreenToWorldPoint(new Vector3(point.X * (SCREEN_WIDTH / 10), point.Y * (SCREEN_HEIGHT / 10), 0));
            //i.transform.position = v;
            //o.transform.position = v;
        }
        
    }
	
	void Update () {
    }
    

    private Vector3 Delta;
    public void BeginDrag()
    {
        Debug.Log("ChangeTile:BeginDrag");
        Delta = Input.mousePosition - Panel.transform.position;
    }

    public void EndDrag()
    {
        Debug.Log("ChangeTile:EndDrag");
    }

    public void Drag()
    {
        Debug.Log("ChangeTile:Drag");
        Panel.transform.position = Input.mousePosition - Delta;
    }
}
