using UnityEngine;
using GridFramework.Grids;

namespace GridFramework.Examples.LightsOut {
	/// <summary>
	///   Set up the example scene.
	/// </summary>
	/// <remarks>
	///   <para>
	///     In this script we store the initial state of the puzzle in an array
	///     and then assemble the objects when the game starts. Once a light
	///     bulb is instantiated it will take care of its own state itself.
	///   </para>
	///   <para>
	///     The puzzle array is the main piece of data. Its size determines the
	///     in-game position of this object. Other components of this object
	///     (like the grid) will be instantiated and set up during runtime. See
	///     the <c>Awake</c> method for details.
	///   </para>
	///   <para>
	///     If this were a real game we could load a different puzzle array and
	///     re-run the initialisation to set up the new puzzle. Since this is
	///     just and example everything is hard-coded for simplicity.
	///   </para>
	/// </remarks>
	public class LightsOut : MonoBehaviour {
#region  Public variables
		/// <summary>
		///   Material to use when the light is *on*.
		/// </summary>
		public Sprite _onSprite;

		/// <summary>
		///   Material to use when the light is *off*.
		/// </summary>
		public Sprite _offSprite;
#endregion  // Public variables

#region  Private variables
		/// <summary>
		///   Initial state of the puzzle, <c>false</c> means off, <c>true</c>
		///   means on.
		/// </summary>
		readonly bool[,] puzzle = {
			{false, true , false, true , true , true , true , true },
			{false, true , true , true , true , false, false, true },
			{true , false, true , false, true , false, false, false},
			{true , false, false, true , false, false, false, false},
			{false, false, true , false, false, true , true , false},
			{true , false, true , true , true , true , true , true },
			{false, true , false, true , true , false, true , false},
			{false, false, false, true , false, false, false, false},
		};
#endregion  // Private variables

#region  Callback methods
		void Awake() {
			// We will be needing the size of the puzzle for later
			var w = puzzle.GetLength(0);
			var h = puzzle.GetLength(1);

			// Position the carrier so the puzzle is central
			transform.position = new Vector3(-w / 2f, -h / 2f, 0);

			// Add a grid to this object so we can position the lights
			var grid = gameObject.AddComponent<RectGrid>();

			// The following loop wil iterate over the columns in every row. In
			// other words: i is the row and j is the column
			for (var i = 0; i < w; ++i) {
				for (var j = 0; j < h; ++j) {
					// Instantiate the light and add components
					var go = new GameObject();
					go.AddComponent<SpriteRenderer>();
					go.AddComponent<BoxCollider>();

					// The grid starts in the lower-left corner, but our puzzle
					// structure above starts in the upper-left corner, so when
					// determining the height of the light we subtract its
					// index from the top. We also set the Z-position because
					// of the overlap of the sprites
					var pos = new Vector3(j + .5f, h - 1 - i + .5f, -i);
					go.transform.position = grid.GridToWorld(pos);

					// Set up the LightBulb script
					var lb = go.AddComponent<LightBulb>();
					lb._grid = grid;
					lb._onSprite = _onSprite;
					lb._offSprite = _offSprite;
					lb._isOn = puzzle[i, j];

					// Turn on the sprite
					lb.SwitchLights();
				}
			}
		}
#endregion  // Callback methods

	}
}
