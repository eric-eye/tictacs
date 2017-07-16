.. This document is using the reStructuredText markup format
.. default-role:: code

############################
Assembling a level from data
############################

If you  have a game that  is made of regular  tiles you might want  to load the
levels  from data.  In this  example  we use  an  array to  describe the  level
information and Grid Framework to position the tiles in the world accordingly.

You can use this example as a basis for loading the level data from a file
instead of using hard-coded arrays.


Files overview
##############

===============  ===================================================
Name             Description
===============  ===================================================
LevelBuilder.cs  The only script of the example.
Colors/          Materials for the blocks.
Prefabs/         Prefabs of the blocks.
===============  ===================================================


How it works
############

A level is described by a two-dimensional array like this one:

.. code-block:: c#

   private static int[,] _level1 = {
      {1, 2, 3, 2, 3, 2, 3, 1, 1, 2, 1}, 
      {3, 1, 2, 1, 0, 2, 1, 2, 3, 1, 3}, 
      {2, 0, 3, 2, 0, 1, 0, 1, 2, 3, 2}, 
      {3, 1, 0, 0, 2, 3, 1, 2, 3, 1, 3}, 
      {2, 0, 3, 2, 3, 2, 3, 1, 2, 3, 2}, 
   };

Each number corresponds to one colour: one for red, two for green and three for
blue. The row and column of each entry are used as the *X*- and *Y* coordinates
of the corresponding block. We loop over the array when assembling the level:

.. code-block:: c#

   for (var r = 0; r < level.GetLength(0); ++r) {
      for (var c = 0; c < level.GetLength(1); ++c) {
         // The real meat is in here
      }
   }

Let's see how we turn the row `r` and the column `c` into a world-position:

.. code-block:: c#

   var position = _grid.HerringUpToWorld(new Vector3(c, r, 0));

We use  hexagonal grids in this  example, but it  works with any other  type of
grid as well,  just replace the herringbone coordinate system  with the one you
want to use  instead. The rest of  the script is about  instantiating the block
when we have its position.

There is one minor caveat:  the text file starts in the upper right-hand corner
while the grid starts in the lower right-hand corner. It would be preferable if
the layout of the characters matched that of the grid. For this reason the grid
has been rotated 180° along its *X*-axis.
