// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Renderers.Hexagonal;

namespace HutongGames.PlayMaker.Actions {
#region  Bottom
	public abstract class FsmGFRhombusBottom : FsmGFStateAction<Rhombus> {
		[Tooltip("Lower edge of the renderer.")]
		[RequiredField]
		public FsmInt _value;
	}

	[Tooltip("Sets the lower edge of the renderer.")]
	public class SetRhombusBottom : FsmGFRhombusBottom {
		protected override void DoAction () {
			_t.Bottom = _value.Value;
		}
	}

	[Tooltip("Gets the lower edge of the renderer.")]
	public class GetRhombusBottom : FsmGFRhombusBottom {
		protected override void DoAction () {
			_value.Value = _t.Bottom;
		}
	}
#endregion  // Bottom

#region  Top
	public abstract class FsmGFRhombusTop : FsmGFStateAction<Rhombus> {
		[Tooltip("Upper edge of the renderer.")]
		[RequiredField]
		public FsmInt _value;
	}

	[Tooltip("Gets the upper edge of the renderer.")]
	public class SetRhombusTop : FsmGFRhombusTop {
		protected override void DoAction () {
			_t.Top = _value.Value;
		}
	}

	[Tooltip("Sets the upper edge of the renderer.")]
	public class GetRhombusTop : FsmGFRhombusTop {
		protected override void DoAction () {
			_value.Value = _t.Top;
		}
	}
#endregion  // Top

#region  Left
	public abstract class FsmGFRhombusLeft : FsmGFStateAction<Rhombus> {
		[Tooltip("Left edge of the renderer.")]
		[RequiredField]
		public FsmInt _value;
	}

	[Tooltip("Sets the left edge of the renderer.")]
	public class SetRhombusLeft : FsmGFRhombusLeft {
		protected override void DoAction () {
			_t.Left = _value.Value;
		}
	}

	[Tooltip("Gets the left edge of the renderer.")]
	public class GetRhombusLeft : FsmGFRhombusLeft {
		protected override void DoAction () {
			_value.Value = _t.Left;
		}
	}
#endregion  // Left

#region  Right
	public abstract class FsmGFRhombusRight : FsmGFStateAction<Rhombus> {
		[Tooltip("Right edge of the renderer.")]
		[RequiredField]
		public FsmInt _value;
	}

	[Tooltip("Sets the right edge of the renderer.")]
	public class SetRhombusRight : FsmGFRhombusRight {
		protected override void DoAction () {
			_t.Right = _value.Value;
		}
	}

	[Tooltip("Gets the right edge of the renderer.")]
	public class GetRhombusRight : FsmGFRhombusRight {
		protected override void DoAction () {
			_value.Value = _t.Right;
		}
	}
#endregion  // Right

#region  Layer From
	public abstract class FsmGFRhombusLayerFrom : FsmGFStateAction<Rhombus> {
		[Tooltip("Lower layer range of the renderer.")]
		[RequiredField]
		public FsmFloat _value;
	}

	[Tooltip("Sets the lower layer range of the renderer.")]
	public class SetRhombusLayerFrom : FsmGFRhombusLayerFrom {
		protected override void DoAction () {
			_t.LayerFrom = _value.Value;
		}
	}

	[Tooltip("Sets the lower layer range of the renderer.")]
	public class GetRhombusLayerFrom : FsmGFRhombusLayerFrom {
		protected override void DoAction () {
			_value.Value = _t.LayerFrom;
		}
	}
#endregion  // Layer From

#region  Layer To
	public abstract class FsmGFRhombusLayerTo : FsmGFStateAction<Rhombus> {
		[Tooltip("Upper layer range of the renderer.")]
		[RequiredField]
		public FsmFloat _value;
	}

	[Tooltip("Sets the upper layer range of the renderer.")]
	public class SetRhombusLayerTo : FsmGFRhombusLayerTo {
		protected override void DoAction () {
			_t.LayerTo = _value.Value;
		}
	}

	[Tooltip("Sets the upper layer range of the renderer.")]
	public class GetRhombusLayerTo : FsmGFRhombusLayerTo {
		protected override void DoAction () {
			_value.Value = _t.LayerTo;
		}
	}
#endregion  // Layer To
}
#endif // GRID_FRAMEWORK_PLAYMAKER

