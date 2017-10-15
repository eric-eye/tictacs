// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER
using UnityEngine;
using GridFramework.Grids;
using GFGrid = GridFramework.Grids.Grid;
using FsmStateAction = HutongGames.PlayMaker.FsmStateAction;

namespace HutongGames.PlayMaker.Actions {
	[ActionCategory( "Grid Framework" )]
	/// <summary>
	///   Abstract base class for all Playmaker actions involving Grid
	///   Framework.
	/// </summary>
	public abstract class FsmGFStateAction<T> : FsmStateAction where T: MonoBehaviour {
#region  Public variables
		/// <summary>
		///   The GameObject that carries the grid this action will refer to.
		/// </summary>
		[RequiredField]
		[CheckForComponent(typeof(GFGrid))]
		[Tooltip("GameObject that carries the grid, defaults to the owner of the FSM.")]
		public FsmOwnerDefault _gridGameObject;

		/// <summary>
		///   Whether to run the action every frame.
		/// </summary>
		/// <remarks>
		///   <para>
		///     If the action is running every frame it will never call the
		///     <c>Finish()</c> method.
		///   </para>
		/// </remarks>
		public bool _everyFrame;
#endregion  // Cache variables

#region  Protected variables
		/// <summary>
		///   The actual grid component used for all actions.
		/// </summary>
		protected T _t;
#endregion  // Protected variables

#region  Abstract methods
		/// <summary>
		///   This is where the action itself is performed.
		/// </summary>
		protected abstract void DoAction();
#endregion  // Abstract methods

#region  Overridden methods
		public override void Reset() {
			_everyFrame = false;
		}

		public override void OnEnter() {
			if (!SetupCaches())
				return;
			DoAction();

			if (!_everyFrame) {
				Finish();
			}
		}

		public override void OnUpdate() {
			DoAction();
		}
#endregion  // Overridden methods

#region  Protected methods
		/// <summary>
		///   Makes sure the <c>_grid</c> is set to something.
		/// </summary>
		/// <remarks>
		///   <para>
		///     The method assigns a grid component to the <c>_grid</c>
		///     instance variable. If is fails in finding the component it will
		///     return <c>false</c>, preventing null exceptions.
		///   </para>
		///   <para>
		///     First the method tries if there is already a variable to the
		///     grid component. If not, then it tries to find a component on
		///     the given <c>gameObject</c> (by default the owner). If yes,
		///     then it uses that.
		///   </para>
		/// </remarks>
		protected bool SetupCaches() {
			var go = Fsm.GetOwnerDefaultTarget(_gridGameObject);
			_t = go.GetComponent<T>();
			return _t != null;
		}
#endregion  // Protected methods
	}
}
#endif // GRID_FRAMEWORK_PLAYMAKER
