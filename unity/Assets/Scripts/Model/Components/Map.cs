
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
	private Dictionary<Point, Tile> Tiles = new Dictionary<Point, Tile>();
	private readonly static int MAP_WIDTH = 100;
	private readonly static int MAP_HEIGHT = 100;
	private Path<Tile> path = null;

	public Tile SelectedTile = null;
	public Tile OriginTile = null;
	public bool DestinationSelected = false;

	public Map()
	{
		for (int y = 0; y < MAP_HEIGHT; y++)
		{
			for (int x = 0; x < MAP_WIDTH; x++)
			{
				Point p = new Point(x, y);
				Tiles[p] = new Tile(p);
			}
		}
		foreach(Tile tile in Tiles.Values)
		{
			tile.FindNeighbors(this);
		}
	}
	
	//change of coordinates when moving in any direction
	public static List<Point> NeighbourShift
	{
		get
		{
			return new List<Point>
			{
				new Point(0, 1),
				new Point(1, 0),
				new Point(0, -1),
				new Point(-1, 0),
			};
		}
	}

	// can get rid of this if we move Tile:FindNeighbors into here
	public bool IsValidTile(Point p)
	{
		return p.X >= 0 && p.X < MAP_WIDTH && p.Y >= 0 && p.Y < MAP_HEIGHT;
	}
	
	public Tile GetTileAt(Point p)
	{
		return Tiles[p];
	}
	
//	public void ClearPath()
//	{
//		foreach (Tile t in Tiles.Values)
//		{
//			t.IsOnPath = false;
//		}
//	}

	public Path<Tile> FindPath()
	{
		Func<Tile, Tile, double> distance = (node1, node2) => 1;
		Path<Tile> path = PathFinder.FindPath(OriginTile, SelectedTile, distance, calcDistance);

		foreach (Point p in Tiles.Keys)
		{
			Tiles[p].IsOnPath = false;
		}
		foreach (Tile t in path)
		{
			Tiles[t.Location].IsOnPath = true;
		}

		return path;
	}

	//Distance between destination tile and some other tile in the grid
	public double calcDistance(Tile tile)
	{
		if (SelectedTile == null)
		{
			Debug.Log("WARN!!! Map:calcDistance - SelectedTile null");
			return 0;
		}
		float deltaX = Mathf.Abs(SelectedTile.Location.X - tile.Location.X);
		float deltaY = Mathf.Abs(SelectedTile.Location.Y - tile.Location.Y);
		return Mathf.Max(deltaX, deltaY);
	}
	
	public void Reset()
	{
		OriginTile = null;
		foreach (Tile t in Tiles.Values)
		{
			t.IsOnPath = false;
		}
	}
}
