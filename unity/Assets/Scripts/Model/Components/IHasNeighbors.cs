//using System.Collections;
using System.Collections.Generic;


//interface that should be implemented by grid nodes used in E. Lippert's generic path finding implementation
public interface IHasNeighbors<N>
{
	IEnumerable<N> Neighbors { get; }
}
