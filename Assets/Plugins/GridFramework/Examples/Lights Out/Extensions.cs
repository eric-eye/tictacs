using UnityEngine;
using GridFramework.Grids;

// This script creates an extension method for a Grid class, which means it
// adds a new method to the class without the need to alter the source code of
// the class. Once declared you can use the method just like any normal class
// method.

namespace GridFramework.Examples.LightsOut {
	/// <summary>
	///   Extension methods for the Lights Out example.
	/// </summary>
	public static class Extensions {
		/// <summary>
		///   Whether two points are adjacent in the grid. Assumes whole
		///   numbers for simplicity.
		/// </summary>
		/// <param name="grid">
		///   Grid used for comparison.
		/// </param>
		/// <param name="a">
		///   First coordinate in world coordinates.
		/// </param>
		/// <param name="b">
		///   Second coordinate in world coordinates.
		/// </param>
		public static bool AreAdjacent(this RectGrid grid, Vector3 a, Vector3 b) {
			// Convert to grid-coordinates
			var u = grid.WorldToGrid(a);
			var v = grid.WorldToGrid(b);
			// Manhattan distance is the sum of the horizontal and vertical distances
			var manhattanDistance = AbsDelta(u.x, v.x) + AbsDelta(u.y, v.y);

			// Diagonal positions would have a Manhattan distance of 2.
			return manhattanDistance <= 1.25;
		}

		/// <summary>
		///   Absolute difference between two values.
		/// </summary>
		private static float AbsDelta(float a, float b) {
			return Mathf.Abs(a - b);
		}
	}
}
