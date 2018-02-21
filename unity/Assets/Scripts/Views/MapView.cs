using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MapView : MonoBehaviour 
{
	private Dictionary<Point, TileView> TileViews = new Dictionary<Point, TileView>();
	private float groundWidth;
	private float groundHeight;
	private static readonly int MAP_WIDTH = 100;
	private static readonly int MAP_HEIGHT = 100;

	public tk2dTileMap Tilemap;
	public GameObject Tile;
	public TileView SelectedTileView { get; set; }
	public TileView OriginTileView { get; set; }
	public List<TileView> PathViews;
	public TileView.SyncMouseDelegate TileViewEnter;
	public TileView.SyncMouseDelegate TileViewExit;
	public TileView.AsyncMouseDelegate TileViewOver;

	void Awake()
	{
		Tilemap = (tk2dTileMap)gameObject.GetComponent("tk2dTileMap");
	}

	private Vector3 GetPosition(Point p)
	{
		Vector3 result = Tilemap.GetTilePosition(p.X, p.Y);
		return result;
	}

	public void Initialize()
	{
		Tilemap = (tk2dTileMap) gameObject.GetComponent("tk2dTileMap");

		for (int x = 0; x < MAP_WIDTH; x++)
		{
			for (int y = 0; y < MAP_HEIGHT; y++)
			{
				Point p = new Point(x, y);
				GameObject t = (GameObject)Instantiate(Tile);
				TileView tileView = (TileView)t.GetComponent("TileView");
				tileView.Location = p;
				tileView.MouseEnter = TileViewEnter;
				tileView.MouseExit = TileViewExit;
				tileView.MouseOver = TileViewOver;
				TileViews[p] = tileView;

				float width = TileViews[new Point(0,0)].GetWidth();
				float height = TileViews[new Point(0,0)].GetHeight();
				Vector3 position = GetPosition(new Point(x, y));
				position.x += width/2;
				position.y += height/2;
				position.z = -10;
				t.transform.position = position;
			}
		}
	}

	public float GetTileWidth()
	{
		return Tile.GetComponent<Renderer>().bounds.center.x;
	}

	public float GetTileHeight()
	{
		return Tile.GetComponent<Renderer>().bounds.center.y;
	}

	public TileView GetTileViewAt(Point p)
	{
		return TileViews[p];
	}
	
//	public Point GetTilePosition(Vector3 v)
//	{
//		int x;
//		int y;
//		tilemap.GetTileAtPosition(v, out x, out y);
//		return new Point(x, y);
//	}
//
//	public Vector3 GetTilePosition(Point p)
//	{
//		Vector3 result = tilemap.GetTilePosition(p.X, p.Y);
//		return result;
//	}

	public void DrawSelectionPath()
	{
		DrawPath (PathViews, Color.white);
		SelectedTileView.ChangeColor(Color.yellow);
	}

	public void DrawSelectedPath()
	{
		DrawPath (PathViews, Color.grey);
		SelectedTileView.ChangeColor(Color.red);
	}
	
	private void DrawPath(List<TileView> path, Color c)
	{
		Clear ();
		foreach (TileView tv in path)
		{
			tv.ChangeColor(c);
		}
		if (path == null)
		{
			Debug.Log("path is null");
		}
	}

	public void Clear()
	{
		foreach (TileView tv in TileViews.Values)
		{
			tv.Clear();
		}
	}
	
//	public static void ClearPath(List<TileView> path)
//	{
//		foreach (TileView tv in path)
//		{
//			tv.Clear();
//		}
//	}

	public void SetPathViews(List<TileView> pathViews)
	{
		PathViews = pathViews;
	}
}
