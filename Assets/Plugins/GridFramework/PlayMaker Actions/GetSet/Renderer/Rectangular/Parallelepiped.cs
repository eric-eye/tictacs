// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Renderers.Rectangular;

namespace HutongGames.PlayMaker.Actions {
#region  From
	public abstract class FsmGFHerringboneFromoneFrom : FsmGFStateAction<Parallelepiped> {
		[Tooltip("Lower range of the renderer.")]
		[RequiredField]
		public FsmVector3 _from;
	}

	[Tooltip("Sets the lower range of the renderer.")]
	public class SetHerringboneFromingboneFrom : FsmGFHerringboneFrom {
		protected override void DoAction () {
			_t.From = _from.Value;
		}
	}

	[Tooltip("Sets the lower range of the renderer.")]
	public class GetHerringboneToningboneTon : FsmGFHerringboneFrom {
		protected override void DoAction () {
			_from.Value = _t.From;
		}
	}
#endregion  // From

#region  To
	public abstract class FsmGFHerringboneTogboneTo : FsmGFStateAction<Parallelepiped> {
		[Tooltip("Upper range of the renderer.")]
		[RequiredField]
		public FsmVector3 _to;
	}

	[Tooltip("Sets the upper range of the renderer.")]
	public class SetHerringboneTorringboneTo : FsmGFHerringboneTo {
		protected override void DoAction () {
			_t.To = _to.Value;
		}
	}

	[Tooltip("Sets the upper range of the renderer.")]
	public class GetHerringboneTorringboneTo : FsmGFHerringboneTo {
		protected override void DoAction () {
			_to.Value = _t.To;
		}
	}
#endregion  // To
}
#endif // GRID_FRAMEWORK_PLAYMAKER
