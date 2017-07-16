using UnityEngine;
using GridFramework.Grids;
using GridFramework.Renderers.Rectangular;
using GridFramework.Extensions.Nearest;

using CoordinateSystem = GridFramework.Grids.RectGrid.CoordinateSystem;

namespace GridFramework.Examples.SlidingPuzzle {
	public static class SlidingPuzzle {
#region  Private variables
		/// <summary>
		///   Private member variable for the renderer of the grid.
		/// </summary>
		public static Parallelepiped _para;

		/// <summary>
		///   This is where we store our information; all game logic uses this
		///   matrix.
		/// </summary>
		private static bool[,] levelMatrix;

		/// <summary>
		///   Size of the level matrix, first component is horizontal, second
		///   vertical.
		/// </summary>
		private static int[] levelMatrixSize;
#endregion  // Private variables

#region  Public variables
		/// <summary>
		///   Private member variable for the grid.
		/// </summary>
		public static RectGrid _grid;
#endregion  // Public variables

#region  Private methods
		/// <summary>
		///   Takes the grid's rendering range and builds a matrix based on
		///   that. All entries are set to true.
		/// </summary>
		private static void BuildLevelMatrix() {
			// Amount of rows and columns, either based on size or rendering
			// range (first entry rows, second one columns).
			ComputeMatrixSize();
			var w = levelMatrixSize[0];
			var h = levelMatrixSize[1];

			levelMatrix = new bool[w, h];

			// Set all entries to true, all squares allowed initially.
			for (var i = 0; i < w; ++i) {
				for (var j = 0; j < h; ++j) {
					levelMatrix[i, j] = true;
				}
			}
		}
		
		/// <summary>
		///   How large the matrix should be. For the sake of simplicity we
		///   only use the rendering range here.
		/// </summary>
		private static void ComputeMatrixSize() {
			var from = _para.From;
			var to   = _para.To;

			// If there is no matrix yet create it, otherwise we can just
			// overwrite its values.
			levelMatrixSize = levelMatrixSize ?? new int[2];

			for (var i = 0; i < 2; ++i) {
				// Get the distance between both ends (in world units), divide
				// it by the spacing (to get grid units) and round down to the
				// nearest integer
				var lower = Mathf.CeilToInt(from[i]);
				var upper = Mathf.CeilToInt(  to[i]);
				levelMatrixSize[i] = upper - lower;
			}
		}
		
		/// <summary>
		///   Take world coodinates and find the corresponding square. The
		///   result is returned as an int array that contains that square's
		///   position in the matrix.
		/// </summary>
		private static int[] GetSquare(Vector3 vec) {
			const CoordinateSystem system = CoordinateSystem.Grid;

			var cell    = _grid.NearestCell(vec, system);
			var shift   = .5f * Vector3.one;
			var square  = cell - shift;
			var indices = new int[2];

			for (var i = 0; i < 2; ++i) {
				// Remember, boxes don't have whole coordinates, that's why we
				// use a little shift to turn e.g. (3.5, 2.5, 1.5) into (3, 2,
				// 1).
				indices[i] = Mathf.RoundToInt(square[i]);
			}

			return indices;
		}
#endregion  // Private methods
		
#region  Public methods
		/// <summary>
		///   Initialize the puzzle using a grid and renderer.
		/// </summary>
		public static void InitializePuzzle(RectGrid grid, Parallelepiped para) {
			_grid = grid;
			_para = para;
			BuildLevelMatrix();
		}

		/// <summary>
		///   Takes world coodinates, finds the corresponding square and sets
		///   that entry to either true or false. Use it to disable or enable
		///   squares.
		/// </summary>
		public static void RegisterObstacle(Transform obstacle, bool state) {
			// First break up the obstacle into several 1x1 obstacles.
			var parts = BreakUpObstacle(obstacle);

			// Now find the square of each part and set it to true or false.
			for (var i = 0; i < parts.GetLength(0); ++i) {
				for (var j = 0; j < parts.GetLength(1); ++j) {
					var square = GetSquare(parts[i, j]);
					levelMatrix[square[0], square[1]] = state;
				}
			}
		}

