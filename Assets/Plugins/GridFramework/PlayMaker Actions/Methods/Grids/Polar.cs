// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Grids;

namespace HutongGames.PlayMaker.Actions {
#region  WorldToGrid
	[Tooltip("Converts world coordinates to grid coordinates")]
	public class PolarWorldToGrid : FsmGFStateAction<PolarGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.WorldToGrid(_from.Value);
		}
	}
#endregion  // WorldToGrid

#region  WorldToPolar
	[Tooltip("Converts world coordinates to polar coordinates")]
	public class PolarWorldToPolar : FsmGFStateAction<PolarGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.WorldToPolar(_from.Value);
		}
	}
#endregion  // WorldToPolar

#region  PolarToGrid
	[Tooltip("Converts polar coordinates to grid coordinates")]
	public class PolarPolarToGrid : FsmGFStateAction<PolarGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.PolarToGrid(_from.Value);
		}
	}
#endregion  // PolarToGrid

#region  PolarToWorld
	[Tooltip("Converts polar coordinates to grid coordinates")]
	public class PolarPolarToWorld : FsmGFStateAction<PolarGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.PolarToWorld(_from.Value);
		}
	}
#endregion  // PolarToWorld

#region  GridToPolar
	[Tooltip("Converts grid coordinates to polar coordinates")]
	public class PolarGridToPolar : FsmGFStateAction<PolarGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.GridToPolar(_from.Value);
		}
	}
#endregion  // GridToPolar

#region  GridToWorld
	[Tooltip("Converts grid coordinates to grid coordinates")]
	public class PolarGridToWorld : FsmGFStateAction<PolarGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.GridToWorld(_from.Value);
		}
	}
#endregion  // GridToWorld
}
#endif // GRID_FRAMEWORK_PLAYMAKER
