using UnityEngine;
using GridFramework.Grids;
using GridFramework.Renderers.Rectangular;

namespace GridFramework.Examples.Endless3D {
	/// <summary>
	///   Adjust the spacing and rending range on the fly.
	/// </summary>
	/// <remarks>
	///   <para>
	///     As the camera moves in XZ-direction the rendering range is
	///     adjusted, and in Y-direction the spacing is adjusted. As the camera
	///     moves upwards the grid area to render increases, so by increasing
	///     the spacing we keep the number of lines to render constant.
	///     Rendering too many lines makes performance suffer and looks ugly.
	///   </para>
	///   <para>
	///     We will create a "fading" effect using two grids where one grid
	///     fades in and the other one fades out.
	///   </para>
	///   <para>
	///     It is assumed the grid has its origin at <c>(0, 0, 0)</c>. The grid
	///     is encapsulated as a new grid structure local to the script. This
	///     level of abstractions allows us to keep the script's <c>Update</c>
	///     method clean.
	///   </para>
	/// </remarks>
	[RequireComponent(typeof(Camera))]
	public class InfinityGrid : MonoBehaviour {
#region  Types
		/// <summary>
		///   Structure representing the compound level grid.
		/// </summary>
		/// <remarks>
		///   <para>
		///     The purpose of this structure is to abstract the grid class
		///     into a simpler "wrapper" class which exposes only as much API
		///     as we need.
		///   </para>
		///   <para>
		///     There are two modes: Dual and Flex. Flex mode is the default,
		///     one grid is stretched according to the camera's height to give
		///     the impression that the grid stays the same and the world
		///     scales instead.
		///   </para>
		///   <para>
		///     Dual mode is similar to Unity's editor grid in that it uses two
		///     grids: the primary grid and a larger secondary grid. As the
		///     camera rises the primary grid fades out and the secondary one
		///     fades in. Once the primary grid vanishes the secondary becomes
		///     the primary and the secondary becomes ten times as large.
		///   </para>
		///   <para>
		///     Dual mode works, but I couldn't get the rendering range to
		///     adjust to make it look good like in the editor. That's the only
		///     shortcoming, so feel free to experiment.
		///   </para>
		/// </remarks>
		private struct Grid {

			/// <summary>
			///   Mode of the new grid.
			/// </summary>
			public enum Mode {
				/// <summary>
				///   Two grids, like Unity Editor.
				/// </summary>
				Dual,
				/// <summary>
				///   One continuously expanding grid.
				/// </summary>
				Flex
			}

			/// <summary>
			///   Main grid for display.
			/// </summary>
			private readonly RectGrid _mainGrid;

			/// <summary>
			///   Secondary, larger grid (only for dual mode).
			/// </summary>
			private readonly RectGrid _subGrid;

			/// <summary>
			///   Renderer for the main grid.
			/// </summary>
			private readonly Parallelepiped _mainRenderer;

			/// <summary>
			///   Renderer for the secondary grid.
			/// </summary>
			private readonly Parallelepiped _subRenderer;

			/// <summary>
			///   Spacing of all axes of primary grid.
			/// </summary>
			private float _spacing;

			/// <summary>
			///   Current mode of the grid.
			/// </summary>
			private Mode _mode;

			/// <summary>
			///   The spacing of the grid (read-only).
			/// </summary>
			public float Spacing {
				get {
					return _mainGrid.Spacing.x;
				}
			}

			/// <summary>
			///   Lower left corner of the range.
			/// </summary>
			public Vector3 From {
				set {
					_mainRenderer.From = value;
					_subRenderer.From  = value / _spacing;
				}
			}

			/// <summary>
			///   Upper right corner of the range.
			/// </summary>
			public Vector3 To {
				set {
					_mainRenderer.To = value;
					_subRenderer.To  = value / _spacing;
				}
			}

			/// <summary>
			///   Construct a new grid made of two rectangular grids.
			/// </summary>
			public Grid(RectGrid mainGrid, RectGrid subGrid,
			            Parallelepiped mainRenderer, Parallelepiped subRenderer,
			            float height) {
				this._mainGrid = mainGrid;
				this._subGrid  = subGrid;

				this._mainRenderer = mainRenderer;
				this._subRenderer  = subRenderer;

				this._spacing =     10.0f;
				this._mode    = Mode.Flex;

				this.Update(height);
			}

