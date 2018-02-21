using System;

public struct Point
{
	public readonly int X;
	public readonly int Y;

	public Point(int x, int y)
	{
		this.X = x;
		this.Y = y;
	}

	public override string ToString()
	{
		return string.Format("({0}, {1})", this.X, this.Y);
	}
}