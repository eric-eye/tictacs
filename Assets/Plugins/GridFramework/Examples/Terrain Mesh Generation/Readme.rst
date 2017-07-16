.. This document is using the reStructuredText markup format
.. default-role:: code

#######################
Terrain mesh generation
#######################

We can use the  grid to generate and manipulate a mesh  at runtime. The initial
shape of  the map is loaded  from a height map  array that lists the  height of
every vertex,  and while the game  is running the  player can raise or  lower a
vertex by left- or right-clicking it.

Files overview
##############

======================  =======================================================
Name                    Description
======================  =======================================================
TerrainMeshBuilder.cs   Build and manipulate the mesh and maintain height data.
======================  =======================================================


How it works
############

The height  map is a two-dimensional  `[row, column]` array of  integers, every
integer represents  the height of  the corresponding index in  grid-units. When
the game loads we build vectors based on the row, column and height.

.. code-block:: c#

   var vertices = new Vector3[rows * columns];
   for (var r = 0; r < rows; r++){
      for (var c = 0; c < columns; c++) {
         var h = heightMap[r, c];

         // grid coordinates -> world coordinates -> local coordinates
         var  gridVertex = new Vector3(c, h, -r);
         var worldVertex = _grid.GridToWorld(gridVertex);
         var localVertex = transform.InverseTransformPoint(worldVertex);

         // add the vertex to the vertices
         vertices[r * rows + c] = localVertex;
      }
   }

The state of our game is expressed entirely in grid-coordinates and in this one
loop we convert that  information to local space. From there on  it is the same
as any other mesh manipulation in Unity.

When we want  to raise or lower  a vertex we first get  the grid-coordinates of
where the player is pointing at. A raycast against the mesh's collider gives us
the world-position and we use the grid to convert it to grid-coordinates, which
we then use to index into the height map.

.. code-block:: c#

   RaycastHit hit;
   _mc.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
               out hit, Mathf.Infinity);
   
   var gridPoint = _grid.WorldToGrid(hit.point);
   var row    = Mathf.RoundToInt(-gridPoint.z);
   var column = Mathf.RoundToInt( gridPoint.x);

To raise a vertex we get its index and then add a multiple of the grid's `Up`
to that vertex and re-assign the vertices back to the mesh.

.. code-block:: c#

   heightMap[row, column] += raise;

