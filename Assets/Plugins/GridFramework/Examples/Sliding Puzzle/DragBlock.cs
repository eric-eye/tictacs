using UnityEngine;
using GridFramework.Extensions.Align;

namespace GridFramework.Examples.SlidingPuzzle {
	public class DragBlock : MonoBehaviour {
#region  Public variables
		/// <summary>
		///   Whether this block is being dragged by the player.
		/// </summary>
		public bool _beingDragged;
#endregion  // Public variables

#region  Private variables
		/// <summary>
		///   The offset between the touch point and the centre.
		/// </summary>
		/// <remarks>
		///   <para>
		///     The player will rarely click the block at its centre, but the
		///     block stil lhas to move around it. This offset is applied every
		///     time the player drags the block to make up for that difference.
		///   </para>
		/// </remarks>
		private Vector3 touchOffset;
	
		/// <summary>
		///   The last known save position we were able to snap to (the
		///   foresight works only from snap to snap).
		/// </summary>
		private Vector3 lastSnap;
	
		/// <summary>
		///   We can only move the block within these bounds (the grid itself
		///   is the largest possible bound).
		/// </summary>
		private Vector3[] _bounds;
#endregion  // Private variables
	
#region  Callback methods
		void Start() {
			_beingDragged = false;
			SlidingPuzzle.RegisterObstacle(transform, false);
		}
		
		void OnMouseDown() {
			var touchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			var position   = transform.position;
			var scale      = transform.lossyScale;

			_beingDragged = true;
			touchOffset   = touchPoint - position;
			lastSnap      = position;
			_bounds       = SlidingPuzzle.CalculateSlidingBounds(position, scale);

			SlidingPuzzle.RegisterObstacle(transform, true);
		}
		
		void OnMouseUp() {
			var position = transform.position;

			_beingDragged = false;
			/* transform.position = ClampPosition(position); */
			SlidingPuzzle._grid.AlignTransform(transform);
			lastSnap = position;
			SlidingPuzzle.RegisterObstacle(transform, false);
		}
		
		void FixedUpdate() {
			if (_beingDragged) {
				Drag();
			}
		}
#endregion  // Callback methods
		
#region  Private methods
		/// <summary>
		///   This is where the dragging logic takes places.
		/// </summary>
		private void Drag() {
			var input = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			var scale = transform.lossyScale;

			var destination = ClampPosition(input - touchOffset);
			destination.z = transform.position.z;
			
			// Now use that information to get the new bounds.
			_bounds = SlidingPuzzle.CalculateSlidingBounds(lastSnap, scale);
			
			// Simulate a snap to the grid so we can get potentially new bounds
			// in the next step.
			lastSnap = ClampPosition(SlidingPuzzle._grid.AlignVector3(destination, scale));
			
			// Move to the destination!
			transform.position = destination;
		}
		
		/// <summary>
		///   Doesn't let the block move out of the grid (uses the grid's
		///   renderFrom and renderTo).
		/// </summary>
		private Vector3 ClampPosition(Vector3 vec) {
			// If there are no other blocks in the way then at least the
			// renderer's From and To must serve as bounds or else we go off
			// grid.
			var from = SlidingPuzzle._para.From;
			var to   = SlidingPuzzle._para.To;
			var scale = transform.lossyScale;

			from = SlidingPuzzle._grid.GridToWorld(from);
			to   = SlidingPuzzle._grid.GridToWorld(  to);

			var lowerLimit = Vector3.Max(from + 0.5f * scale, _bounds[0]);
			var upperLimit = Vector3.Min(  to - 0.5f * scale, _bounds[1]);
			
			upperLimit.z = transform.position.z;
			lowerLimit.z = transform.position.z;
			
			// This method of using the maximum of the minimum is similar to
			// Unity's Mathf.Clamp(), except it is for vectors.
			return Vector3.Max(lowerLimit, Vector3.Min(upperLimit, vec));
		}
	
		// work in progress...
		private void RestrictDiagonally(ref Vector3 dest, ref bool[] restrict) {
			var diff = dest - transform.position;
	
			if (diff.x > 0) {  // East
				if (restrict[0] && diff.y > 0) {  // North-East
					Debug.Log("block NE!");
					if ((diff.y > diff.x) || transform.position.y > lastSnap.y) {
						dest.x = lastSnap.x;  // cancel horizontal
						Debug.Log("cancel horiz.");
					} else {
						dest.y = lastSnap.y;  // cancel vertical
					}
				} else if (restrict[3] && diff.y < 0) { // South-East
					if (-diff.y > diff.x) {
						dest.x = lastSnap.x;  // cancel horizontal
					} else {
						dest.y = lastSnap.y;  // cancel vertical
					}
				}
			} else {  // West
				if (restrict[1] && diff.y > 0) {  // North-West
					if (diff.y > -diff.x) {
						dest.x = lastSnap.x;  // cancel horizontal
					} else {
						dest.y = lastSnap.y;  // cancel vertical
					}
				} else if (restrict[2] && diff.y > 0) {  // South-West
					if (diff.y < diff.x) {
						dest.x = lastSnap.x;  // cancel horizontal
					} else {
						dest.y = lastSnap.y;  // cancel vertical
					}
				}
			}
		}
#endregion  // Private methods
	}
}
