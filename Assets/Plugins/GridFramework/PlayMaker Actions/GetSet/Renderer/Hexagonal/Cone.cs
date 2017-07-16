// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Renderers.Hexagonal;

namespace HutongGames.PlayMaker.Actions {
#region  OriginY
	public abstract class FsmGFConeOriginY : FsmGFStateAction<Cone> {
		[Tooltip("Y coordinate of the origin of the renderer.")]
		[RequiredField]
		public FsmInt _value;
	}

	[Tooltip("Sets the Y coordinate of the origin of the renderer.")]
	public class SetConeOriginY : FsmGFConeOriginY {
		protected override void DoAction () {
			_t.OriginY = _value.Value;
		}
	}

	[Tooltip("Sets the Y coordinate of the origin of the renderer.")]
	public class GetConeOriginY : FsmGFConeOriginY {
		protected override void DoAction () {
			_value.Value = _t.OriginY;
		}
	}
#endregion  // OriginY

#region  OriginX
	public abstract class FsmGFConeOriginX : FsmGFStateAction<Cone> {
		[Tooltip("X coordinate of the origin of the renderer.")]
		[RequiredField]
		public FsmInt _value;
	}

	[Tooltip("Sets the X coordinate of the origin of the renderer.")]
	public class SetConeOriginX : FsmGFConeOriginX {
		protected override void DoAction () {
			_t.OriginX = _value.Value;
		}
	}

	[Tooltip("Sets the X coordinate of the origin of the renderer.")]
	public class GetConeOriginX : FsmGFConeOriginX {
		protected override void DoAction () {
			_value.Value = _t.OriginX;
		}
	}
#endregion  // OriginX

#region  Radius From
	public abstract class FsmGFConeRadiusFrom : FsmGFStateAction<Cone> {
		[Tooltip("Lower radius range of the renderer.")]
		[RequiredField]
		public FsmInt _value;
	}

	[Tooltip("Sets the lower radius range of the renderer.")]
	public class SetConeRadiusFrom : FsmGFConeRadiusFrom {
		protected override void DoAction () {
			_t.RadiusFrom = _value.Value;
		}
	}

	[Tooltip("Sets the lower radius range of the renderer.")]
	public class GetConeRadiusFrom : FsmGFConeRadiusFrom {
		protected override void DoAction () {
			_value.Value = _t.RadiusFrom;
		}
	}
#endregion  // Radius From

#region  Radius To
	public abstract class FsmGFConeRadiusTo : FsmGFStateAction<Cone> {
		[Tooltip("Upper radius range of the renderer.")]
		[RequiredField]
		public FsmInt _value;
	}

	[Tooltip("Sets the upper radius range of the renderer.")]
	public class SetConeRadiusTo : FsmGFConeRadiusTo {
		protected override void DoAction () {
			_t.RadiusTo = _value.Value;
		}
	}

	[Tooltip("Sets the upper radius range of the renderer.")]
	public class GetConeRadiusTo : FsmGFConeRadiusTo {
		protected override void DoAction () {
			_value.Value = _t.RadiusTo;
		}
	}
#endregion  // Radius To

#region  Hex From
	public abstract class FsmGFConeHexFrom : FsmGFStateAction<Cone> {
		[Tooltip("Lower hex range of the renderer.")]
		[RequiredField]
		public FsmInt _value;
	}

	[Tooltip("Sets the lower hex range of the renderer.")]
	public class SetConeHexFrom : FsmGFConeHexFrom {
		protected override void DoAction () {
			_t.HexFrom = _value.Value;
		}
	}

	[Tooltip("Sets the lower hex range of the renderer.")]
	public class GetConeHexFrom : FsmGFConeHexFrom {
		protected override void DoAction () {
			_value.Value = _t.HexFrom;
		}
	}
#endregion  // Hex From

#region  Hex To
	public abstract class FsmGFConeHexTo : FsmGFStateAction<Cone> {
		[Tooltip("Upper hex range of the renderer.")]
		[RequiredField]
		public FsmInt _value;
	}

	[Tooltip("Sets the upper hex range of the renderer.")]
	public class SetConeHexTo : FsmGFConeHexTo {
		protected override void DoAction () {
			_t.HexTo = _value.Value;
		}
	}

	[Tooltip("Sets the upper hex range of the renderer.")]
	public class GetConeHexTo : FsmGFConeHexTo {
		protected override void DoAction () {
			_value.Value = _t.HexTo;
		}
	}
#endregion  // Hex To

#region  Layer From
	public abstract class FsmGFConeLayerFrom : FsmGFStateAction<Cone> {
		[Tooltip("Lower layer range of the renderer.")]
		[RequiredField]
		public FsmFloat _value;
	}

	[Tooltip("Sets the lower layer range of the renderer.")]
	public class SetConeLayerFrom : FsmGFConeLayerFrom {
		protected override void DoAction () {
			_t.LayerFrom = _value.Value;
		}
	}

	[Tooltip("Sets the lower layer range of the renderer.")]
	public class GetConeLayerFrom : FsmGFConeLayerFrom {
		protected override void DoAction () {
			_value.Value = _t.LayerFrom;
		}
	}
#endregion  // Layer From

#region  Layer To
	public abstract class FsmGFConeLayerTo : FsmGFStateAction<Cone> {
		[Tooltip("Upper layer range of the renderer.")]
		[RequiredField]
		public FsmFloat _value;
	}

	[Tooltip("Sets the upper layer range of the renderer.")]
	public class SetConeLayerTo : FsmGFConeLayerTo {
		protected override void DoAction () {
			_t.LayerTo = _value.Value;
		}
	}

	[Tooltip("Sets the upper layer range of the renderer.")]
	public class GetConeLayerTo : FsmGFConeLayerTo {
		protected override void DoAction () {
			_value.Value = _t.LayerTo;
		}
	}
#endregion  // Layer To
}
#endif // GRID_FRAMEWORK_PLAYMAKER
