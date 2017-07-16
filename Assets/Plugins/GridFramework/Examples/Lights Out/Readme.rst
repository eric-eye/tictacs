.. This document is using the reStructuredText markup format
.. default-role:: code

################################
Lights-out game with polar grids
################################

This is a puzzle game where the goal is to  turn off all the lights by clicking
them.  Every time the player clicks a  light that light and all adjacent lights
toggle their state. In this example the lights have been randomly set and it is
not guaranteed that the game can actually be solved.

This example demonstrates several concepts: at the top-level we are writing out
game logic entirely using  grid-coordinates and we don't concern ourselves with
how it all maps to world-space.

We also  use a  custom extension  method to  keep the game logic clean from any
implementation details.  The extension  method contains  all the  logic that is
required to decide when two tiles are adjacent in the grid.

Finally, all the  tiles are generated at runtime: we  use the grid's properties
to compute the positions of the light bulbs. Even the grid is generated, making
building the game  fuly automated. This is very useful  for level designers who
can then write the individual puzzles in a text-based level format and have the
file parsed. See the *Level Parsing* example for a demonstration.


Files overview
##############

=======================   ===================================================
Name                      Description
=======================   ===================================================
LightsOut.cs              Set up the scene from puzzle data.
LightBulb.cs              The individual behaviour of every light.
LightsManager.cs          Coordinates handling of click events.
Extensions.cs             Extension method for rectangular grids.
Sprites/                  Sprites of the light bulbs.
=======================   ===================================================


How it works
############

As mentioned  above the example  implements different concepts. At  the topmost
level when  the player clicks a  tile an event  is fired. Every single  tile is
subscribed to that event and reacts accordingly.


Game logic
==========

On start  every grid subscribes to  the click event  and when a light  has been
clicked the  lights manager will send  and event. The clicked  light passes its
own position to the manager, the manager  passes it on to every receiving light
and every light  compares it to its  own position. If the light  is adjacent it
toggles its  state, and adjacency is  determined by an extension  method of the
grid.

.. code-block:: c#

   var isAdjacent = grid.IsAdjacent(transform.position, reference);
   
   if (isAdjacent) {
      _isOn = !_isOn;
      SwitchLights();
   }


Determining adjacency
=====================

The rules for adjacency are very  simple: if the difference between coordinates
is :math:`-1`, :math:`0` or :math:`1` they  are adjacent. If we want to exclude
diagonals we require  that the sum of  the absolute difference is  no more than
:math:`1`. Or in other words: the Manhattan distance is less or equal to one.

.. code-block:: c#

   bool AreAdjacent(this RectGrid grid, Vector3 a, Vector3 b) {
      var u = grid.WorldToGrid(a);
      var v = grid.WorldToGrid(b);

      var manhattanDistance = AbsDelta(u.x, v.x) + AbsDelta(u.y, v.y);
      return manhattanDistance <= 1.25;
   }

   float AbsDelta(float a, float b) {
      return Mathf.Abs(a - b);
   }
