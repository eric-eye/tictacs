// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Grids;

namespace HutongGames.PlayMaker.Actions {
#region  GridToWorld
	[Tooltip("Converts grid coordinates to world coordinates")]
	public class RectGridToWorld : FsmGFStateAction<RectGrid> {
		[RequiredField]
		public FsmVector3 _grid;
		public FsmVector3 _world;

		protected override void DoAction() {
			_world.Value = _t.GridToWorld(_grid.Value);
		}
	}
#endregion  // GridToWorld

#region  WorldToGrid
	[Tooltip("Converts world coordinates to grid coordinates")]
	public class RectWorldToGrid : FsmGFStateAction<RectGrid> {
		[RequiredField]
		public FsmVector3 _grid;
		public FsmVector3 _world;

		protected override void DoAction() {
			_grid.Value = _t.WorldToGrid(_world.Value);
		}
	}
#endregion  // WorldToGrid
}
#endif // GRID_FRAMEWORK_PLAYMAKER
