using UnityEngine;
using GridFramework.Grids;
using GridFramework.Renderers;

namespace GridFramework.Examples.TerrainMesh {
	/// <summary>
	///   Builds a mesh that resembles a terrain in a retro game like SimCity.
	/// </summary>
	[RequireComponent(typeof(RectGrid))]
	public class TerrainMeshBuilder : MonoBehaviour {
#region  Public variables
		/// <summary>
		///   Used for the rendering the plane.
		/// </summary>
		public Material _groundMaterial;
#endregion  // Public variables
		
#region  Private variables
		/// <summary>
		///   Every entry represents the height of the corresponding vertex.
		/// </summary>
		/// <remarks>
		///   <para>
		///     The order of entries is <c>[row, height]</c>.
		///   </para>
		/// </remarks>
		private readonly int[,] heightMap = {
			{1, 1, 1, 0, 0, 0, 0, 1, 1, 2}, 
			{0, 0, 0, 0, 0, 0, 0, 1, 1, 2}, 
			{0, 0, 0, 1, 1, 0, 0, 1, 1, 1}, 
			{0, 0, 0, 1, 1, 0, 0, 0, 0, 0}, 
			{0, 0, 0, 1, 1, 0, 0, 0, 0, 0}, 
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
			{0, 0, 0, 0, 0, 0, 0, 1, 0, 0}, 
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 1}, 
			{0, 0, 0, 0, 0, 0, 0, 0, 1, 1}, 
		};
		
		/// <summary>
		///   The mesh used for terrain, we will be referencing it often.
		/// </summary>
		private Mesh _mesh;
	
		private MeshFilter _mf;
		private MeshCollider _mc;
		private MeshRenderer _mr;
		private RectGrid _grid;

		private string heightString;
#endregion  // Private variables
	
#region  Callback methods
		void Awake() {
			_mf   = gameObject.AddComponent<MeshFilter>();
			_mc   = gameObject.AddComponent<MeshCollider>();
			_mr   = gameObject.AddComponent<MeshRenderer>();
			_grid = GetComponent<RectGrid>();
			
			// Create the mesh, then assign it to the components that need it
			// and show the height map in the GUI.
			BuildMesh();
			AssignMesh();
			UpdateHeightString();

			// Disable the renderer, looks better that way.
			var gr = GetComponent<GridRenderer>();
			if (gr) {
				gr.enabled = false;
			}
		}

		void OnGUI() {
			const int x = 10, y = 10, w = 170, h = 170;
			var rect = new Rect (x, y, w, h);
			GUI.TextArea(rect, heightString);
		}
		
		void OnMouseOver() {
			int raise;

			if (Input.GetMouseButtonDown(0)) {  // when the player left-clicks
				raise = 1;
			} else if (Input.GetMouseButtonDown(1)) { // when the player right-clicks
				raise = -1;
			} else {
				return;
			}

			RaycastHit hit;
			_mc.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity);

			var gridPoint = _grid.WorldToGrid(hit.point);
			var row    = Mathf.RoundToInt(-gridPoint.z);
			var column = Mathf.RoundToInt( gridPoint.x);
			var index  = RowColumn2Index(row, column);

			var vertices = _mesh.vertices;
			vertices[index] += raise * _grid.Up;
			_mesh.vertices = vertices;

			CleanupMesh(_mesh);
	
			// To update the mesh collider remove its mesh and then re-assign
			// it again.
			_mc.sharedMesh = null;
			_mc.sharedMesh = _mesh;

			heightMap[row, column] += raise;
			UpdateHeightString();
		}
#endregion  // Callback methods
		
