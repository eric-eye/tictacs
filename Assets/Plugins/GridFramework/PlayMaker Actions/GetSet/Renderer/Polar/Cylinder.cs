// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Renderers.Polar;

namespace HutongGames.PlayMaker.Actions {
#region  Radial From
	public abstract class FsmGFCylinderRadialFrom : FsmGFStateAction<Cylinder> {
		[Tooltip("Lower radial range of the renderer.")]
		[RequiredField]
		public FsmFloat _value;
	}

	[Tooltip("Sets the lower radial range of the renderer.")]
	public class SetRadialFrom : FsmGFCylinderRadialFrom {
		protected override void DoAction () {
			_t.RadialFrom = _value.Value;
		}
	}

	[Tooltip("Sets the lower radial range of the renderer.")]
	public class GetRadialFrom : FsmGFCylinderRadialFrom {
		protected override void DoAction () {
			_value.Value = _t.RadialFrom;
		}
	}
#endregion  // Radial From

#region  Radial To
	public abstract class FsmGFCylinderRadialTo : FsmGFStateAction<Cylinder> {
		[Tooltip("Upper radial range of the renderer.")]
		[RequiredField]
		public FsmFloat _value;
	}

	[Tooltip("Sets the upper radial range of the renderer.")]
	public class SetRadialTo : FsmGFCylinderRadialTo {
		protected override void DoAction () {
			_t.RadialTo = _value.Value;
		}
	}

	[Tooltip("Sets the upper radial range of the renderer.")]
	public class GetRadialTo : FsmGFCylinderRadialTo {
		protected override void DoAction () {
			_value.Value = _t.RadialTo;
		}
	}
#endregion  // Radial To

#region  Sector From
	public abstract class FsmGFCylinderSectorFrom : FsmGFStateAction<Cylinder> {
		[Tooltip("Lower sector range of the renderer.")]
		[RequiredField]
		public FsmFloat _value;
	}

	[Tooltip("Sets the lower sector range of the renderer.")]
	public class SetSectorFrom : FsmGFCylinderSectorFrom {
		protected override void DoAction () {
			_t.SectorFrom = _value.Value;
		}
	}

	[Tooltip("Sets the lower sector range of the renderer.")]
	public class GetSectorFrom : FsmGFCylinderSectorFrom {
		protected override void DoAction () {
			_value.Value = _t.SectorFrom;
		}
	}
#endregion  // Sector From

#region  Sector To
	public abstract class FsmGFCylinderSectorTo : FsmGFStateAction<Cylinder> {
		[Tooltip("Upper sector range of the renderer.")]
		[RequiredField]
		public FsmFloat _value;
	}

	[Tooltip("Sets the upper sector range of the renderer.")]
	public class SetSectorTo : FsmGFCylinderSectorTo {
		protected override void DoAction () {
			_t.SectorTo = _value.Value;
		}
	}

	[Tooltip("Sets the upper sector range of the renderer.")]
	public class GetSectorTo : FsmGFCylinderSectorTo {
		protected override void DoAction () {
			_value.Value = _t.SectorTo;
		}
	}
#endregion  // Sector To

#region  Layer From
	public abstract class FsmGFCylinderLayerFrom : FsmGFStateAction<Cylinder> {
		[Tooltip("Lower layer range of the renderer.")]
		[RequiredField]
		public FsmFloat _value;
	}

	[Tooltip("Sets the lower layer range of the renderer.")]
	public class SetLayerFrom : FsmGFCylinderLayerFrom {
		protected override void DoAction () {
			_t.LayerFrom = _value.Value;
		}
	}

	[Tooltip("Sets the lower layer range of the renderer.")]
	public class GetLayerFrom : FsmGFCylinderLayerFrom {
		protected override void DoAction () {
			_value.Value = _t.LayerFrom;
		}
	}
#endregion  // Layer From

#region  Layer To
	public abstract class FsmGFCylinderLayerTo : FsmGFStateAction<Cylinder> {
		[Tooltip("Upper layer range of the renderer.")]
		[RequiredField]
		public FsmFloat _value;
	}

	[Tooltip("Sets the upper layer range of the renderer.")]
	public class SetLayerTo : FsmGFCylinderLayerTo {
		protected override void DoAction () {
			_t.LayerTo = _value.Value;
		}
	}

	[Tooltip("Sets the upper layer range of the renderer.")]
	public class GetLayerTo : FsmGFCylinderLayerTo {
		protected override void DoAction () {
			_value.Value = _t.LayerTo;
		}
	}
#endregion  // Layer To
}
#endif // GRID_FRAMEWORK_PLAYMAKER
