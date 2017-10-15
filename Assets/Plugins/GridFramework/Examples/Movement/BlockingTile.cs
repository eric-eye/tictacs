using UnityEngine;

namespace GridFramework.Examples.Movement {
	/// <summary>
	///   Set up this object as an obstacle tile.
	/// </summary>
	public class BlockingTile : MonoBehaviour {
		// Start() is called after Awake(), this ensures that the matrix has
		// already been built.
		void Start() {
			// Set the entry that corresponds to the obstacle's position as
			// false.
			ForbiddenTiles.SetTile(transform.position, false);
		}
	}
}