#region  Private methods
		/// <summary>
		///   Build a new mesh object based on the height map.
		/// </summary>
		private void BuildMesh() {
			_mesh = new Mesh();
			_mesh.Clear();

			var vertices  = ComputeVertices(_mesh);
			AssignTriangles(_mesh);
			AssignUVs(_mesh, vertices);
			
			CleanupMesh(_mesh);
		}
		
		private void AssignMesh() {
			_mf.mesh = _mesh;
			_mc.sharedMesh = _mesh;
			_mr.sharedMaterial = _groundMaterial;
		}

		/// <summary>
		///   Compute the vertices of the mesh.
		/// </summary>
		private Vector3[] ComputeVertices(Mesh mesh) {
			var rows = heightMap.GetLength(0);
			var columns = heightMap.GetLength(1);
			
			// This mesh uses shared vertices for simplicity
			var vertices = new Vector3[rows * columns];
			for (var r = 0; r < rows; r++){
				for (var c = 0; c < columns; c++) {
					var h = heightMap[r, c];
					// Now assign the vertex depending on its row, column and
					// height (vector in local space; row, column and height in
					// grid space)
					var  gridVertex = new Vector3(c, h, -r);
					var worldVertex = _grid.GridToWorld(gridVertex);
					var localVertex = transform.InverseTransformPoint(worldVertex);

					vertices[r * rows + c] = localVertex;
				}
			}

			// Assign the vertices to the mesh
			mesh.vertices = vertices;
			return vertices;
		}

		private int[] AssignTriangles(Mesh mesh) {
			// Three vertices per triangle, two triangles per tile, one tile
			// between two columns/rows.
			var rows = heightMap.GetLength(0);
			var columns = heightMap.GetLength(1);

			var amount = 3 * 2 * (rows - 1) * (columns - 1);
			var triangles = new int[amount];
			var counter = 0; // this will keep track of where in the triangles array we currently are
			
			for (var i = 0; i < rows - 1; i++) {
				for (var j = 0; j < columns - 1; j++){
					// assign the vertex indices in a clockwise direction
					triangles[counter + 0] = RowColumn2Index(i + 0, j + 0);
					triangles[counter + 1] = RowColumn2Index(i + 0, j + 1);
					triangles[counter + 2] = RowColumn2Index(i + 1, j + 0);
					triangles[counter + 3] = RowColumn2Index(i + 1, j + 0);
					triangles[counter + 4] = RowColumn2Index(i + 0, j + 1);
					triangles[counter + 5] = RowColumn2Index(i + 1, j + 1);
					counter += 6; // increment the counter for the next two triangles (six vertices)
				}
			}

			// Assign the triangles to the mesh
			mesh.triangles = triangles;
			return triangles;
		}

		private Vector2[] AssignUVs(Mesh mesh, Vector3[] vertices) {
			// The UVs are the same as the coordinates of the vertex, that way
			// every tile has a copy of the texture image.
			var uvs = new Vector2[vertices.Length];
	        for (var i = 0; i < uvs.Length; ++i) {
	            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
	        }
	        mesh.uv = uvs;
	        return uvs;
		}

		private void CleanupMesh(Mesh mesh) {
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
		}
		
		private void UpdateHeightString() {
			heightString = "";

			var rows = heightMap.GetLength(0);
			var columns = heightMap.GetLength(1);
			for (var i = 0; i < rows; ++i) {
				for (var j = 0; j < columns; ++j) {
					// Add the entries from the heights array
					heightString += " " + heightMap[i, j] + " ";
				}
				// Line break after reaching the end of the row
				heightString += "\n";
			}
		}

		/// <summary>
		///   Convert a row-column pair to an array index.
		/// </summary>
		/// <remarks>
		///   <para>
		///     The index is calculated by going through each row until the end
		///     and then jumping to the beginning of the next row (rows and
		///     columns start at 1).
		///   </para>
		///   <para>
		///     We need this because the vertex- and triangles arrays of the
		///     mesh are one-dimensional structures, unlike out two-dimensional
		///     height map.
		///   </para>
		/// </remarks>
		private int RowColumn2Index(int row, int column) {
			var columns = heightMap.GetLength(1);
			return row * columns + column;
		}
#endregion  // Private methods
	}
}
