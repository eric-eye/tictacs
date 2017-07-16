using UnityEngine;
using GridFramework.Grids;
using GridFramework.Renderers.Rectangular;

namespace GridFramework.Examples.Movement {
	/// <summary>
	///   Keep track of the game map's state.
	/// </summary>
	/// <remarks>
	///   <para>
	///     This class has three purposes: it stores a two-dimensional array of
	///     bool values (I will call this array the matrix from now on and its
	///     entries tiles), it sets and returns the value of tiles based on
	///     which world positions they correspond to.  Each tile represents a
	///     face of a rectangular grid on the XY plane and its position in the
	///     matrix mirrors its position in the grid with (0,0) being the lower
	///     left face, the first index denotes the row and the second index the
	///     column of a face in the grid.
	///   </para>
	///   <para>
	///     This class works together with two other scripts, ModelGrid and
	///     BlockingTile.  The matrix is built first by ModelGrid in the
	///     Awake() function to make sure it is built before any Start()
	///     function gets called. It just calls the matrix-building method and
	///     passes which grid to use for reference. Then each obstacle fires
	///     the Start() method from its BlockingTile script to set its tile as
	///     forbidden. The player's movement script makes use of the CheckTile
	///     method to find out if a face is forbidden or not, but it doesn't
	///     write anything to the matrix.
	///   </para>
	///   <para>
	///     This script demonstrates how you can use Grid Framework to store
	///     information about individual tiles apart from the objects they
	///     belong to in a format accessible to all objects in the scene.  The
	///     information does not have to be limited to just boolean values, you
	///     can extend this approach to store any other information you might
	///     have and build more complex game machanics around it.
	///   </para>
	/// </remarks>
	public static class ForbiddenTiles {
#region  Public variables
		/// <summary>
		///   Two-dimensional array of bool values, represents the state of the
		///   map. <c>True</c> means the tile is legal to step on.
		/// </summary>
		public static bool[,] _tiles;

		/// <summary>
		///   The grid everything is based on.
		/// </summary>
		public static RectGrid _grid;
#endregion  // Public variables
		
#region  Public methods
		/// <summary>
		///   Builds the matrix and sets everything up, gets called by a script
		///   attached to the grid object.
		/// </summary>
		/// <param name="grid">
		///   The grid will be assigned to this class's <c>_grid</c>.
		/// </param>
		/// <param name="renderer">
		///   The grid's parallelepiped renderer is used for determining the
		///   size of the matrix.
		/// </param>
		public static void Initialize(RectGrid grid, Parallelepiped renderer) {
			_grid = grid;

			// Amount of rows and columns based on the rendering range (assumes
			// the From is the zero-vector).
			var rows    = Mathf.FloorToInt(renderer.To.x);
			var columns = Mathf.FloorToInt(renderer.To.y);

			_tiles = new bool[rows, columns];

			for (var row = 0; row < rows; ++row) {
				for (var column = 0; column < columns; ++column) {
					_tiles[row, column] = true;
				}
			}
		}

		/// <summary>
		///   Takes world coordinates, finds the corresponding tile and sets
		///   that entry to either true or false. Use it to disable or enable
		///   tile.
		/// </summary>
		/// <param name="tile">
		///   Position of the tile in world-coordinates.
		/// </param>
		/// <param name="status">
		///   Initial value of the tile.
		/// </param>
		public static void SetTile(Vector3 tile, bool status) {
			// We need the tile in grid-coordinates
			tile = _grid.WorldToGrid(tile);
			_tiles[Mathf.FloorToInt(tile.x), Mathf.FloorToInt(tile.y)] = status;
		}
		
		/// <summary>
		///   Takes world coordinates, finds the corresponding tile and returns
		///   the value of that tile. Use it to check if a tile is forbidden or
		///   not.
		/// </summary>
		/// <param name="tile">
		///   Position of the tile in grid-coordinates.
		/// </param>
		public static bool CheckTile(Vector3 tile) {
			return _tiles[Mathf.FloorToInt(tile.x), Mathf.FloorToInt(tile.y)];
		}
#endregion  // Public methods
	}
}
