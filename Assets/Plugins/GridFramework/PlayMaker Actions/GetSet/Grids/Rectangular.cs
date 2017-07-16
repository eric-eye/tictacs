// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Grids;
// Missing: shearing (impossible)

namespace HutongGames.PlayMaker.Actions {
#region  Spacing
	public abstract class FsmGFRSpacing : FsmGFStateAction<RectGrid> {
		[Tooltip("Spacing of the rectangular grid.")]
		[RequiredField]
		public FsmVector3 _spacing;
	}

	[Tooltip("Sets the spacing of the rectangular grid.")]
	public class SetSpacing : FsmGFRSpacing {
		protected override void DoAction () {
			_t.Spacing = _spacing.Value;
		}
	}

	[Tooltip("Gets the spacing of the rectangular grid.")]
	public class GetSpacing : FsmGFRSpacing {
		protected override void DoAction () {
			_spacing.Value = _t.Spacing;
		}
	}
#endregion  // Spacing

#region  Right
	[Tooltip("Gets the grid's local \"right\" direction scaled by the spacing.")]
	public class GetRight : FsmGFStateAction<RectGrid> {
		[Tooltip("The grid's local \"right\" direction scaled by the spacing.")]
		public FsmVector3 _right;
		protected override void DoAction () {
			_right.Value = _t.Right;
		}
	}
#endregion  // Right

#region  Up
	[Tooltip("Gets the grid's local \"up\" direction scaled by the spacing.")]
	public class GetUp : FsmGFStateAction<RectGrid> {
		[Tooltip("The grid's local \"up\" direction scaled by the spacing.")]
		public FsmVector3 _up;
		protected override void DoAction () {
			_up.Value = _t.Up;
		}
	}
#endregion  // Up

#region  Forward
	[Tooltip("Gets the grid's local \"forward\" direction scaled by the spacing.")]
	public class GetForward : FsmGFStateAction<RectGrid> {
		[Tooltip("The grid's local \"forward\" direction scaled by the spacing.")]
		public FsmVector3 _forward;
		protected override void DoAction () {
			_forward.Value = _t.Forward;
		}
	}
#endregion  // Forward
}

#endif  // GRID_FRAMEWORK_PLAYMAKER
