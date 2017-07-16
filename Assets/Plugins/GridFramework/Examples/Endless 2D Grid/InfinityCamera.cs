using UnityEngine;
using GridFramework.Renderers.Rectangular;

namespace GridFramework.Examples.Endless2D {
	/// <summary>
	///   In this example we adjust the grid's rendering range at runtime to the
	///   view rectangle or the main camera.
	/// </summary>
	/// <remarks>
	///   <para>
	///     Rendering a very large amount of grid lines can get expensive, and
	///     when the player will only ever see just a small portion of the grid
	///     it's a complete waste of resources. We will instead fake the
	///     illusion of an infinitely large grid by dynamically adjusting the
	///     rendering range as the camera is moved.
	///   </para>
	///   <para>
	///     The view rectangle of an orthographic camera is defined by the
	///     camera's orthographic size and its aspect ratio. The orthographic
	///     size is the height of the rectangle in world units and the width is
	///     computed by multiplying the height with the aspect ratio.
	///   </para>
	///   <code>
	///     width = height * aspectRatio
	///   </code>
	///   <para>
	///     The position of the centre of the rectangle is the position of the
	///     camera in the world. We need the lower left-hand and upper
	///     right-hand corners of the rectangle as the rendering range of out
	///     grid.
	///   </para>
	///   <para>
	///     We will go slightly beyond the range of the camera for optimisation
	///     purposed. Every time the range is changed the grid lines need to be
	///     re-calculated. By going slightly beyond the view rectangle we give
	///     the camera an are of tolerance where it can move without triggering
	///     re-calculation.
	///   </para>
	/// </remarks>
	[RequireComponent(typeof(Camera))]
	public class InfinityCamera : MonoBehaviour {
#region  Public variables
		/// <summary>
		///   The renderer of the seemingly infinite grid we want to resize
		///   dynamically during gameplay.
		/// </summary>
		[SerializeField]
		public Parallelepiped _renderer;
		
		/// <summary>
		///   How much buffer do we want to use?
		/// </summary>
		/// More buffer means more to render each time, but less frequent recalculations.
		public float _buffer = 10.0f;
		
		/// <summary>
		///   Speed of the camera.
		/// </summary>
		public float _cameraSpeed = 5.0f;
		
		/// <summary>
		///   Boost factor.
		/// </summary>
		public float _speedBoost =  2.0f;
	
		/// <summary>
		///   Boost key to hold down.
		/// </summary>
		public string _boostButton = "left shift";
#endregion  // Public variables

#region  Private variables
		// use these internally to decide when to recalculate
		private Vector3 _lastPosition;
		private Camera _cam;
#endregion  // Private variables
		
#region  Callback methods
		void Start() {
			_lastPosition = transform.position;
			
			// Make sure the camera is orthographic (we will use an
			// orthographic camera here because it is simpler)
			_cam = GetComponent<Camera>();
			_cam.orthographic = true;
			
			// Set the grid's range according to the camera's size plus the
			// buffer we want to use.  We scale the buffer with 1.1 to make it
			// a little bit larger, just in case.
			var camX = _cam.aspect * _cam.orthographicSize + 1.1f * _buffer;
			var camY = _cam.orthographicSize + 1.1f * _buffer;
			var posX = transform.position.x;
			var posY = transform.position.y;
			// we set the range relative to the camera's position
			_renderer.From = new Vector3(posX - camX, posY - camY, 0);
			_renderer.To   = new Vector3(posX + camX, posY + camY, 0);
		}
		
		void Update() {
			Move();
			
			// if we exceeded the buffer limit let's recalculate the points for rendering
			var beyondX = Mathf.Abs(transform.position.x - _lastPosition.x) >= _buffer;
			var beyondY = Mathf.Abs(transform.position.y - _lastPosition.y) >= _buffer;
			if (beyondX || beyondY) {
				ResizeGrid();
			}
		}
		
		// display some information
		void OnGUI() {
			var lastX = _lastPosition.x;
			var lastY = _lastPosition.y;
			var currX = transform.position.x;
			var currY = transform.position.y;

			var message =
				"The current camera position is\n   " + currX +" / " + currY + "\n " +
				"and the last fixed position was\n   " + lastX +" / "+ lastY;
			var rect = new Rect (10, 10, 200, 70);
			GUI.TextArea(rect, message);
		}
#endregion  // Callback methods
		
#region  Private methods
		private void ResizeGrid () {
			Vector3 shift = Vector3.zero;
			for (var i = 0; i < 2; ++i) {
				var current = transform.position[i];
				var last    = _lastPosition[i];

				shift[i] += current - last;
			}
			
			// Add the shift to both values
			_renderer.From += shift;
			_renderer.To   += shift;
			
			_lastPosition = transform.position;
		}
		
		/// <summary>
		///   A simple method for handling movement using the arrow keys (hold
		///   <c>boostButton</c> to move faster).
		/// </summary>
		private void Move(){
			var h = Input.GetAxis("Horizontal");
			var v = Input.GetAxis("Vertical");
			var boost = Input.GetKey(_boostButton) ? _speedBoost : 1;

			var move = new Vector3(h, v, 0) * _cameraSpeed * boost * Time.deltaTime;

			transform.position += move;
		}
#endregion  // Private methods
	}
}
