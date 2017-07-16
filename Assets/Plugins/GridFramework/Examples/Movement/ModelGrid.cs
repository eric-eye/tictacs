using UnityEngine;
using GridFramework.Grids;
using GridFramework.Renderers.Rectangular;

namespace GridFramework.Examples.Movement {
	/// <summary>
	///   Set up the initial state of the game's model based on the attached
	///   grid.
	/// </summary>
	/// <remarks>
	///   <para>
	///     We will build the matrix based on the grid that is attached to this
	///     object.  All entries are true by default, then each obstacle will
	///     mark its entry as false.
	///   </para>
	/// </remarks>
	[RequireComponent(typeof (RectGrid))]
	[RequireComponent(typeof (Parallelepiped))]
	public class ModelGrid : MonoBehaviour {
	
		// Awake is called before Start()
		void Awake() {
			var grid = GetComponent<RectGrid>();
			var para = GetComponent<Parallelepiped>();

			ForbiddenTiles.Initialize(grid, para);
		}
	}
}
