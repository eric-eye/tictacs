using UnityEngine;

namespace GridFramework.Examples.Endless3D {
	/// <summary>
	///   A simple 3D control script with six degrees of freedom.
	/// </summary>
	public class SpaceFlight : MonoBehaviour {
		private const float _linearVelocity  = 0.25f;
		private const float _angularVelocity = 1.0f;
		
		private Vector3    _originalPosition;
		private Quaternion _originalRotation;

		void Start() {
			_originalPosition = transform.position;
			_originalRotation = transform.rotation;
		}

		void Update() {
			var linear  = Vector3.zero;
			var angular = Vector3.zero;

			// Rotation in Euler angles
			var euler = transform.rotation.eulerAngles;

			// Input is hard-coded to keys, not a good idea, but makes the
			// script portable across different projects.

			linear.x += Input.GetKey("d"    ) ?  1.0f : 0.0f;
			linear.x += Input.GetKey("a"    ) ? -1.0f : 0.0f;
			linear.y += Input.GetKey("space") ?  1.0f : 0.0f;
			linear.y += Input.GetKey("x"    ) ? -1.0f : 0.0f;
			linear.z += Input.GetKey("w"    ) ?  1.0f : 0.0f;
			linear.z += Input.GetKey("s"    ) ? -1.0f : 0.0f;

			angular.x += Input.GetKey("r") ?  1.0f : 0.0f;
			angular.x += Input.GetKey("f") ? -1.0f : 0.0f;
			angular.y += Input.GetKey("c") ?  1.0f : 0.0f;
			angular.y += Input.GetKey("z") ? -1.0f : 0.0f;
			angular.z += Input.GetKey("q") ?  1.0f : 0.0f;
			angular.z += Input.GetKey("e") ? -1.0f : 0.0f;

			transform.position += linear.x * _linearVelocity * transform.right;
			transform.position += linear.y * _linearVelocity * transform.up;
			transform.position += linear.z * _linearVelocity * transform.forward;
			euler += angular * _angularVelocity;
			transform.rotation = Quaternion.Euler(euler);

			// Reset key
			if (Input.GetKey("backspace")) {
				transform.position = _originalPosition;
				transform.rotation = _originalRotation;
			}
		}

		void OnGUI() {
			const string message = "Fly around using the following controls:\n"
				+ "WASD - sideways and forward\n"
				+ "Space & X - Up & Down\n"
				+ "\n"
				+ "R & F - Pitch\n"
				+ "Q & E - Roll\n"
				+ "Z & C - Yaw\n"
				+ "\n"
				+ "Backspace - Reset";
			GUI.TextArea (new Rect (10, 10, 300, 150), message);
		}
	}
}
