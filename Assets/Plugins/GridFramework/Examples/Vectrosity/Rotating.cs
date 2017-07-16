using UnityEngine;
#if  GRID_FRAMEWORK_VECTROSITY
using GridFramework.Renderers;
using Vectrosity;
using GridFramework.Extensions.Vectrosity;
#endif  // GRID_FRAMEWORK_VECTROSITY

namespace GridFramework.Examples.Vectrosity {
	public class Rotating : MonoBehaviour {
#region  Public variables
		/// <summary>
		///   Material for the line renderer.
		/// </summary>
		public Material _material;

		/// <summary>
		///   Texture for lines.
		/// </summary>
		public Texture _texture;
#endregion  // Public variables

#region  Private variables
		/// <summary>
		///   Width of the lines.
		/// </summary>
		public float _width = 7f;
#endregion  // Private variables

#if  GRID_FRAMEWORK_VECTROSITY
#region  Callback methods
		void Start() {
			var line = MakeLine(_width);
			SetupLine(line);
		}

		void Update() {
			var deltaTime = Time.deltaTime;
			var rotation1 = 10 * Vector3.right * deltaTime;
			var rotation2 = 15 * Vector3.up    * deltaTime;
			transform.Rotate(rotation1);
			transform.Rotate(rotation2, Space.World);
		}

#endregion  // Callback methods

#region  Private methods
		/// <summary>
		///   Make a vector line from the grid's renderer.
		/// </summary>
		private VectorLine MakeLine(float width) {
			const string lineName = "Rotating Lines";

			var gridRenderer = GetComponent<GridRenderer>();
			var points = gridRenderer.GetVectrosityPoints();

			for (var i = 0; i < points.Count; ++i) {
				var point = points[i];
				points[i] = point - transform.position;
			}

			var line = new VectorLine(lineName, points, width);
			return line;
		}

		/// <summary>
		///   Prepare a vector line for our use-case.
		/// </summary>
		/// <param name="line">
		///   The line to set up.
		/// </param>
		private void SetupLine(VectorLine line) {
			line.material      = _material;
			line.drawTransform = transform;
			line.Draw3DAuto();
		}
#endregion  // Private methods
#endif  // GRID_FRAMEWORK_VECTROSITY
	}
}
