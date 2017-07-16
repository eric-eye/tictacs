using UnityEngine;
using GridFramework.Grids;

namespace GridFramework.Examples.LightsOut {
	/// <summary>
	///   The behaviour of every individual light bulb.
	/// </summary>
	[RequireComponent(typeof(SpriteRenderer))]
	public class LightBulb : MonoBehaviour {
#region  Public variables
		/// <summary>
		///   Material to use when the light is *on*.
		/// </summary>
		public Sprite _onSprite;

		/// <summary>
		///   Material to use when the light is *off*.
		/// </summary>
		public Sprite _offSprite;
		
		/// <summary>
		///   Current state of the switch.
		/// </summary>
		/// <remarks>
		///   <para>
		///     The state of the switch (intial set is done in the editor,rest
		///     at runtime).
		///   </para>
		/// </remarks>
		public bool _isOn;
		
		/// <summary>
		///   The grid we want to use for our game logic.
		/// </summary>
		public RectGrid _grid;
#endregion  // Public variables

#region  Callback methods
		void OnEnable() {
			LightsManager.OnHitSwitch += OnHitSwitch;
		}
		
		void OnDisable() {
			LightsManager.OnHitSwitch -= OnHitSwitch;
		}
		
		void OnMouseUpAsButton() {
			LightsManager.SendSignal(transform.position, _grid);
		}
#endregion  // Callback methods

#region  Private methods
		/// <summary>
		///   Gets called upon the event <c>OnHitSwitch</c>.
		/// </summary>
		/// <param name="reference">
		///   Position of the clicked switch in grid coordinates.
		/// </param>
		/// <param name="grid">
		///   The grid we use for reference.
		/// </param>
		private void OnHitSwitch(Vector3 reference, RectGrid grid) {
			// Don't do anything if this light doesn't belong to the grid we
			// use.
			if (grid != _grid) {
				return;
			}
			
			// Check if this light is adjacent to the switch; this is an
			// extenion method that always picks the method that belongs to the
			// specific grid type. The implementation is in another file.
			var isAdjacent = grid.AreAdjacent(transform.position, reference);

			if (isAdjacent) {
				_isOn = !_isOn;
				SwitchLights();
			}
		}
#endregion  // Private methods
		
#region  Public methods
		/// <summary>
		///   Toggles the material of the ligh.
		/// </summary>
		public void SwitchLights() {
			var sr = gameObject.GetComponent<SpriteRenderer>();
			sr.sprite = _isOn ? _onSprite : _offSprite;
		}
#endregion  // Public methods
	}
}
