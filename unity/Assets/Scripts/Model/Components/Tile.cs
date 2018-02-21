
using System;
using System.Collections.Generic;

public class Tile : GridObject, IHasNeighbors<Tile>
{
	private readonly static int TILE_LAYER = 10;

	public bool Traversable;
	public bool IsOnPath { get; set; }

	public Tile (Point location) :
		base(location, TILE_LAYER)
	{
		Traversable = true;
		IsOnPath = false;
	}

	public IEnumerable<Tile> AllNeighbors { get; set; }
	
	public IEnumerable<Tile> Neighbors
	{
		get 
		{
			List<Tile> passableNeighbors = new List<Tile>();
			foreach (Tile tile in AllNeighbors) {
				if (tile.Traversable) {
					passableNeighbors.Add (tile);
				}
			}
			return passableNeighbors;
		}
	}

	// TODO: put this in Map
	public void FindNeighbors(Map map)
	{	
		List<Tile> neighbors = new List<Tile>();
		
		foreach (Point point in Map.NeighbourShift)
		{
			int neighborX = Location.X + point.X;
			int neighborY = Location.Y + point.Y;
			
			if (map.IsValidTile(new Point(neighborX, neighborY)))
			{
				neighbors.Add(map.GetTileAt(new Point(neighborX, neighborY)));
			}
		}
		AllNeighbors = neighbors;
	}
}
