using UnityEngine;
using GridFramework.Grids;
using GridFramework.Renderers.Rectangular;

namespace GridFramework.Examples.SlidingPuzzle {
	/// <summary>
	///   Behaviour that initializes the puzzle when the game starts with the
	///   grid and parallelepiped renderer.
	/// </summary>
	[RequireComponent(typeof(RectGrid))]
	[RequireComponent(typeof(Parallelepiped))]
	public class PuzzleGrid : MonoBehaviour {
		// Awake is being called before Start; this makes sure we have a matrix
		// to begin with when we add the blocks
		void Awake() {
			// because of how we wrote the accessor this will also immediately
			// build the matrix of our level
			var grid = gameObject.GetComponent<RectGrid>();
			var para = gameObject.GetComponent<Parallelepiped>();
			SlidingPuzzle.InitializePuzzle(grid, para);
		}
		
		// visualizes the matrix in text form to let you see what's going on
		void OnGUI(){
			const int w = 100;
			const int h = 100;
			const int x =  10;

			var y = Screen.height - x - h;

			GUI.TextArea(new Rect(x, y, w, h), SlidingPuzzle.MatrixToString());
		}
	}
}
