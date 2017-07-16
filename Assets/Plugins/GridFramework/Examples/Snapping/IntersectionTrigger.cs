using UnityEngine;

namespace GridFramework.Examples.Snapping {
	/// <summary>
	///   Behaviour that tells the snapping behaviour that the GameObject is
	///   intersecting another one.
	/// </summary>
	public class IntersectionTrigger : MonoBehaviour {
		private SnappingUnits snappingScript;

		void OnTriggerEnter(Collider other) {
			snappingScript.SetIntersecting(true);
		}
		
		void OnTriggerExit(Collider other) {
			snappingScript.SetIntersecting(false);
		}
		
		public void SetSnappingScript(SnappingUnits script) {
			snappingScript = script;
		}
	}
}
