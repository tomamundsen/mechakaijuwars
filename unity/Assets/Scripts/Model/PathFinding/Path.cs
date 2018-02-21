using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Path<Tile> : List<Tile>, IEnumerable<Tile>
{
	private int Size;

	public Tile LastStep { get; private set; }
	public Path<Tile> PreviousSteps { get; private set; }
	public double TotalCost { get; private set; }

	private Path(Tile lastStep, Path<Tile> previousSteps, double totalCost, int size)
	{
		LastStep = lastStep;
		PreviousSteps = previousSteps;
		TotalCost = totalCost;
		this.Size = size;
	}

	public Path(Tile start) : this(start, null, 0, 1) {}

	public List<Tile> GetList()
	{
		List<Tile> result = new List<Tile>();
		Path<Tile> step = this;

		while (step.PreviousSteps != null)
		{
			result.Add(step.LastStep);
			step = step.PreviousSteps;
		}
		return result;
	}

	public int GetSize()
	{
		return this.Size;
	}

	public Path<Tile> AddStep(Tile step, double stepCost)
	{
		return new Path<Tile>(step, this, TotalCost + stepCost, this.Size + 1);
	}

	public IEnumerator<Tile> GetEnumerator()
	{
		for (Path<Tile> p = this; p != null; p = p.PreviousSteps)
			yield return p.LastStep;
	}
}