// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Renderers;

// Missing: LineSets (impossible)

namespace HutongGames.PlayMaker.Actions {
#region  ColorX
	public abstract class FsmGFRColorX : FsmGFStateAction<GridRenderer> {
		[Tooltip("Color of X-lines.")]
		[RequiredField]
		public FsmColor _colorX;
	}

	[Tooltip("Sets the color of X-lines.")]
	public class SetColorX : FsmGFRColorX {
		protected override void DoAction () {
			_t.ColorX = _colorX.Value;
		}
	}

	[Tooltip("Sets the color of X-lines.")]
	public class GetColorX : FsmGFRColorX {
		protected override void DoAction () {
			_colorX.Value = _t.ColorX;
		}
	}
#endregion  // ColorX

#region  ColorY
	public abstract class FsmGFRColorY : FsmGFStateAction<GridRenderer> {
		[Tooltip("Color of Y-lines.")]
		[RequiredField]
		public FsmColor _colorY;
	}

	[Tooltip("Sets the color of Y-lines.")]
	public class SetColorY : FsmGFRColorY {
		protected override void DoAction () {
			_t.ColorY = _colorY.Value;
		}
	}

	[Tooltip("Sets the color of Y-lines.")]
	public class GetColorY : FsmGFRColorY {
		protected override void DoAction () {
			_colorY.Value = _t.ColorY;
		}
	}
#endregion  // ColorY

#region  ColorZ
	public abstract class FsmGFRColorZ : FsmGFStateAction<GridRenderer> {
		[Tooltip("Color of Z-lines.")]
		[RequiredField]
		public FsmColor _colorZ;
	}

	[Tooltip("Sets the color of Z-lines.")]
	public class SetColorZ : FsmGFRColorZ {
		protected override void DoAction () {
			_t.ColorZ = _colorZ.Value;
		}
	}

	[Tooltip("Sets the color of Z-lines.")]
	public class GetColorZ : FsmGFRColorZ {
		protected override void DoAction () {
			_colorZ.Value = _t.ColorZ;
		}
	}
#endregion  // ColorZ

#region  LineWidth
	public abstract class FsmGFRLineWidth : FsmGFStateAction<GridRenderer> {
		[Tooltip("Width of the rendered lines.")]
		[RequiredField]
		public FsmFloat _lineWidth;
	}

	[Tooltip("Sets the width of the rendered lines.")]
	public class SetLineWidth : FsmGFRLineWidth {
		protected override void DoAction () {
			_t.LineWidth = _lineWidth.Value;
		}
	}

	[Tooltip("Sets the width of the rendered lines.")]
	public class GetLineWidth : FsmGFRLineWidth {
		protected override void DoAction () {
			_lineWidth.Value = _t.LineWidth;
		}
	}
#endregion  // LineWidth

#region  Priority
	public abstract class FsmGFRPriority : FsmGFStateAction<GridRenderer> {
		[Tooltip("Priority of the renderer.")]
		[RequiredField]
		public FsmInt _priority;
	}

	[Tooltip("Sets the priority of the renderer.")]
	public class SetPriority : FsmGFRPriority {
		protected override void DoAction () {
			_t.Priority = _priority.Value;
		}
	}

	[Tooltip("Sets the priority of the renderer.")]
	public class GetPriority : FsmGFRPriority {
		protected override void DoAction () {
			_priority.Value = _t.Priority;
		}
	}
#endregion  // Priority

#region  Material
	public abstract class FsmGFRMaterial : FsmGFStateAction<GridRenderer> {
		[Tooltip("Material of the renderer.")]
		[RequiredField]
		public FsmMaterial _material;
	}

	[Tooltip("Sets the material of the renderer.")]
	public class SetGridMaterial : FsmGFRMaterial {
		protected override void DoAction () {
			_t.Material = _material.Value;
		}
	}

	[Tooltip("Sets the material of the renderer.")]
	public class GetGridMaterial : FsmGFRMaterial {
		protected override void DoAction () {
			_material.Value = _t.Material;
		}
	}
#endregion  // Material
}
#endif // GRID_FRAMEWORK_PLAYMAKER
