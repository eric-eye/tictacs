// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Grids;

namespace HutongGames.PlayMaker.Actions {
#region  SphericToWorld
	[Tooltip("Converts spheric coordinates to world coordinates")]
	public class SphereSphericToWorld : FsmGFStateAction<SphereGrid> {
		[RequiredField]
		public FsmVector3 _grid;
		public FsmVector3 _world;

		protected override void DoAction() {
			_world.Value = _t.SphericToWorld(_grid.Value);
		}
	}
#endregion  // SphericToWorld

#region  GeographicToWorld
	[Tooltip("Converts geographic coordinates to world coordinates")]
	public class SphereGeographicToWorld : FsmGFStateAction<SphereGrid> {
		[RequiredField]
		public FsmVector3 _grid;
		public FsmVector3 _world;

		protected override void DoAction() {
			_world.Value = _t.GeographicToWorld(_grid.Value);
		}
	}
#endregion  // GeographicToWorld

#region  GridToWorld
	[Tooltip("Converts grid coordinates to world coordinates")]
	public class SphereGridToWorld : FsmGFStateAction<SphereGrid> {
		[RequiredField]
		public FsmVector3 _grid;
		public FsmVector3 _world;

		protected override void DoAction() {
			_world.Value = _t.GridToWorld(_grid.Value);
		}
	}
#endregion  // GridToWorld

#region  WorldToSpheric
	[Tooltip("Converts world coordinates to spheric coordinates")]
	public class SphereWorldToSpheric : FsmGFStateAction<SphereGrid> {
		[RequiredField]
		public FsmVector3 _grid;
		public FsmVector3 _world;

		protected override void DoAction() {
			_world.Value = _t.WorldToSpheric(_grid.Value);
		}
	}
#endregion  // WorldToSpheric

#region  GeographicToSpheric
	[Tooltip("Converts geographic coordinates to spheric coordinates")]
	public class SphereGeographicToSpheric : FsmGFStateAction<SphereGrid> {
		[RequiredField]
		public FsmVector3 _grid;
		public FsmVector3 _world;

		protected override void DoAction() {
			_world.Value = _t.GeographicToSpheric(_grid.Value);
		}
	}
#endregion  // GeographicToSpheric

#region  GridToSpheric
	[Tooltip("Converts grid coordinates to spheric coordinates")]
	public class SphereGridToSpheric : FsmGFStateAction<SphereGrid> {
		[RequiredField]
		public FsmVector3 _grid;
		public FsmVector3 _world;

		protected override void DoAction() {
			_world.Value = _t.GridToSpheric(_grid.Value);
		}
	}
#endregion  // GeographicToSpheric

#region  WorldToGeographic
	[Tooltip("Converts world coordinates to geographic coordinates")]
	public class SphereWorldToGeographic : FsmGFStateAction<SphereGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.WorldToGeographic(_from.Value);
		}
	}
#endregion  // WorldToGeographic

#region  SphericToGeographic
	[Tooltip("Converts spheric coordinates to geographic coordinates")]
	public class SphereSphericToGeographic : FsmGFStateAction<SphereGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.SphericToGeographic(_from.Value);
		}
	}
#endregion  // SphericToGeographic

#region  GridToGeographic
	[Tooltip("Converts grid coordinates to geographic coordinates")]
	public class SphereGridToGeographic : FsmGFStateAction<SphereGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.GridToGeographic(_from.Value);
		}
	}
#endregion  // GeographicToGeographic

#region  WorldToGrid
	[Tooltip("Converts world coordinates to grid coordinates")]
	public class SphereWorldToGrid : FsmGFStateAction<SphereGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.WorldToGrid(_from.Value);
		}
	}
#endregion  // WorldToGrid

#region  SphericToGeographic
	[Tooltip("Converts spheric coordinates to grid coordinates")]
	public class SphereSphericToGrid : FsmGFStateAction<SphereGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.SphericToGrid(_from.Value);
		}
	}
#endregion  // SphericToGeographic

#region  GeographicToGrid
	[Tooltip("Converts geographic coordinates to grid coordinates")]
	public class SphereGeographicToGrid : FsmGFStateAction<SphereGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.GeographicToGrid(_from.Value);
		}
	}
#endregion  // GeographicToGrid
}
#endif // GRID_FRAMEWORK_PLAYMAKER
