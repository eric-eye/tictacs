.. This document is using the reStructuredText markup format
.. default-role:: code

###############
Endless 3D grid
###############

In this example we are  displaying a grid in a 3D  world which scales depending
on the height of the camera.  We use a  rectangular grid in this case,  but the
code can be adjusted for any other kind of grid as well.

There are two ways we can approach this challenge: either display oen grid that
continuously scales,  or display two grids with a fixes spacing where one fades
out and the other fades in as the camera rises and sinks. The latter is similar
to how the grid in the Unity scene view works.


Files overview
##############

===============  ===================================================
Name             Description
===============  ===================================================
InfinityGrid.cs  The script that manages the grid.
Orbiting.cs      Attached to every planet to make it orbit the sun.
SpaceFlight.cs   Attached to the camera to fly around in space.
===============  ===================================================


How it works
############

As  mentioned  above there  are  two  modes,  I  will  explain them  in  detail
individually.  The grids are wrapped up in a custom `struct` which exposes less
API and provides a convenience wrapper for our use-case.  During every frame we
call the  wrapper's  `Update`  method  with the  height  of the  camera  as the
argument,  and then we  adjust the far clipping  plane of the camera to make it
look good.

This approach  requires two  cameras:  one is  your usual  main camera  for the
scene,  the second  camera is placed exactly at the same location,  but it only
renders the grids. See the scene for how exactly the cameras are sat up.


Dual-grid
=========

The dual-grid setup has two grids.  At first the primary  grid has full opacity
and the secondary grid, which is larger, is transparent.  As we move upwards we
decrease  the opacity  of the  primary  grid and  increase  the opacity  of the
secondary grid.  Once a threshold has been reached we swap the roles of the tow
grids,  the secondary  becomes primary  and  vice-versa.  We also  increase the
spacing of the new secondary grid to the square of the new primary grid.

.. code-block:: c#

   float level = height >= 0 ? Mathf.FloorToInt(height / _step)
                             : Mathf.CeilToInt( height / _step);
   
   _mainGrid.Spacing = Mathf.Pow(_spacing, level) * Vector3.one;
   _subGrid.Spacing  = _spacing * _mainGrid.Spacing;


Flex-grid
=========

We only need one grid and we increase its spacing linearly.

.. code-block:: c#

   _mainGrid.Spacing = Mathf.Max(0.5f, height) * Vector3.one;
