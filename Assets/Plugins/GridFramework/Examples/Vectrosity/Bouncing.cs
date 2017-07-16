using UnityEngine;
using GridFramework.Renderers;
#if  GRID_FRAMEWORK_VECTROSITY
using Vectrosity;
using GridFramework.Extensions.Vectrosity;
#endif  // GRID_FRAMEWORK_VECTROSITY

namespace GridFramework.Examples.Vectrosity {
	[RequireComponent(typeof(GridRenderer))]
	public class Bouncing : MonoBehaviour {
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

#if  GRID_FRAMEWORK_VECTROSITY
#region  Callback methods
		void Start() {
			var line = MakeLine(_texture);
			SetupLine(line);
		}
#endregion  // Callback methods

#region  Private methods
		/// <summary>
		///   Make a vector line from the grid's renderer.
		/// </summary>
		private VectorLine MakeLine(Texture texture) {
			const float  width    = 10f;
			const string lineName = "Bouncy Lines";

			var gridRenderer = GetComponent<GridRenderer>();
			var points = gridRenderer.GetVectrosityPoints();

			for (var i = 0; i < points.Count; ++i) {
				var point = points[i];
				var local = transform.InverseTransformPoint(point);
				points[i] = local;
			}

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
			line.drawTransform = transform;
			line.Draw3DAuto();
		}
#endregion  // Private methods
#endif  // GRID_FRAMEWORK_VECTROSITY
	}
}
