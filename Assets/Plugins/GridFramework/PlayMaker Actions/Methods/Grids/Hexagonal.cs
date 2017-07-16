// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Grids;

// Missing: Any conversion to and from cubic

namespace HutongGames.PlayMaker.Actions {
#region  WorldToHerringUp
	[Tooltip("Converts world coordinates to upwards herringbone coordinates")]
	public class HexWorldToHerringUp : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.WorldToHerringUp(_from.Value);
		}
	}
#endregion  // WorldToHerringUp

#region  WorldToHerringDown
	[Tooltip("Converts world coordinates to downwards herringbone coordinates")]
	public class HexWorldToHerringDown : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.WorldToHerringDown(_from.Value);
		}
	}
#endregion  // WorldToHerringDown

#region  WorldToRhombicUp
	[Tooltip("Converts world coordinates to upwards rhombic coordinates")]
	public class HexWorldToRhombicUp : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.WorldToRhombicUp(_from.Value);
		}
	}
#endregion  // WorldToRhombicUp

#region  WorldToRhombicDown
	[Tooltip("Converts world coordinates to downwards rhombic coordinates")]
	public class HexWorldToRhombicDown : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.WorldToRhombicDown(_from.Value);
		}
	}
#endregion  // WorldToRhombicDown

#region  HerringUpToWorld
	[Tooltip("Converts upwards herringbone coordinates to world coordinates")]
	public class HexHerringUpToWorld : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.HerringUpToWorld(_from.Value);
		}
	}
#endregion  // HerringUpToHerringUp

#region  HerringUpToHerringDown
	[Tooltip("Converts upwards herringbone coordinates to downwards herringbone coordinates")]
	public class HexHerringUpToHerringDown : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.HerringUpToHerringDown(_from.Value);
		}
	}
#endregion  // HerringUpToHerringDown

#region  HerringUpToRhombicUp
	[Tooltip("Converts upwards herringbone coordinates to upwards rhombic coordinates")]
	public class HexHerringUpToRhombicUp : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.HerringUpToRhombicUp(_from.Value);
		}
	}
#endregion  // HerringUpToRhombicUp

#region  HerringUpToRhombicDown
	[Tooltip("Converts upwards herringbone coordinates to downwards rhombic coordinates")]
	public class HexHerringUpToRhombicDown : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.HerringUpToRhombicDown(_from.Value);
		}
	}
#endregion  // HerringUpToRhombicDown

#region  HerringDownToWorld
	[Tooltip("Converts downwards herringbone coordinates to world coordinates")]
	public class HexHerringDownToWorld : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.HerringDownToWorld(_from.Value);
		}
	}
#endregion  // HerringDownToHerringDown

#region  HerringDownToHerringUp
	[Tooltip("Converts downwards herringbone coordinates to upwards herringbone coordinates")]
	public class HexHerringDownToHerringDown : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.HerringDownToHerringUp(_from.Value);
		}
	}
#endregion  // HerringDownToHerringUp

#region  HerringDownToRhombicUp
	[Tooltip("Converts downwards herringbone coordinates to upwards rhombic coordinates")]
	public class HexHerringDownToRhombicUp : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.HerringDownToRhombicUp(_from.Value);
		}
	}
#endregion  // HerringDownToRhombicUp

#region  HerringDownToRhombicDown
	[Tooltip("Converts downwards herringbone coordinates to downwards rhombic coordinates")]
	public class HexHerringDownToRhombicDown : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.HerringDownToRhombicDown(_from.Value);
		}
	}
#endregion  // HerringDownToRhombicDown

#region  RhombicUpToWorld
	[Tooltip("Converts upwards rhombic coordinates to world coordinates")]
	public class HexRhombicUpToWorld : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.RhombicUpToWorld(_from.Value);
		}
	}
#endregion  // RhombicUpToRhombicUp

#region  RhombicUpToHerringUp
	[Tooltip("Converts upwards rhombic coordinates to upwards herringbone coordinates")]
	public class HexRhombicUpToHerringUp : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.RhombicUpToHerringUp(_from.Value);
		}
	}
#endregion  // RhombicUpToRhombicUp

#region  RhombicUpToHerringDown
	[Tooltip("Converts upwards rhombic coordinates to downwards herringbone coordinates")]
	public class HexRhombicUpToHerringDown : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.RhombicUpToHerringDown(_from.Value);
		}
	}
#endregion  // RhombicUpToHerringDown

#region  RhombicUpToRhombicDown
	[Tooltip("Converts upwards rhombic coordinates to downwards rhombic coordinates")]
	public class HexRhombicUpToRhombicDown : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.RhombicUpToRhombicDown(_from.Value);
		}
	}
#endregion  // RhombicUpToRhombicDown

#region  RhombicDownToWorld
	[Tooltip("Converts downwards rhombic coordinates to world coordinates")]
	public class HexRhombicDownToWorld : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.RhombicDownToWorld(_from.Value);
		}
	}
#endregion  // RhombicDownToRhombicDown

#region  RhombicDownToHerringUp
	[Tooltip("Converts downwards rhombic coordinates to upwards herringbone coordinates")]
	public class HexRhombicDownToHerringUp : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.RhombicDownToHerringUp(_from.Value);
		}
	}
#endregion  // RhombicDownToRhombicDown

#region  RhombicDownToHerringDown
	[Tooltip("Converts downwards rhombic coordinates to downwards herringbone coordinates")]
	public class HexRhombicDownToHerringDown : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.RhombicDownToHerringDown(_from.Value);
		}
	}
#endregion  // RhombicDownToHerringDown

#region  RhombicDownToRhombicUp
	[Tooltip("Converts downwards rhombic coordinates to upwards rhombic coordinates")]
	public class HexRhombicDownToRhombicUp : FsmGFStateAction<HexGrid> {
		[RequiredField]
		public FsmVector3 _from;
		public FsmVector3 _to;

		protected override void DoAction() {
			_to.Value = _t.RhombicDownToRhombicUp(_from.Value);
		}
	}
#endregion  // RhombicDownToRhombicUp
}
#endif // GRID_FRAMEWORK_PLAYMAKER
