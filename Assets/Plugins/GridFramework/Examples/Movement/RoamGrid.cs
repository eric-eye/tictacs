using UnityEngine;
using GridFramework.Grids;
using GridFramework.Renderers.Rectangular;
using GridFramework.Extensions.Align;

namespace GridFramework.Examples.Movement {
	/// <summary>
	///   Move the player tile by tile, respecting boundaries and obstacles.
	/// </summary>
	/// <remarks>
	///   <para>
	///     This script demonstrates how you can use Grid Framework to store
	///     information about individual tiles apart from the objects they
	///     belong to in a format accessible to all objects in the scene.
	///   </para>
	/// </remarks>
	public class RoamGrid : MonoBehaviour {
#region  Private variables
		/// <summary>
		///   How fast to move.
		/// </summary>
		private readonly float _moveSpeed = 2f;

		/// <summary>
		///   Whether the object will move to.
		/// </summary>
		private Vector3 _goal;

		/// <summary>
		///   Whether the player is in the process of moving.
		/// </summary>
		private bool _isMoving;

		/// <summary>
		///   The grid we use for movement.
		/// </summary>
		private RectGrid _grid;

		/// <summary>
		///   The range of the renderer restricts the size of the grid area.
		/// </summary>
		private Parallelepiped _renderer;
#endregion  // Private variables
		
#region  Callback methods
		void Start() {
			_grid = ForbiddenTiles._grid;
			_renderer = _grid.gameObject.GetComponent<Parallelepiped>();
		
			_grid.AlignTransform(transform);
		}

		void FixedUpdate() {
			if (_isMoving) {
				Move();
			} else {
				PickNext();
			}
		}
#endregion  // Callback methods

#region  Private methods
		/// <summary>
		///   Increment the player's position towards the goal.
		/// </summary>
		private void Move() {
			var t = _moveSpeed * Time.deltaTime;
			var position = transform.position;

			position.x = Mathf.MoveTowards(transform.position.x, _goal.x, t);
			position.y = Mathf.MoveTowards(transform.position.y, _goal.y, t);

			transform.position = position;

			// Check if we reached the destination (use a certain tolerance so
			// we don't miss the point becase of rounding errors)
			var deltaX = Mathf.Abs(transform.position.x - _goal.x);
			var deltaY = Mathf.Abs(transform.position.y - _goal.y);
			if( deltaX < 0.01f && deltaY < 0.01f) {
				_isMoving = false;
			}
		}

		/// <summary>
		///   Pick the next goal based on user input. The input if filtered
		///   through a number for criteria.
		/// </summary>
		private void PickNext() {
			Vector3 direction;  // Direction to move in (grid-coordinates)

			var h = Input.GetAxisRaw("Horizontal");
			var v = Input.GetAxisRaw("Vertical");

			if (h > 0) {
				direction = Vector3.right;
			} else if (h < 0) {
				direction = Vector3.left;
			} else if (v > 0) {
				direction = Vector3.up;
			} else if (v < 0) {
				direction = Vector3.down;
			} else {
				return;
			}

			// We will be operating in grid space, so convert the position
			_goal = _grid.WorldToGrid(transform.position) + direction;

			// Check that the target is still inside the rendered region of the
			// grid.
			for(var i = 0; i < 2; ++i){
				var beyondLower = _goal[i] < _renderer.From[i];
				var beyondUpper = _goal[i] > _renderer.To[i];

				if(beyondLower || beyondUpper) {
					Debug.Log("I can't swim.");
					return;
				}
			}

			// Check for walls
			if (!ForbiddenTiles.CheckTile(_goal)) {
				Debug.Log("Ouch!");
				return;
			}

			_goal = _grid.GridToWorld(_goal);
			_isMoving = true;
		}
#endregion  // Private methods
	}
}