			public Vector3 WorldToGrid(Vector3 world) {
				return _mainGrid.WorldToGrid(world);
			}

			public void Update(float height) {
				UpdateSpacing(height);
				if (_mode == Mode.Dual) {
					UpdateAlpha(height);
				}
			}

			/// <summary>
			///   Update the spacing of the grid based on the height of the
			///   camera.
			/// </summary>
			private void UpdateSpacing(float height) {
				if (_mode == Mode.Dual) {
					var level = height >= 0 ? Mathf.FloorToInt(height / _step)
					                        : Mathf.CeilToInt( height / _step);

					_mainGrid.Spacing = Mathf.Pow(_spacing, level) * Vector3.one;
					_subGrid.Spacing  = _spacing * _mainGrid.Spacing;
				} else {
					_mainGrid.Spacing = Mathf.Max(0.5f, height) * Vector3.one;
				}
			}

			/// <summary>
			///   Update the opacity of the grid based on the height of the
			///   camera.
			/// </summary>
			private void UpdateAlpha(float height) {
				float mainAlpha, subAlpha;

				var mainX = _mainRenderer.ColorX;
				var mainZ = _mainRenderer.ColorZ;
				var subX  = _subRenderer.ColorX;
				var subZ  = _subRenderer.ColorZ;

				//between 0 and 1
				var ratio = height / _step - Mathf.Floor(height / _step);
				mainAlpha = 1.0f - ratio;
				subAlpha  = 0.0f + ratio;

				mainX.a = mainAlpha;
				mainZ.a = mainAlpha;
				subX.a  = subAlpha;
				subZ.a  = subAlpha;

			
				_mainRenderer.ColorX = mainX;
				_mainRenderer.ColorZ = mainZ;

				_subRenderer.ColorX = subX;
				_subRenderer.ColorZ = subZ;
			
			}
		}
#endregion  // Types

#region  Private variables
		/// <summary>
		///   At what height steps to change grids (Dual mode only).
		/// </summary>
		private const float _step = 10.0f;

		/// <summary>
		///   Abstracting the grids into one.
		/// </summary>
		private Grid _grid;

		/// <summary>
		///   Aspect ratio between spacing and camera distance.
		/// </summary>
		private const float _spacingRatio = 1.0f;

		/// <summary>
		///   Size of the grid in both directions.
		/// </summary>
		private const float _gridSize = 20.0f;

		private const float _farPlaneScale = 15.0f;

		/// <summary>
		///   The grid camera, cached.
		/// </summary>
		private Camera _cam;
#endregion  // Private variables

#region  Public variables
		/// <summary>
		///   The primary grid.
		/// </summary>
		public RectGrid _mainGrid;
		/// <summary>
		///   The secondary grid (only for dual mode).
		/// </summary>
		public RectGrid _subGrid;

		/// <summary>
		///   The primary renderer.
		/// </summary>
		public Parallelepiped _mainRenderer;
		/// <summary>
		///   The secondary renderer (only for dual mode).
		/// </summary>
		public Parallelepiped _subRenderer;
#endregion  // Public variables

#region  Callback methods
		void Start(){
			_cam = GetComponent<Camera>();
			var y = transform.position.y;
			_grid = new Grid(_mainGrid, _subGrid, _mainRenderer, _subRenderer, y);
		}

		void Update() {
			// Distance of the far plane scales linearly with the camera height.
			var height = Mathf.Abs(transform.position.y);
			var clipPlane = _farPlaneScale * Mathf.Max(0.5f, height);

			_grid.Update(height);
			_cam.farClipPlane = clipPlane;

			// Adjust the rendering range of the grid to be around the camera.
			var pos = _grid.WorldToGrid(_cam.transform.position);
			pos.y = 0;
			_grid.From = pos - _gridSize * new Vector3(1, 0, 1);
			_grid.To   = pos + _gridSize * new Vector3(1, 0, 1);
		}
#endregion  // Callback methods
	}
}