		/// <summary>
		///   Calculates the movement constrains for a block that's being
		///   dragged.
		/// </summary>
		/// <remarks>
		///   <para>
		///     When we want to slide a block we need to know how far we can go
		///     before we "collide" (note that there is no actual Unity
		///     collision detection involved anywhere).  We can only look up to
		///     on square ahead in each direction, so the bounds need to be
		///     recalculated from time to time; this allows us to have
		///     obstacles in all sorts of directions, like a maze that can
		///     change all the time.
		///   </para>
		/// </remarks>
		public static Vector3[] CalculateSlidingBounds(Vector3 pos, Vector3 scl){
			// Break up the block and find the lower left and upper right
			// square in the matrix.
			var squares       = BreakUpObstacle(pos, scl);
			var squaresWidth  = squares.GetLength(0);
			var squaresHeight = squares.GetLength(1);
			var lowerLeft     = GetSquare(squares[0, 0]);
			var upperRight    = GetSquare(squares[squaresWidth - 1, squaresHeight - 1]);
			var matrixWidth   = levelMatrix.GetLength(0);
			var matrixHeight  = levelMatrix.GetLength(1);

			int left = lowerLeft[0], bottom = lowerLeft[1], right = upperRight[0], top = upperRight[1];

			// For each adjacent left square check if all left fields are free
			// (a bitmask would have been the way to go instead of four bools,
			// but let's keep it simple).
			var freeEast = true;

			//iterate over all the squares one square left of the left edge
			for (var i = bottom; i <= top; ++i) {
				// use Min so we don't go off the matrix size.
				var x = Mathf.Min(matrixWidth - 1, right + 1);
				var y = i;
				freeEast = freeEast && levelMatrix[x, y];
			}

			bool freeWest = true;
			//iterate
			for (var i = bottom; i <= top; ++i){
				// Use the Max so we don't get negative values (the matrix
				// starts at 0).
				var x = Mathf.Max(0, left - 1);
				var y = i;
				freeWest = freeWest && levelMatrix[x, y];
			}
					
			var freeSouth = true;
			for (var i = left; i <= right; ++i){
				var x = i;
				var y = Mathf.Max(0, bottom - 1);
				freeSouth = freeSouth && levelMatrix[x, y];
			}
			
			var freeNorth = true;
			for (var i = left; i <= right; ++i) {
				var x = i;
				var y = Mathf.Min(matrixHeight - 1, top + 1);
				freeNorth = freeNorth && levelMatrix[x, y];
			}

			// Now assume the block canot move anywhere; for each free
			// direction loosen the constraints by one grid unit (world unit *
			// spacing).
			var bounds = new [] {pos, pos};
			if (freeEast)
				bounds[1] += _grid.Spacing.x*Vector3.right;
			if (freeNorth)
				bounds[1] += _grid.Spacing.y*Vector3.up;
			if (freeWest)
				bounds[0] -= _grid.Spacing.x*Vector3.right;
			if (freeSouth)
				bounds[0] -= _grid.Spacing.y*Vector3.up;
			
			// the bounds can still be outside of the grid, so we need to clamp that as well
			for (var i = 0; i < 2; i++) {
				for (var j = 0; j < 2; j++) {
					var to   = _para.To;
					var bound = bounds[i][j];
					bounds[i][j] = Mathf.Clamp(bound, _grid.GridToWorld(Vector3.zero)[j] + 0.5f * scl[j], _grid.GridToWorld(to)[j] - 0.5f * scl[j]);
				}
			}
			
			return bounds;
		}

		public static void CalculateDiagonalBounds(Vector3 pos, Vector3 scl, ref bool[] bounds) {
			// break up the block and find the lower left and upper right square in the matrix
			var squares    = BreakUpObstacle(pos, scl);
			var lowerLeft  = GetSquare(squares[0, 0]); // we store the position inside the matrix here
			var w = squares.GetLength(0) - 1;
			var h = squares.GetLength(1) - 1;
			var upperRight = GetSquare(squares[w, h]);
			
			var left   = lowerLeft[0]  - 1;
			var bottom = lowerLeft[1]  - 1;
			var right  = upperRight[0] + 1;
			var top    = upperRight[1] + 1;

			bounds = bounds ?? new bool[4];

			var maxTop   = levelMatrixSize[1] - 1;
			var maxRight = levelMatrixSize[0] - 1;

			bounds[0] = top     < maxTop && right < maxRight && !levelMatrix[right,    top]; // North-East
			bounds[1] = top     < maxTop && left  > 0        && !levelMatrix[ left,    top]; // North-West
			bounds[2] = bottom  > 0      && left  > 0        && !levelMatrix[ left, bottom]; // South-West
			bounds[3] = bottom  > 0      && right < maxRight && !levelMatrix[right, bottom]; // South-East
		}
		
