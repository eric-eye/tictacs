// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Grids;

// Missing: Sides (impossible)

namespace HutongGames.PlayMaker.Actions {
#region  Radius
	public abstract class FsmGFHRadius : FsmGFStateAction<HexGrid> {
		[Tooltip("Radius of the hex grid's hexes, i.e. the distance form the centre to a vertex.")]
		[RequiredField]
		public FsmFloat _radius;
	}

	[Tooltip("Sets the radius of the hex grid's hexes, i.e. the distance form the centre to a vertex.")]
	public class SetHexRadius : FsmGFHRadius {
		protected override void DoAction () {
			_t.Radius = _radius.Value;
		}
	}

	[Tooltip("Gets the radius of the hex grid's hexes, i.e. the distance form the centre to a vertex.")]
	public class GetHexRadius : FsmGFHRadius {
		protected override void DoAction () {
			_radius.Value = _t.Radius;
		}
	}
#endregion  // Radius

#region  Side
	[Tooltip("Gets the hex grid's \"side\", which is 1.5 times the radius.")]
	public class GetSide : FsmGFStateAction<HexGrid> {
		[Tooltip("The hex grid's \"side\", which is 1.5 times the radius.")]
		public FsmFloat _side;
		protected override void DoAction () {
			_side.Value = _t.Side;
		}
	}
#endregion  // Side

#region  Height
	[Tooltip("Gets the hex grid's \"height\", which is the full width of the hex.")]
	public class GetHeight : FsmGFStateAction<HexGrid> {
		[Tooltip("The hex grid's \"height\", which is the full width of the hex.")]
		public FsmFloat _height;
		protected override void DoAction () {
			_height.Value = _t.Height;
		}
	}
#endregion  // Height

#region  Width
	[Tooltip("Gets the hex grid's \"height\", which is the distance between opposite vertices, i.e. twice the radius.")]
	public class GetWidth : FsmGFStateAction<HexGrid> {
		[Tooltip("The hex grid's \"height\", which is the distance between opposite vertices.")]
		public FsmFloat _width;
		protected override void DoAction () {
			_width.Value = _t.Width;
		}
	}
#endregion  // Width
}
#endif  // GRID_FRAMEWORK_PLAYMAKER
