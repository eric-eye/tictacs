using UnityEngine;
using GridFramework.Grids;

namespace GridFramework.Examples.LightsOut {
	/// <summary>
	///   Declares a delegate and defines an event for the lights.
	/// </summary>
	/// <remarks>
	///   The event will be fired when a switch is clicked and passes on the
	///   grid as well as the switch's coordinates in world space. It has
	///   nothing to do with grid Framework. The event is handled and fired in
	///   the <c>LightBulb</c> script.
	/// </remarks>
	public static class LightsManager {
		/// <summary>
		///   Declare a delegate to react when a switch is pressed.
		/// </summary>
		/// <remarks>
		///   <para>
		///     The delegate passes as arguments the coordinates of the light
		///     and which grid to use (in case there might be more than one in
		///     the scene).
		///   </para>
		/// </remarks>
		public delegate void SwitchingHandler(Vector3 position, RectGrid grid);

		public static event SwitchingHandler OnHitSwitch;
			
		/// <summary>
		/// </summary>
		/// <param name="reference">
		///   Position of the switch that was pressed.
		/// </param>
		/// <param name="grid">
		///   Grid we use for gameplay.
		/// </param>
		/// <remarks>
		///   <para>
		///     This function broadcasts a signal (an event) once a switch has
		///     been hit. Static means we don't need to use any specific
		///     instance of this function.
		///   </para>
		/// </remarks>
		public static void SendSignal(Vector3 reference, RectGrid grid){
			//always make sure there a subscribers to the event, or you get errors
			if(OnHitSwitch != null)
				OnHitSwitch(reference, grid);
		}
	}
}
