// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Renderers.Hexagonal;

// Missing: shift (impossible)

namespace HutongGames.PlayMaker.Actions {
#region  Bottom
	public abstract class FsmGFRectangleBottom : FsmGFStateAction<Rectangle> {
		[Tooltip("Lower edge of the renderer.")]
		[RequiredField]
		public FsmInt _value;
	}

	[Tooltip("Sets the lower edge of the renderer.")]
	public class SetRectangleBottom : FsmGFRectangleBottom {
		protected override void DoAction () {
			_t.Bottom = _value.Value;
		}
	}

	[Tooltip("Gets the lower edge of the renderer.")]
	public class GetRectangleBottom : FsmGFRectangleBottom {
		protected override void DoAction () {
			_value.Value = _t.Bottom;
		}
	}
#endregion  // Bottom

#region  Top
	public abstract class FsmGFRectangleTop : FsmGFStateAction<Rectangle> {
		[Tooltip("Upper edge of the renderer.")]
		[RequiredField]
		public FsmInt _value;
	}

	[Tooltip("Gets the upper edge of the renderer.")]
	public class SetRectangleTop : FsmGFRectangleTop {
		protected override void DoAction () {
			_t.Top = _value.Value;
		}
	}

	[Tooltip("Sets the upper edge of the renderer.")]
	public class GetRectangleTop : FsmGFRectangleTop {
		protected override void DoAction () {
			_value.Value = _t.Top;
		}
	}
#endregion  // Top

#region  Left
	public abstract class FsmGFRectangleLeft : FsmGFStateAction<Rectangle> {
		[Tooltip("Left edge of the renderer.")]
		[RequiredField]
		public FsmInt _value;
	}

	[Tooltip("Sets the left edge of the renderer.")]
	public class SetRectangleLeft : FsmGFRectangleLeft {
		protected override void DoAction () {
			_t.Left = _value.Value;
		}
	}

	[Tooltip("Gets the left edge of the renderer.")]
	public class GetRectangleLeft : FsmGFRectangleLeft {
		protected override void DoAction () {
			_value.Value = _t.Left;
		}
	}
#endregion  // Left

#region  Right
	public abstract class FsmGFRectangleRight : FsmGFStateAction<Rectangle> {
		[Tooltip("Right edge of the renderer.")]
		[RequiredField]
		public FsmInt _value;
	}

	[Tooltip("Sets the right edge of the renderer.")]
	public class SetRectangleRight : FsmGFRectangleRight {
		protected override void DoAction () {
			_t.Right = _value.Value;
		}
	}

	[Tooltip("Gets the right edge of the renderer.")]
	public class GetRectangleRight : FsmGFRectangleRight {
		protected override void DoAction () {
			_value.Value = _t.Right;
		}
	}
#endregion  // Right

#region  Layer From
	public abstract class FsmGFRectangleLayerFrom : FsmGFStateAction<Rectangle> {
		[Tooltip("Lower layer range of the renderer.")]
		[RequiredField]
		public FsmFloat _value;
	}

	[Tooltip("Sets the lower layer range of the renderer.")]
	public class SetRectangleLayerFrom : FsmGFRectangleLayerFrom {
		protected override void DoAction () {
			_t.LayerFrom = _value.Value;
		}
	}

	[Tooltip("Gets the lower layer range of the renderer.")]
	public class GetRectangleLayerFrom : FsmGFRectangleLayerFrom {
		protected override void DoAction () {
			_value.Value = _t.LayerFrom;
		}
	}
#endregion  // Layer From

#region  Layer To
	public abstract class FsmGFRectangleLayerTo : FsmGFStateAction<Rectangle> {
		[Tooltip("Upper layer range of the renderer.")]
		[RequiredField]
		public FsmFloat _value;
	}

	[Tooltip("Sets the upper layer range of the renderer.")]
	public class SetRectangleLayerTo : FsmGFRectangleLayerTo {
		protected override void DoAction () {
			_t.LayerTo = _value.Value;
		}
	}

	[Tooltip("Gets the upper layer range of the renderer.")]
	public class GetRectangleLayerTo : FsmGFRectangleLayerTo {
		protected override void DoAction () {
			_value.Value = _t.LayerTo;
		}
	}
#endregion  // Layer To
}
#endif // GRID_FRAMEWORK_PLAYMAKER
