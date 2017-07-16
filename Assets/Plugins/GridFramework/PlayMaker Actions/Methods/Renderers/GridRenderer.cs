// #define GRID_FRAMEWORK_PLAYMAKER
#if GRID_FRAMEWORK_PLAYMAKER

using GridFramework.Renderers;

namespace HutongGames.PlayMaker.Actions {
	[Tooltip("Refreshes the lines of the renderer.")]
	public class GFGridRendererRefresh : FsmGFStateAction<GridRenderer> {
		protected override void DoAction() {
			_t.Refresh();
		}
	}
}
#endif // GRID_FRAMEWORK_PLAYMAKER

