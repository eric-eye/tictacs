using UnityEngine;
using System.Collections;
using GridFramework.Grids;
using GridFramework.Extensions.Nearest;

using CoordinateSystem = GridFramework.Grids.PolarGrid.CoordinateSystem;

namespace GridFramework.Examples.RotaryDial {
	/// <summary>
	///   Simulate the behaviour a rotary dial found on old telephones. You
	///   could use these techniques to create interesting GUIs or puzzles with
	///   spinning interfaces for the player.
	/// </summary>
	/// <remarks>
	///   <para>
	///     We achieve the result by first deciding which sector was clicked
	///     and then using the sector and the grid's angle to determine the
	///     total angle for the rotation. The actual game logic can be nicely
	///     encapsulated in OnReachStop, isolating it from the rotation.
	///   </para>
	/// </remarks>
	[RequireComponent(typeof(PolarGrid))]
	[RequireComponent(typeof(Collider))]
	public class RotaryDial : MonoBehaviour {
#region  Public variables
		/// <summary>
		///   Speed of the dial rotation.
		/// </summary>
		public float _speed = 75f;

		/// <summary>
		///   Speed boost factor when the dial is returning.
		/// </summary>
		public float _boost = 2f;

		/// <summary>
		///   How many sectors is the first number offset from its actual
		///   sector number.
		/// </summary>
		public int _offset =  1;

		/// <summary>
		///   How many sectors the finger stop is away from sector zero.
		/// </summary>
		public int _fingerStop =  1;

		/// <summary>
		///   If the number clicked is lower than this do nothing.
		/// </summary>
		public int _lowerLimit =  1;

		/// <summary>
		///   If the number clicked is larger than this do nothing.
		/// </summary>
		public int _upperLimit = 10;
#endregion  // Public variables

#region  Private variables
		/// <summary>
		///   Dial becomes unclickable while it is rotating
		/// </summary>
		private bool _clickable = true;

		/// <summary>
		///   Grid for grid logic.
		/// </summary>
		private PolarGrid _grid;

		/// <summary>
		///   Collider for clicking.
		/// </summary>
		private Collider _col;
#endregion  // Private variables

#region  Callback methods
		void Awake () {
			_grid = GetComponent<PolarGrid>();
			_col = GetComponent<Collider>();
		}

		void OnMouseDown () {
			// If the dial is spinning ignore clicks, else handle input and
			// rotate the dial.
			if (!_clickable) {
				return;
			}

			// Use raycasting to get the world coordinates where the player
			// clicked
			RaycastHit hit;
			_col.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
					out hit, Mathf.Infinity);

			// The Y-coordinate is our sector; round it to the nearest integer,
			// subtract the offset to get the number instead of the actual cell
			var face = _grid.NearestFace(hit.point, CoordinateSystem.Grid);

			var number = Mathf.FloorToInt(face.y) - _offset;
			RotateDial(number);
		}
#endregion  // Callback methods

#region  Private methods
		/// <summary>
		///   Compute the rotation angle and tell the script to apply the
		///   rotation.
		/// </summary>
		/// <param name="number">
		///   Number that was clicked.
		/// </param>
		private void RotateDial(int number) {
			// if the cell is outside our range we don't do anything and exit
			// the method
			if (number < _lowerLimit || number > _upperLimit) {
				return;
			}

			// The number tells us how many sectos we need to rotate by,
			// Degrees is the number of degree in one sector, and _offset is a
			// fixed number of additional sectors.
			var angle = (number + _offset + _fingerStop) * _grid.Degrees;

			// We apply the rotation over time via a custom coroutine. This is
			// an advanced topic and not related to Grid Framework so don't
			// worry if you don't understand it
			StartCoroutine(RotateOverTime(angle, number));
		}

		/// <summary>
		///   Called after the dial has finished rotating to, but before it
		///   starts rotating back.
		/// </summary>
		/// <param name="number">
		///   Cell which was clicked.
		/// </param>
		/// <remarks>
		///   <para>
		///     Put your game logic in here. The method is called by the
		///     animation method and the dialed number is supplied by it.
		///   </para>
		/// </remarks>
		void OnReachStop(int number) {
			Debug.Log("*ring* " + number);
		}

		/// <summary>
		///   Animates the dial.
		/// </summary>
		/// <param name="angle">
		///   How many degrees to rotate.
		/// </param>
		/// <param name="number">
		///   The number to report when the first rotation has ended.
		/// </param>
		/// <remarks>
		///   <para>
		///     This function is for use in coroutines, it will rotate the
		///     object around its own origin for a certain amount of degrees
		///     with fixed speed
		///   </para>
		/// </remarks>
		IEnumerator RotateOverTime(float angle, int number) {
			var axis  = -Vector3.forward;
			var pivot = transform.position;

			float startTime, angleCovered, t;

			// Remember the start time and disable clicks
			startTime = Time.time;
			_clickable = false;
			t = 0;

			while (t < 1.0f) {
				// the covered angle depends on the time passed and the speed
				angleCovered = (Time.time - startTime) * _speed;
				// t is the fraction of the full angle, update it
				t = angleCovered / angle;
				// reset the rotation
				transform.rotation = Quaternion.identity;
				// and apply the updated angle
				transform.RotateAround(pivot, axis, t * angle);
				// this loop will repeat as long as out fraction hasn't been filled
				yield return null;
			}

			OnReachStop(number);

			// it's time to rotate back, start the math over again
			startTime = Time.time;
			t = 0;

			while (t < 1.0f) {
				angleCovered = (Time.time - startTime) * _boost * _speed;
				t = angleCovered / angle;
				transform.rotation = Quaternion.identity;
				// similar to the above, excpet now (1 - t) because we go
				// backwards
				transform.RotateAround(pivot, axis, (1 - t) * angle);
				yield return null;
			}

			// Force reset to iron out inaccuracies due to rounding errors, and
			// re-enable clicks.
			transform.rotation = Quaternion.identity;
			_clickable = true;
		}
#endregion  // Private methods
	}
}
