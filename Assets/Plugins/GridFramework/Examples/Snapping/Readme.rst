.. This document is using the reStructuredText markup format
.. default-role:: code

###############################
Snapping to the grid at runtime
###############################

Snap objects to the grid while the game is running. Objects can snap either to
faces or edges depending on their scale.


Files overview
##############

======================  =======================================================
Name                    Description
======================  =======================================================
SnappingUnits.cs        This is the script that does the snapping.
IntersectionTrigger.cs  Responsible for setting up the trigger that detects
                        intersection
ExampleDescription.cs   Displays a description of this example.
Materials/              Materials for normal and intersection state.
======================  =======================================================


How it works
############

This example is very simple because we can use the `AlignTransform` extension
method that is provided by Grid Framework. First we get the position the player
is pointing at via raycasting, then we correct that position by aligning it. We
do this every frame while the block is being dragged.

.. code-block:: c#

   _grid.AlignTransform(transform);

That's almost all there is to it. What is left now is correcting the
*Y*-coordinate because otherwise the block will be inside the floor.

.. code-block:: c#

   private Vector3 CalculateOffsetY() {
      //first store the objects position in grid coordinates
      var gridPosition = _grid.WorldToGrid(transform.position);
      //then change only the Y coordinate
      gridPosition.y = .5f * transform.lossyScale.y;
      
      //convert the result back to world coordinates
      return _grid.GridToWorld(gridPosition);
   }

This is a bit more complicated than it would have to be for such a simple
example, but it is universally correct.