		/// <summary>
		///   An alternative to the above that takes in a Transform as an
		///   argument.
		/// </summary>
		public static Vector3[,] BreakUpObstacle(Transform obstacle) {
			var position = obstacle.position;
			var scale    = obstacle.lossyScale;

			return BreakUpObstacle(position, scale);
		}

		/// <summary>
		///   Break a large obstacle spanning several squares into several
		///   obstacles spanning one square each.
		/// </summary>
		public static Vector3[,] BreakUpObstacle(Vector3 pos, Vector3 scale) {
			// First convert the scale to int and store X and Y values
			// separate.
			var spacing = _grid.Spacing;
			var obstacleScale = new int[2];
			for (var i = 0; i < 2; i++) {
				var roundedScale   = Mathf.RoundToInt(scale[i]);
				var roundedSpacing = Mathf.RoundToInt(spacing[i]);
				var localScale     = Mathf.CeilToInt(roundedScale / roundedSpacing);

				obstacleScale[i] = Mathf.Max(1, localScale); //no lower than 1
			}
			
			// We will apply a shift so we always get the centre of the broken
			// up parts, the shift depends on whether even or odd.
			var shift = new Vector3[2];
			for (var k = 0; k < 2; ++k) {
				if (obstacleScale[k]%2 == 0) { // even
					shift[k] = (-obstacleScale[k] / 2.0f + 0.5f) * (k==0 ? Vector3.right : Vector3.up);
				} else { // odd
					shift[k] = (-(obstacleScale[k] - 1) / 2.0f) * (k==0 ? Vector3.right : Vector3.up);
				}
			}
			
			// this is where we store the single obstacles
			var obstacleMatrix = new Vector3[obstacleScale[0], obstacleScale[1]];
			
			// now break the obstacle up into squares and handle each square
			// individually like an obstacle.
			for (var i = 0; i < obstacleScale[0]; ++i) {
				for (var j = 0; j < obstacleScale[1]; ++j) {
					// What shift does is shift from the centre of the obstacle
					// to the centre of the lower left sqare; from there we
					// slowly increment over the remaining squares

					var scaled = Vector3.Scale(new Vector3(i, j, 0), spacing);
					obstacleMatrix[i,j] = pos + shift[0] + shift[1] + scaled;
				}
			}
			
			return obstacleMatrix;
		}
		
		/// <summary>
		///   This returns the matrix as a string so you can read it yourself,
		///   like in a GUI for debugging (nothing grid-related going on here,
		///   feel free to ignore it).
		/// </summary>
		public static string MatrixToString() {
			const string vacant   = "_";
			const string occupied = "X";

			var text = "";
			for (var j = levelMatrix.GetLength(1) - 1; j >= 0; --j) {
				for (var i = 0; i < levelMatrix.GetLength(0); ++i) {
					text = text + (levelMatrix[i,j] ? vacant : occupied) + " ";
				}
				text += "\n";
			}
			return text;
		}
		
		// This method was used at some point but now it is of no use; I left it in though for you if you are interested
	/*	//takes world coodinates, finds the corresponding square and returns the value of that square. Use it to cheack if a square is forbidden or not
		public static bool CheckObstacle(Transform obstacle){
			bool free = true; // assume it is allowed
			//first break up the obstacle into several 1x1 obstacles
			Vector3[,] parts = BreakUpObstacle(obstacle);
			//now find the square of each part and set it to true or false
			for(int i = 0; i < parts.GetUpperBound(0) + 1; i++){
				for(int j = 0; j < parts.GetUpperBound(1) + 1; j++){
					int[] square = GetSquare(parts[i,j]);
					free = free && levelMatrix[square[0],square[1]]; // add all the entries, returns true if and only if all are true
				}
			}
			return free;
		}
	*/
#endregion  // Public methods
	}
}
