using UnityEngine;
#if  GRID_FRAMEWORK_VECTROSITY
using System.Collections.Generic;
using System.Collections;
using GridFramework.Renderers;
using Vectrosity;
using GridFramework.Extensions.Vectrosity;
#endif  // GRID_FRAMEWORK_VECTROSITY

namespace GridFramework.Examples.Vectrosity {
	public class ColorSwapping : MonoBehaviour {
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
#if  GRID_FRAMEWORK_VECTROSITY
		/// <summary>
		///   The list of assigned colours.
		/// </summary>
		private List<Color32> _colors;

		/// <summary>
		///   The array of possible colours.
		/// </summary>
		private Color[] _palette = {
			Color.white,
			Color.red,
			Color.green,
			Color.blue,
			Color.yellow,
			Color.cyan,
			Color.magenta,
		};

		/// <summary>
		///   The Vectrosity line.
		/// </summary>
		private VectorLine _line;

		/// <summary>
		///   For looping though the arrays.
		/// </summary>
		private int _iterator;

		/// <summary>
		///   Whether to change the color of a line during the current frame.
		/// </summary>
		private bool _doChangeColor = true;
#endif  // GRID_FRAMEWORK_VECTROSITY
#endregion  // Private variables

#if  GRID_FRAMEWORK_VECTROSITY
#region  Callback methods
		void Start() {
			_line = MakeLine(_texture);
			_colors = MakeColors(_line.points3.Count);
			SetupLine(_line, _colors);
		}

		void Update() {
			DelayChanging();
			if(_doChangeColor){
				// Pick a random colour for the current line and apply it.
				var index = Random.Range(0, 7);
				_colors[_iterator] = _palette[index];
				_line.SetColors(_colors);
				++_iterator; //next line
				// 0 -> 1 -> 2 -> 3 -> 4 -> 5 -> 6 -> 0 -> 1 -> 2 -> ...
				_iterator %= _colors.Count;
			}
			Rotate();
		}
#endregion  // Callback methods

#region  Private methods
		/// <summary>
		///   Make a vector line from the grid's renderer.
		/// </summary>
		private VectorLine MakeLine(Texture texture) {
			const float  width   = 10f;
			const string lineName = "Color-swapping Lines";

			var gridRenderer = GetComponent<GridRenderer>();
			var points = gridRenderer.GetVectrosityPoints();

			for (var i = 0; i < points.Count; ++i) {
				var point = points[i];
				points[i] = point - transform.position;
			}

			return new VectorLine(lineName, points, texture, width);
		}

		/// <summary>
		///   Instantiate the list of colors for the line.
		/// </summary>
		private List<Color32> MakeColors(int amount) {
			var paletteLength = _palette.Length;
			var lineColors = new List<Color32>();

			for(var i = 0; i < amount / 2; i++){
				//i % colors.Length returns always a number between 0 and the amout of
				//colous we have listed. it increments every time and when the maximum
				//has been reached it reverts back to zero
				var index = i % paletteLength;
				lineColors.Add(_palette[index]);
			}

			return lineColors;
		}

		/// <summary>
		///   Prepare a vector line for our use-case.
		/// </summary>
		/// <param name="line">
		///   The line to set up.
		/// </param>
		private void SetupLine(VectorLine line, List<Color32> colors) {
			line.material      = _material;
			line.drawTransform = transform;
			line.SetColors(colors);
			line.Draw3DAuto();
		}

		/// <summary>
		///   Wait a while before allowing to change colours again.
		/// </summary>
		private IEnumerator DelayChanging() {
			var waitPeriod  = Random.Range(0.2f, 1.0f);
			var currentTime = Time.time;
			var endTime     = currentTime + waitPeriod;
			_doChangeColor = false;

			while (currentTime < endTime) {
				currentTime = Time.time;
				yield return null;
			}
			_doChangeColor = true;
		}

		/// <summary>
		///   Rotate the grid in place.
		/// </summary>
		private void Rotate() {
			var rot1 = -15 * Vector3.right * Time.deltaTime;
			var rot2 =  10 * Vector3.up    * Time.deltaTime;
			transform.Rotate(rot1);
			transform.Rotate(rot2, Space.World);
		}
#endregion  // Private methods
#endif  // GRID_FRAMEWORK_VECTROSITY
	}
}
