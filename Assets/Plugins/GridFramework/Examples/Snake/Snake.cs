// In the grid-based movement example I demonstrated how to move square by
// square.  Here it is taken one step further, an entire chain of objects are
// moved. we store all segments (including the head) in a list and issue
// movement for all of them in each turn. The head is moved manually and all
// other segments take the psotion of their predecessor. The actual nake object
// is not moved at all, instead it just manages the semgents.
// 
// This example demonstrates how a simple concept (moving from square to
// sqaure) can be used to solve a problem that appears to be more complex.
// Instead of moving each segment we just move the head and have the other
// segments follow it.


using UnityEngine;
using System.Collections;
using System.Collections.Generic;  // needed for List<T>
using GridFramework.Grids;
using GridFramework.Renderers.Rectangular;
using System.Linq;  // neeed for Last()

namespace GridFramework.Examples.Snake {
	public class Snake : MonoBehaviour {
#region  Private variables
		/// <summary>
		///   By how many segments to grow the snake (add to make the snake
		///   grow during gameplay).
		/// </summary>
		private int grow;

		/// <summary>
		///   This will be false while moving, we don't want movements to
		///   stack.
		/// </summary>
		private bool movable = true;

		/// <summary>
		///   We will hold all movement until all segments are ready.
		/// </summary>
		private bool onHold = true;

		/// <summary>
		///   Keep track of moving segments, enable movement only after each
		///   one has finished moving.
		/// </summary>
		private int movingSegments;

		/// <summary>
		///   We will store our segments here, the first segment is the head,
		///   the last one the tail.
		/// </summary>
		private List<Transform> segments;

		/// <summary>
		///   The four possible directions and one neutral (centre) direction.
		/// </summary>
		private enum Direction {N, E, S, W, C};
#endregion  // Private variables

#region  Public variables
		/// <summary>
		///   The grid we use for moving the head.
		/// </summary>
		public RectGrid _grid; 

		/// <summary>
		///   The renderer for the grid.
		/// </summary>
		public Parallelepiped _renderer;

		/// <summary>
		///   Prefab for the snake segments.
		/// </summary>
		public Transform _segmentPrefab;

		/// <summary>
		///   The starting length of our snake.
		/// </summary>
		public int _startSize = 8;

		/// <summary>
		///   Snake movement speed (applies to each segment).
		/// </summary>
		public float _speed = 5.0f;
#endregion  // Public variables

#region  Callback methods
		void Awake () {
			segments = new List<Transform>();
			BuildSnake ();
		}

		void Update () {
			// Ignore all input if the snake is already moving or if the player
			// isn't giving any directions.
			var h = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
			var v = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

			if (!movable || (h == 0 && v == 0)) {
				return;
			}

			var dir = Direction.C;
			if (h == 1) {
				dir = Direction.E;
			} else if (h == -1) {
				dir = Direction.W;
			} else if (v == 1) {
				dir = Direction.N;
			} else if (v == -1) {
				dir = Direction.S;
			}

			// Now move the snake (the direction is translated to a vector
			// before that)
			var moveTo = DirectionToVector(dir);
			Move(moveTo);
		}
#endregion  // Callback methods

#region  Private methods
		/// <summary>
		///   Builds our initial snake by setting its initial growth to the starting size.
		/// </summary>
		private void BuildSnake () {
			// A snake needs at least a head, so make sure the minimum growth
			// is 1. Moving by zero will not move the snake but it will still
			// make a segment, our head, spawn.
			grow = Mathf.Max(1, _startSize);
			Move(Vector3.zero);
		}


		/// <summary>
		///   Takes in one of the five directions and returns the corresponding vector.
		/// </summary>
		private Vector3 DirectionToVector(Direction dir) {
			var up    = _grid.Up;
			var right = _grid.Right;
			switch (dir) {
				case Direction.N: return  up;
				case Direction.S: return -up;
				case Direction.E: return  right;
				case Direction.W: return -right;
				default: return Vector3.zero;
			}
		}

		/// <summary>
		///   Move the snake into a direction.
		/// </summary>
		private void Move(Vector3 dir) {
			// First check if the destination we want to move towards is within
			// bounds; if not we print a message. If the snake has no head we
			// can't do any check, so make sure there is at least one segment
			if (segments.Count > 0) {
				var target  = segments[0].position + dir;
				var outside = OutsideRange(target);
				if (outside) {
					Debug.Log("Illegal Move, turn around!");
					return;
				}
			}

			// Now let's grow our snake; we grow only one segment per turn.
			grow = Grow(grow);

			movable = false; // We are ready to move, so stop all movement input.

			// From behind move every segment to the position of its
			// predecessor; note that the animation is on hold until every
			// movement has been assigned.
			for (int i = segments.Count - 1; i > 0; --i) { 
				var segment = segments[i];
				var target  = segments[i - 1].position;
				StartCoroutine(MoveWithSpeed(segment, target));
			}
			// finally, move the head
			StartCoroutine(MoveWithSpeed(segments[0], segments[0].position + dir));

			// all movement has been issued, now we can release the segments
			onHold = false;
		}

		/// <summary>
		///   Whether the head of the snake is outside the grid.
		/// </summary>
		/// <param name="position">
		///   Position in grid-coordinates.
		/// </param>
		/// <remarks>
		///   <para>
		///   </para>
		/// </remarks>
		private bool OutsideRange(Vector3 position) {
			var local = _grid.WorldToGrid(position);
			var from  = _renderer.From;
			var   to  = _renderer.To;

			var x = local.x;
			var y = local.y;

			if (x > to.x || y > to.y ||x < from.x || y < from.y){
				return true;
			}
			return false;
		}

		/// <summary>
		///   Grow the snake by a certain number of segments.
		/// </summary>
		private int Grow(int growth) {
			if (growth <= 0) {
				return 0;
			}

			// If the list is empty (no head) we spawn our head where the
			// snake object lies.
			var hasSegments = segments.Count > 0;
			var tailPos = hasSegments ? segments.Last().position : transform.position;
			var newSegment = Instantiate(_segmentPrefab);

			// Parenting isn't really needed, but it keeps things
			// organized. Place the new segment at the tail and append it
			// to the list, then decrement the growth counter.
			newSegment.parent   = transform;
			newSegment.position = tailPos;
			segments.Add(newSegment);

			return --growth;
		}

		/// <summary>
		///   This is a coroutine, it runs independently of the script's Update
		///   method. Coroutines can be used to "do stuff over time".
		/// </summary>
		private IEnumerator MoveWithSpeed (Transform trns, Vector3 position) {
			// For each segment that has had its movement assigned we increment
			// the counter. Until all segments (including the head) have been
			// assigned the animation is on hold.
			++movingSegments;
			while (onHold) {
				yield return null;
			}

			var startPos = trns.position;
			var distance = Vector3.Magnitude(startPos - position);
			var startTime = Time.time;
			var distanceCovered = 0.0f;
			var t = 0.0f;

			// The check distance > 0.0f is needed to make sure we don't divide
			// by 0.
			while (t < 1.0f && distance > 0.0f) {
				distanceCovered = (Time.time - startTime) * _speed; // s = v • t
				t = distanceCovered / distance;
				trns.position = Vector3.Lerp (startPos, position, t);
				yield return null;
			}

			trns.position = position;

			// This segment is done, so decrease the counter. Once the last
			// segment has finished its animation all future animations will be
			// on hold and the snake can receive movement input again.
			--movingSegments;
			if (movingSegments == 0) {
				onHold = true;
				movable = true;
			}
		}
#endregion  // Private methods
	}
}
