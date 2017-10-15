.. This document is using the reStructuredText markup format
.. default-role:: code

##################################
Grid-based movement with obstacles
##################################

This example demonstrates two things: moving  from tile to tile in a grid-based
fashion and using a  grid to map between the game world  and a tile-based model
world. Every  time the player picks  a direction the script  checks whether the
target tile is occupied or not.

The example employs  a weak form of the model-view-controller  pattern, but the
distinction is not as strict as in a real MVC implementation.


Files overview
##############

=================    ==========================================================
Name                 Description
=================    ==========================================================
ForbiddenTiles.cs    Model- and controller script for the game.
RoamGrid.cs          Attached to the player to make it move around the world.
ModelGrid.cs         Attached to the grid so it can register itself to the
                     controller.
BlockSquare.cs       Attached to each obstacle so can register itself to the
                     model.
=================    ==========================================================


How it works
############

The game is managed by a static class that maintains a reference to a grid. The
game is modelled  by a two-dimensional array of `bool`  values, where the index
into the array  corresponds to what the  playing field looks like.  The size of
the array is determined by the range of the renderer.

.. code-block:: c#

   var rows    = Mathf.FloorToInt(renderer.To.x);
   var columns = Mathf.FloorToInt(renderer.To.y);

   _tiles = new bool[rows, columns];

After the  matrix has been built  we need to  mark the occupied tiles.  This is
done  by the  obstacles individually:  each one  sends its  coordinates to  the
manager, the manager  computes the index into  the array and sets  the value to
`false`.

.. code-block:: c#

   tile = _grid.WorldToGrid(tile);
   var row = Mathf.FloorToInt(tile.x);
   var column = Mathf.FloorToInt(tile.y);

   _tiles[row, column] = false;

Moving the player works  by first picking a direction from  user input and then
checking the destination in the model.

.. code-block:: c#

   // direction is one of Vector3.right, left, up or down
   _goal = _grid.WorldToGrid(transform.position) + direction;

   // Check for walls
   if (!ForbiddenTiles.CheckTile(_goal)) {
      Debug.Log("Ouch!");
      return;
   }

   _goal = _grid.GridToWorld(_goal);

When we have the  destination we use the above method to  find the index, check
the value of the matrix and then either accept or reject the target.

.. code-block:: c#

   var row = Mathf.FloorToInt(tile.x);
   var column = Mathf.FloorToInt(tile.y);

   return _tiles[row, column];
