// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Renderers.Hexagonal;

// Missing: Shift (impossible)

namespace HutongGames.PlayMaker.Actions {
#region  From
	public abstract class FsmGFHerringboneFrom : FsmGFStateAction<Herringbone> {
		[Tooltip("Lower range of the renderer.")]
		[RequiredField]
		public FsmVector3 _from;
	}

	[Tooltip("Sets the lower range of the renderer.")]
	public class SetHerringboneFrom : FsmGFHerringboneFrom {
		protected override void DoAction () {
			_t.From = _from.Value;
		}
	}

	[Tooltip("Sets the lower range of the renderer.")]
	public class GetHerringboneFromom : FsmGFHerringboneFrom {
		protected override void DoAction () {
			_from.Value = _t.From;
		}
	}
#endregion  // From

#region  To
	public abstract class FsmGFHerringboneTo : FsmGFStateAction<Herringbone> {
		[Tooltip("Upper range of the renderer.")]
		[RequiredField]
		public FsmVector3 _to;
	}

	[Tooltip("Sets the upper range of the renderer.")]
	public class SetHerringboneTo : FsmGFHerringboneTo {
		protected override void DoAction () {
			_t.To = _to.Value;
		}
	}

	[Tooltip("Sets the upper range of the renderer.")]
	public class GetHerringboneFrom : FsmGFHerringboneTo {
		protected override void DoAction () {
			_to.Value = _t.To;
		}
	}
#endregion  // To
}
#endif // GRID_FRAMEWORK_PLAYMAKER
