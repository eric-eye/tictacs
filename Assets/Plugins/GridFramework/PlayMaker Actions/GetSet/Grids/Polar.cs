// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Grids;

namespace HutongGames.PlayMaker.Actions {
#region  Radius
	public abstract class FsmGFPRadius : FsmGFStateAction<PolarGrid> {
		[Tooltip("Radius of the polar grid.")]
		[RequiredField]
		public FsmFloat _radius;
	}

	[Tooltip("Sets the radius of the polar grid.")]
	public class SetPolarRadius : FsmGFPRadius {
		protected override void DoAction () {
			_t.Radius = _radius.Value;
		}
	}

	[Tooltip("Gets the radius of the polar grid.")]
	public class GetPolarRadius : FsmGFPRadius {
		protected override void DoAction () {
			_radius.Value = _t.Radius;
		}
	}
#endregion  // Radius

#region  Sectors
	public abstract class FsmGFPSectors : FsmGFStateAction<PolarGrid> {
		[Tooltip("Amount of sectors in the polar grid.")]
		[RequiredField]
		public FsmInt _sectors;
	}

	[Tooltip("Sets the amount of sectors in the polar grid.")]
	public class SetSectors : FsmGFPSectors {
		protected override void DoAction () {
			if( _sectors.Value < 1 )
				_sectors.Value = 1;
			_t.Sectors = _sectors.Value;
		}
	}

	[Tooltip("Gets the amount of sectors in the polar grid.")]
	public class GetSectors : FsmGFPSectors {
		protected override void DoAction () {
			_sectors.Value = _t.Sectors;
		}
	}
#endregion  // Sectors

#region  Radians
	[Tooltip("Gets the angle between two sectors in the polar grid (in radians).")]
	public class FsmGFPRadians : FsmGFStateAction<PolarGrid> {
		[Tooltip("Angle between two sectors in the polar grid (in radians).")]
		public FsmFloat _radians;
		protected override void DoAction () {
			_radians.Value = _t.Radians;
		}
	}
#endregion  // Radians

#region  Degrees
	[Tooltip("Gets the angle between two sectors in the polar grid (in degrees).")]
	public class FsmGFPAngleDeg : FsmGFStateAction<PolarGrid> {
		[Tooltip("Angle between two sectors in the polar grid (in degrees).")]
		public FsmFloat _degrees;
		protected override void DoAction () {
			_degrees.Value = _t.Degrees;
		}
	}
#endregion  // Degrees

#region  Rotation
	[Tooltip("Gets the rotation between two sectors in the polar grid.")]
	public class FsmGFPRotation : FsmGFStateAction<PolarGrid> {
		[Tooltip("Rotation between two sectors in the polar grid.")]
		public FsmQuaternion _rotation;
		protected override void DoAction () {
			_rotation.Value = _t.Rotation;
		}
	}
#endregion  // Rotation

#region  Right
	[Tooltip("Gets a vector with polar coordinates (1, 0, 0).")]
	public class FsmGFPRight : FsmGFStateAction<PolarGrid> {
		[Tooltip("Polar coordinates (1, 0, 0) in world-space.")]
		public FsmVector3 _right;
		protected override void DoAction () {
			_right.Value = _t.Right;
		}
	}
#endregion  // Right
}
#endif  // GRID_FRAMEWORK_PLAYMAKER
