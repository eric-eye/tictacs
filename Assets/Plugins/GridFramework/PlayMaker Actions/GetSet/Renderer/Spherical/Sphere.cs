// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Renderers.Spherical;

namespace HutongGames.PlayMaker.Actions {
#region  AltFrom
	public abstract class FsmGFSphereAltFrom : FsmGFStateAction<Sphere> {
		[Tooltip("Lower range of the renderer's altitude.")]
		[RequiredField]
		public FsmFloat _from;
	}

	[Tooltip("Sets the lower range of the renderer's altitude.")]
	public class SetSphereAltFrom : FsmGFSphereAltFrom {
		protected override void DoAction () {
			_t.AltFrom = _from.Value;
		}
	}

	[Tooltip("Gets the lower range of the renderer's altitude.")]
	public class GetSphereAltFrom : FsmGFSphereAltFrom {
		protected override void DoAction () {
			_from.Value = _t.AltFrom;
		}
	}
#endregion  // AltFrom

#region  AltTo
	public abstract class FsmGFSphereAltTo : FsmGFStateAction<Sphere> {
		[Tooltip("Lower range of the renderer's altitude.")]
		[RequiredField]
		public FsmFloat _from;
	}

	[Tooltip("Sets the lower range of the renderer's altitude.")]
	public class SetSphereAltTo : FsmGFSphereAltTo {
		protected override void DoAction () {
			_t.AltTo = _from.Value;
		}
	}

	[Tooltip("Gets the lower range of the renderer's altitude.")]
	public class GetSphereAltTo : FsmGFSphereAltTo {
		protected override void DoAction () {
			_from.Value = _t.AltTo;
		}
	}
#endregion  // AltTo

#region  LonFrom
	public abstract class FsmGFSphereLonFrom : FsmGFStateAction<Sphere> {
		[Tooltip("Lower range of the renderer's longitude.")]
		[RequiredField]
		public FsmFloat _from;
	}

	[Tooltip("Sets the lower range of the renderer's longitude.")]
	public class SetSphereLonFrom : FsmGFSphereLonFrom {
		protected override void DoAction () {
			_t.LonFrom = _from.Value;
		}
	}

	[Tooltip("Gets the lower range of the renderer's longitude.")]
	public class GetSphereLonFrom : FsmGFSphereLonFrom {
		protected override void DoAction () {
			_from.Value = _t.LonFrom;
		}
	}
#endregion  // LonFrom

#region  LonTo
	public abstract class FsmGFSphereLonTo : FsmGFStateAction<Sphere> {
		[Tooltip("Lower range of the renderer's longitude.")]
		[RequiredField]
		public FsmFloat _from;
	}

	[Tooltip("Sets the lower range of the renderer's longitude.")]
	public class SetSphereLonTo : FsmGFSphereLonTo {
		protected override void DoAction () {
			_t.LonTo = _from.Value;
		}
	}

	[Tooltip("Gets the lower range of the renderer's longitude.")]
	public class GetSphereLonTo : FsmGFSphereLonTo {
		protected override void DoAction () {
			_from.Value = _t.LonTo;
		}
	}
#endregion  // LonTo

#region  LatFrom
	public abstract class FsmGFSphereLatFrom : FsmGFStateAction<Sphere> {
		[Tooltip("Lower range of the renderer's latitude.")]
		[RequiredField]
		public FsmFloat _from;
	}

	[Tooltip("Sets the lower range of the renderer's latitude.")]
	public class SetSphereLatFrom : FsmGFSphereLatFrom {
		protected override void DoAction () {
			_t.LatFrom = _from.Value;
		}
	}

	[Tooltip("Gets the lower range of the renderer's latitude.")]
	public class GetSphereLatFrom : FsmGFSphereLatFrom {
		protected override void DoAction () {
			_from.Value = _t.LatFrom;
		}
	}
#endregion  // LatFrom

#region  LatTo
	public abstract class FsmGFSphereLatTo : FsmGFStateAction<Sphere> {
		[Tooltip("Lower range of the renderer's latitude.")]
		[RequiredField]
		public FsmFloat _from;
	}

	[Tooltip("Sets the lower range of the renderer's latitude.")]
	public class SetSphereLatTo : FsmGFSphereLatTo {
		protected override void DoAction () {
			_t.LatTo = _from.Value;
		}
	}

	[Tooltip("Gets the lower range of the renderer's latitude.")]
	public class GetSphereLatTo : FsmGFSphereLatTo {
		protected override void DoAction () {
			_from.Value = _t.LatTo;
		}
	}
#endregion  // LatTo
}
#endif // GRID_FRAMEWORK_PLAYMAKER
