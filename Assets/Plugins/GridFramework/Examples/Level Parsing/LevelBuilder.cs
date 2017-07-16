using UnityEngine;
using System.Collections.Generic; //needed for the generic list class
using GridFramework.Grids;

namespace GridFramework.Examples.LevelParsing {
	/// <summary>
	///   Assembles a level from data along the grid.
	/// </summary>
	/// <remarks>
	///   <para>
	///     The level arrangements is stored inside this script, and we can
	///     cycle through the levels. All instantiated blocks are kept track of
	///     in oder to be be able to remove them again when assembling another
	///     level.
	///   </para>
	///   <para>
	///     This example is using a hexagonal grid, but it can be just as well
	///     be used for any other type of grid when you choose the appropriate
	///     coordinate conversion function.
	///   </para>
	/// </remarks>
	[RequireComponent(typeof(HexGrid))]
	public class LevelBuilder : MonoBehaviour {
#region  Private variables
		private static int[,] _level1 = {
			{1, 2, 3, 2, 3, 2, 3, 1, 1, 2, 1}, 
			{3, 1, 2, 1, 0, 2, 1, 2, 3, 1, 3}, 
			{2, 0, 3, 2, 0, 1, 0, 1, 2, 3, 2}, 
			{3, 1, 0, 0, 2, 3, 1, 2, 3, 1, 3}, 
			{2, 0, 3, 2, 3, 2, 3, 1, 2, 3, 2}, 
		};

		private static int[,] _level2 = {
			{0, 3, 3, 1, 3, 2, 2, 3, 0, 2, 0}, 
			{1, 3, 3, 2, 3, 3, 1, 0, 0, 1, 1}, 
			{1, 3, 0, 0, 2, 3, 2, 0, 0, 1, 1}, 
			{1, 3, 2, 0, 0, 2, 2, 0, 1, 1, 1}, 
			{3, 1, 0, 0, 1, 3, 2, 3, 1, 2, 3}, 
		};

		private static int[,] _level3 = {
			{1, 2, 0, 2, 0, 3, 2, 0, 3, 1, 1}, 
			{0, 1, 3, 1, 0, 3, 3, 0, 0, 0, 0}, 
			{1, 0, 2, 3, 2, 0, 0, 3, 1, 3, 1}, 
			{3, 1, 0, 1, 0, 1, 1, 2, 1, 2, 3}, 
			{0, 2, 2, 3, 2, 2, 0, 1, 0, 2, 0}, 
		};

		private static readonly int[][,] _levels = { _level1, _level2, _level3 };

		/// <summary>
		///   Index of the current level into the <c>_levels</c> array.
		/// </summary>
		private int _currentLevel;

		/// <summary>
		///   The grid we place blocks on.
		/// </summary>
		private HexGrid _grid;

		/// <summary>
		///   In order to delete all the blocks we need to keep track of them.
		/// </summary>
		private List<GameObject> _blocks;
#endregion  // Private variables

#region  Public variables
		/// <summary>
		///   An array of text files to be read.
		/// </summary>
		public TextAsset[] _levelFiles;
			
		// Prefabs for our objects
		/// <summary>
		///   Prefab for the red object.
		/// </summary>
		public GameObject _red;

		/// <summary>
		///   Prefab for the green object.
		/// </summary>
		public GameObject _green;

		/// <summary>
		///   Prefab for the blue object.
		/// </summary>
		public GameObject _blue;
#endregion  // Public variables
		
#region  Callback methods
		public void Awake(){
			_grid = GetComponent<HexGrid>();
			_blocks = new List<GameObject>();

			BuildLevel();
		}

		void OnGUI(){
			const string message = "Try Another Level";
			var buttonRect = new Rect(10, 10, 150, 50);

			if (GUI.Button(buttonRect, message)) {
				_currentLevel = (++ _currentLevel) % _levels.GetLength(0);
				BuildLevel();
			}
		}
#endregion  // Callback methods

#region  Private methods
		/// <summary>
		///   Spawns blocks based on a text file and a grid.
		/// </summary>
		private void BuildLevel() {
			// loop over the list of old blocks and destroy all of them, we
			// don't want the new level on top of the old one. Destroying the
			// blocks doesn't remove the reference to them in the list, so
			// clear the list
			foreach (var go in _blocks) {
				Destroy(go);
			}
			_blocks.Clear();

			var level = _levels[_currentLevel];

			for (var r = 0; r < level.GetLength(0); ++r) {
				for (var c = 0; c < level.GetLength(1); ++c) {
					GameObject block;

					switch (level[r, c]) {
						case 1: block = _red  ; break;
						case 2: block = _green; break;
						case 3: block = _blue ; break;
						default: continue;
					}

					var position = _grid.HerringUpToWorld(new Vector3(c, r, 0));
					var obj = Instantiate(block, position, Quaternion.identity) as GameObject;

					_blocks.Add(obj);
				}
			}
		}
#endregion  // Private methods
	}
}
