using System;
using System.Collections.Generic;
using System.Linq;

public static class PathFinder
{
	//distance f-ion should return distance between two adjacent nodes
	//estimate should return distance between any node and destination node
	static public Path<Tile> FindPath<Tile>(
		Tile start, 
		Tile destination, 
		Func<Tile, Tile, double> distance, 
		Func<Tile, double> estimate)
		where Tile : GridObject, IHasNeighbors<Tile>
	{
		var closed = new HashSet<Tile>();
		var queue = new PriorityQueue<double, Path<Tile>>();
		queue.Enqueue(0, new Path<Tile>(start));
		while (!queue.IsEmpty)
		{
			var path = queue.Dequeue();
			if (closed.Contains(path.LastStep))
				continue;
			if ((path.LastStep.Location.X == destination.Location.X) && (path.LastStep.Location.Y == destination.Location.Y))
				return path;
			closed.Add(path.LastStep);
			foreach(Tile n in path.LastStep.Neighbors)
			{
				double d = distance(path.LastStep, n);
				var newPath = path.AddStep(n, d);
				queue.Enqueue(newPath.TotalCost + estimate(n), newPath);
			}
		}
		return null;
	}
}