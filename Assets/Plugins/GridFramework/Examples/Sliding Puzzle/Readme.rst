.. This document is using the reStructuredText markup format
.. default-role:: code

##############
Sliding puzzle
##############

Unity's physics engine is very good if you are looking for physically realistic
behaviour,  but if you are looking for a  rigid unrealistic model where you can
slide blocks  right past each  other without friction  it can be easier to take
things into your own hands.  In this example we use  Grid Framework to create a
new model for how objects are supposed to interact with each other.


Files overview
##############

================  =========================================================
Name              Description
================  =========================================================
SlidingPuzzle.cs  The main script, it coordinates the entire game.
PuzzleGrid.cs     Attach to the grit, initializes the game on start.
DragBlock.cs      The dragging rules for blocks.
================  =========================================================


How it works
############

The basic idea behind  this example is that we  keep track of the sctate of the
game using a two-by-two array (matrix)  of type `bool` where an entry is `true`
if the corresponding grid cell is free and `false` if it is occupied.

The puzzle is being kept track of in the static `SlidingPuzzle` class. When the
game starts the  `PuzzleGrid` script that has  been attached to the grid object
initializes the game by  passing the grid and  its renderer to `SlidingPuzzle`.
We need the renderer because we  use its `From` and `To` to compute the size of
the matrix. The matrix is build with all squares set to be free.

Once the puzzle has been initialized the block need to set their matrix entries
to occupied.  To this end we break  each block  into one-by-one  units and find
their individual matrix entries and set them to occupied.

When a block is  being dragged we reverse the process:  we find the entries and
set them to free. Then we compute the bound for moving the block by finding the
largest possible  rectangular sub-matrix  and using  the lower  left and  upper
right corners  (in world coordinates)  and the bounds  for the block.  When the
block gets released its new position is set to occupied.
