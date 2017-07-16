using UnityEngine;

namespace GridFramework.Examples.Endless3D {
	/// <summary>
	///   A simple script that makes an object orbit around its parent.
	/// </summary>
	public class Orbiting : MonoBehaviour {
		/// <summary>
		///   Length of a "year" in Earth days.
		/// </summary>
		public float _orbitalPeriod = 365.0f;
		
		/// <summary>
		///   Scale the orbital period by this.
		/// </summary>
		private const float _velocityScale = 100.0f;

		void Update () {
			if (!transform.parent) {
				return;
			}

			var position = transform.position;
			var parentPosition = transform.parent.position;

			var a = (Time.time % 365.0f) * _velocityScale / _orbitalPeriod;
			var d = Vector3.Magnitude(position - parentPosition);

			var deltaPosition = new Vector3(Mathf.Sin(a), 0.0f, Mathf.Cos(a)) * d;

			// Please forgive me for using a circular orbit instead of an
			// elliptical one.
			transform.position = parentPosition + deltaPosition;
		}
	}
}
