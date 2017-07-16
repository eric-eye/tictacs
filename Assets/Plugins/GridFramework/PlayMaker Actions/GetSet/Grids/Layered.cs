// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Grids;

namespace HutongGames.PlayMaker.Actions {
#region  Depth
	public abstract class FsmGFLDepth : FsmGFStateAction<LayeredGrid> {
		[Tooltip("Distance between two grid layers.")]
		[RequiredField]
		public FsmFloat _dept;
	}

	[Tooltip("Sets the distance between two grid layers.")]
	public class SetDepth : FsmGFLDepth {
		protected override void DoAction () {
			_t.Depth = _dept.Value;
		}
	}

	[Tooltip("Gets the distance between two grid layers.")]
	public class GetDepth : FsmGFLDepth {
		protected override void DoAction () {
			_dept.Value = _t.Depth;
		}
	}
#endregion  // Depth

#region  Forward
	public abstract class FsmGFLForward : FsmGFStateAction<LayeredGrid> {
		[Tooltip("Vector from one layer to the next one")]
		[RequiredField]
		public FsmVector3 _forward;
	}

	[Tooltip("Gets the distance between two grid layers.")]
	public class LayeredGetForward : FsmGFLForward {
		protected override void DoAction () {
			_forward.Value = _t.Forward;
		}
	}
#endregion  // Forward
}
#endif  // GRID_FRAMEWORK_PLAYMAKER
