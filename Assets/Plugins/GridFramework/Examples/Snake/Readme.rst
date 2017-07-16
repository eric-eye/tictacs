.. This document is using the reStructuredText markup format
.. default-role:: code

##########
Snake game
##########

A classical game of Snake where a chain of segments moves through the grid.  We
use boundary-checking to keep the snake inside the grid.  This example does not
use  any  complicated  grid  API,  we  only  generate  direction  vectors  from
properties. This makes the example well-suited for beginners.


Files overview
##############

==============  =========================================================
Name            Description
==============  =========================================================
Snake.cs        Behaviour of the entire snake.
Segment.prefab  Prefab for each of the segments.
==============  =========================================================


How it works
############

We won't be keeping track of any model state, we do all our game logic directly
in world space. The movement vectors are made from properties of grid.

.. code-block:: c#

   private Vector3 DirectionToVector(Direction dir) {
      var up    = _grid.Up;
      var right = _grid.Right;
      switch (dir) {
         case Direction.N: return  up;
         case Direction.S: return -up;
         case Direction.E: return  right;
         case Direction.W: return -right;
      }
   }

To check whether a  target position would be  outside the grid we convert it to
grid-coordinates and compare the result to the range of the renderer.

.. code-block:: c#

   private bool OutsideRange(Vector3 position) {
      var local = _grid.WorldToGrid(position);
      var from  = _renderer.From;
      var   to  = _renderer.To;
   
      var x = local.x;
      var y = local.y;
   
      if (x > to.x || y > to.y ||x < from.x || y < from.y){
         return true;
      }
      return false;
   }
