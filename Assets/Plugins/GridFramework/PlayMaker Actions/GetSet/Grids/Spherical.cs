// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Grids;

namespace HutongGames.PlayMaker.Actions {
#region  Radius
	public abstract class FsmGFSRadius : FsmGFStateAction<SphereGrid> {
		[Tooltip("Radius of the spherical grid.")]
		[RequiredField]
		public FsmFloat _radius;
	}

	[Tooltip("Sets the radius of the spherical grid.")]
	public class SetSphereRadius : FsmGFSRadius {
		protected override void DoAction () {
			_t.Radius = _radius.Value;
		}
	}

	[Tooltip("Gets the radius of the spherical grid.")]
	public class GetSphereRadius : FsmGFSRadius {
		protected override void DoAction () {
			_radius.Value = _t.Radius;
		}
	}
#endregion  // Radius

#region  Parallels
	public abstract class FsmGFSParallels : FsmGFStateAction<SphereGrid> {
		[Tooltip("Radius of the spherical grid.")]
		[RequiredField]
		public FsmInt _parallels;
	}

	[Tooltip("Sets the radius of the spherical grid.")]
	public class SetSphereParallels : FsmGFSParallels {
		protected override void DoAction () {
			_t.Parallels = _parallels.Value;
		}
	}

	[Tooltip("Gets the radius of the spherical grid.")]
	public class GetSphereParallels : FsmGFSParallels {
		protected override void DoAction () {
			_parallels.Value = _t.Parallels;
		}
	}
#endregion  // Parallels

#region  Meridians
	public abstract class FsmGFSMeridians : FsmGFStateAction<SphereGrid> {
		[Tooltip("Radius of the spherical grid.")]
		[RequiredField]
		public FsmInt _meridians;
	}

	[Tooltip("Sets the radius of the spherical grid.")]
	public class SetSphereMeridians : FsmGFSMeridians {
		protected override void DoAction () {
			_t.Meridians = _meridians.Value;
		}
	}

	[Tooltip("Gets the radius of the spherical grid.")]
	public class GetSphereMeridians : FsmGFSMeridians {
		protected override void DoAction () {
			_meridians.Value = _t.Meridians;
		}
	}
#endregion  // Meridians

#region  Polar
	[Tooltip("Gets the angle between two parallels in the sphere grid (in radians).")]
	public class FsmGFSPolar : FsmGFStateAction<SphereGrid> {
		[Tooltip("Angle between two parallels in the sphere grid (in radians).")]
		public FsmFloat _polar;
		protected override void DoAction () {
			_polar.Value = _t.Polar;
		}
	}
#endregion  // Polar

#region  PolarDeg
	[Tooltip("Gets the angle between two parallels in the sphere grid (in degrees).")]
	public class FsmGFSPolarDeg : FsmGFStateAction<SphereGrid> {
		[Tooltip("Angle between two parallels in the sphere grid (in degrees).")]
		public FsmFloat _polarDeg;
		protected override void DoAction () {
			_polarDeg.Value = _t.PolarDeg;
		}
	}
#endregion  // PolarDeg

#region  Azimuth
	[Tooltip("Gets the angle between two meridians in the sphere grid (in radians).")]
	public class FsmGFSAzimuth : FsmGFStateAction<SphereGrid> {
		[Tooltip("Angle between two meridians in the sphere grid (in radians).")]
		public FsmFloat _azimuth;
		protected override void DoAction () {
			_azimuth.Value = _t.Azimuth;
		}
	}
#endregion  // Azimuth

#region  AzimuthDeg
	[Tooltip("Gets the angle between two meridians in the sphere grid (in degrees).")]
	public class FsmGFSAzimuthDeg : FsmGFStateAction<SphereGrid> {
		[Tooltip("Angle between two meridians in the sphere grid (in degrees).")]
		public FsmFloat _azimuthDeg;
		protected override void DoAction () {
			_azimuthDeg.Value = _t.AzimuthDeg;
		}
	}
#endregion  // AzimuthDeg
}
#endif  // GRID_FRAMEWORK_PLAYMAKER
