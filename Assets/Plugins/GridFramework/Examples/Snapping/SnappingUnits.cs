using UnityEngine;
using GridFramework.Grids;
using GridFramework.Extensions.Align;


namespace GridFramework.Examples.Snapping {
	/// <summary>
	///   Make objects stick to the ground of a grid, similar to how buildings
	///   are placed in strategy games.
	/// </summary>
	/// <remarks>
	///   <para>
	///     Objects will always be placed with the bottom touching the grid.
	///     Mouse input is being handled by shooting a ray from the cursor and
	///     letting it hit the grid's collider. If there is no collider nothing
	///     will happen. In this example I used a plane collider on the grid.
	///   </para>
	///   <para>
	///     This script demonstrates the snapping feature during runtime and
	///     conversions from world space to grid space and back.
	///   </para>
	///   <para>
	///     Due to popular request this example now also detects when the block
	///     is intersecting with another block. In that case the active block
	///     will be tinted red until you move out. If you let go while
	///     intersecting it will snap back to the last non-intersecting
	///     position. This is achieved by using a child object with a trigger
	///     that checks for intersections and reports back to this script. This
	///     is just standard unity physics and has nothing to do with Grid
	///     Framework itself, but it is still a question people ask often.
	///   </para>
	/// </remarks>
	[RequireComponent(typeof(Collider))]
	public class SnappingUnits : MonoBehaviour {
#region  Public variables
		/// <summary>
		///   The grid we want to snap to.
		/// </summary>
		public RectGrid _grid; 

		/// <summary>
		///   Default material to use.
		/// </summary>
		public Material _defaultMaterial;

		/// <summary>
		///   Material to use when blocks are intersecting.
		/// </summary>
		public Material _redMaterial;
#endregion  // Public variables

#region  Private variables
		/// <summary>
		///   A collider attached to the grid, will be used for handling mouse input.
		/// </summary>
		private Collider gridCollider;
		
		/// <summary>
		///   True while the player is dragging the block around, otherwise false.
		/// </summary>
		private bool beingDragged;

		/// <summary> 
		///   The previous valid position.
		/// </summary>
		private Vector3 oldPosition;

		/// <summary> 
		///   This keeps track of how many other cubes we are intersecting with.
		/// </summary> 
		private int _intersecting;
#endregion  // Private variables

#region  Callback methods
		void Awake() {
			gridCollider = _grid.gameObject.GetComponent<Collider>();
			// Perform an initial align and snap the objects to the bottom.
			_grid.AlignTransform(transform);
			transform.position = CalculateOffsetY ();
			// Store the first safe position.
			oldPosition = transform.position;
			// Setup the rigidbody for collision and construct a trigger.
			SetupRigidbody();
			ConstructTrigger();
		}
		
		void OnMouseDown() {
			beingDragged = true;
		}

		void OnMouseUp() {
			beingDragged = false;
			transform.position = oldPosition;
			_intersecting = 0;
			TintRed(_intersecting);
		}
		
		// Use FixedUpdate instead of Update to allow collision detection to
		// catch up with movement.
		void FixedUpdate() {
			if (beingDragged) {
				// Store the position if it is valid
				if (_intersecting == 0) {
					oldPosition = transform.position;
				}
				DragObject();
			}
		}
#endregion  // Callback methods
		
#region  Private methods
		/// <summary>
		///   This function gets called every frame while the object (its
		///   collider) is being dragged with the mouse.
		/// </summary>
		private void DragObject() {
			if(!_grid || !gridCollider) {
				return;
			}

			// Handle mouse input to convert it to world coordinates
			var cursorWorldPoint = ShootRay();
    		
			// Change the X and Z coordinates according to the cursor (the Y
			// coordinate stays the same after the last step).
			transform.position = cursorWorldPoint;
			
			// Now align the object and snap it to the bottom.
			_grid.AlignTransform(transform);
			transform.position = CalculateOffsetY(); // this forces the Y-coordinate into position
		}
		
		/// <summary>
		///   Makes the object snap to the bottom of the grid, respecting the
		///   grid's rotation.
		/// </summary>
		private Vector3 CalculateOffsetY() {
			//first store the objects position in grid coordinates
			var gridPosition = _grid.WorldToGrid(transform.position);
			//then change only the Y coordinate
			gridPosition.y = .5f * transform.lossyScale.y;
			
			//convert the result back to world coordinates
			return _grid.GridToWorld(gridPosition);
		}

		/// <summary>
		///   Shoots a ray, which can only hit the grid plane, from the mouse
		///   cursor via the camera and returns the hit position.
		/// </summary>
		private Vector3 ShootRay() {
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;
			gridCollider.Raycast(ray, out hit, Mathf.Infinity);

			// This is where the player's cursor is pointing towards (if
			// nothing was hit return the current position => no movement)
			return hit.collider != null ? hit.point : transform.position;
		}

		// this method will be called by the trigger
		public void SetIntersecting(bool intersecting) {
			// ignore sitting objects, only moving ones should respond
			if(!beingDragged) {
				return;
			}

			if (intersecting) {
				++_intersecting;
			} else {
				--_intersecting;
			}

			TintRed(_intersecting);
		}

		/// <summary>
		///   Tint the block red if intersecting.
		/// </summary>
		void TintRed(int intersections) {
			var _renderer = gameObject.GetComponent<Renderer>();
			var material = intersections > 0 ? _redMaterial : _defaultMaterial;

			_renderer.material = material;
		}

		/// <summary>
		///   Set up the rigidbody component for intersection recognition.
		/// </summary>
		private void SetupRigidbody() {
			const RigidbodyConstraints constraints = RigidbodyConstraints.FreezeAll;

			var rb = GetComponent<Rigidbody>();
			if(!rb) {
				rb = gameObject.AddComponent<Rigidbody>();
			}

			// Non-kinematic to allow collision detection, no gravity and all
			// rotations and movement frozen (prevents physics from moving the
			// object).
			rb.isKinematic = false;
			rb.useGravity  = false;
			rb.constraints = constraints;
		}

		/// <summary>
		///   Create a child GameObject and add a trigger to it to do the
		///   intersection detection.
		/// </summary>
		private void ConstructTrigger() {
			var go  = new GameObject();
			var col = (Collider)go.AddComponent(GetComponent<Collider>().GetType());

			go.name = "IntersectionTrigger";
			// attach it to this block and make it exactly the same, except slightly smaller
			go.transform.parent = transform;
			go.transform.localPosition = Vector3.zero; //exactly at the centre of the actual object
			go.transform.localScale = 0.9f * Vector3.one; //slightly smaller than the actual object
			go.transform.localRotation = Quaternion.identity; // same rotation as the actual object
			col.isTrigger = true;
			// attach the script to the collider and connect it to this script
			IntersectionTrigger script = go.AddComponent<IntersectionTrigger>();

			script.SetSnappingScript(this);
		}
#endregion  // Private methods
	}
}
