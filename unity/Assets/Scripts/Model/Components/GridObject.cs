using System;
using UnityEngine;

//abstract class implemented by Tile class
public abstract class GridObject
{
	public int Layer { get; set; }
	public Point Location { get; set; }

	public GridObject(Point location, int layer)
	{
		Location = location;
		Layer = layer;
	}	
		
	public override string ToString()
	{
		return this.Location.ToString();
	}
}