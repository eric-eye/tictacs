using UnityEngine;
#if  GRID_FRAMEWORK_VECTROSITY
using GridFramework.Grids;
using GridFramework.Renderers.Hexagonal;
using Vectrosity;
using GridFramework.Extensions.Vectrosity;
#endif  // GRID_FRAMEWORK_VECTROSITY

namespace GridFramework.Examples.Vectrosity {
	public class Resizing : MonoBehaviour {
#region  Public variables
		/// <summary>
		///   Material for the line renderer.
		/// </summary>
		public Material _material;

		/// <summary>
		///   Texture for lines.
		/// </summary>
		public Texture _texture;

		/// <summary>
		///   Color of the lines.
		/// </summary>
		public Color _color = new Color(1f, 0f, 0f, 1f);
#endregion  // Public variables

#region  Private variables
#if  GRID_FRAMEWORK_VECTROSITY
		private HexGrid _grid;
		private Rectangle _renderer;
		private Vector3 tempPos;
		private VectorLine _line;
		private bool growingRadius = true;
#endif  // GRID_FRAMEWORK_VECTROSITY
#endregion  // Private variables

#if  GRID_FRAMEWORK_VECTROSITY
#region  Callback methods
		void Start() {
			_line = MakeLine(_texture);
			SetupLine(_line);
		}

		void Update() {
			ResizeGrid(_grid);
			
			// In order for the rendering to align properly with the grid the
			// grid has to be at the world's origin. This is not a very
			// efficient way of doing things, every frame we instantiate a new
			// list of points and throw the old one away, causing more work for
			// the garbage collector. For such a small grid the impact is
			// negligible tough.
			tempPos = transform.position;
			transform.position = Vector3.zero;
			_line.points3.Clear();
			_line.points3.AddRange(_renderer.GetVectrosityPoints());
			transform.position = tempPos;
		}
#endregion  // Callback methods

#region  Private methods
		/// <summary>
		///   Make a vector line from the grid's renderer.
		/// </summary>
		private VectorLine MakeLine(Texture texture) {
			const float  width    = 7f;
			const string lineName = "Resizing Lines";

			_renderer = GetComponent<Rectangle>();
			_grid     = GetComponent<HexGrid>();
			var points = _renderer.GetVectrosityPoints();

			return new VectorLine(lineName, points, texture, width);
		}

		/// <summary>
		///   Prepare a vector line for our use-case.
		/// </summary>
		/// <param name="line">
		///   The line to set up.
		/// </param>
		private void SetupLine(VectorLine line) {
			line.material      = _material;
			line.color         = _color;
			line.Draw3DAuto();
		}

		private void ResizeGrid(HexGrid grid){
			var current  = grid.Radius;
			var target   = growingRadius ? 3f : 2f;
			var maxDelta = .5f * Time.deltaTime;
			var atLimit  = Mathf.Abs(grid.Radius - target) <= Mathf.Epsilon;

			grid.Radius = Mathf.MoveTowards(current , target, maxDelta);

			if (growingRadius) {
				growingRadius &= !atLimit;
			} else {
				growingRadius |= atLimit;
			}
		}
#endregion  // Private methods
#endif  // GRID_FRAMEWORK_VECTROSITY
	}
}
