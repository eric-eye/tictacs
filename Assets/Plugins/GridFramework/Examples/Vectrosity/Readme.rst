.. This document is using the reStructuredText markup format
.. default-role:: code

#########################
Vectrosity plugin support
#########################

Vectrosity is a 3rd party Unity plugin for rendering 3D vector lines and Grid
Framework offers support for it by allowing you to access the points of a
renderer in a way that is fit for Vectrosity.

To get this example to compile you have to define the preprocessor directives
for Vectrosity support. You can find detailed information in the user manual
chapter titled *Supporting other Unity plugins*, but here is the TL;DR: define
`GRID_FRAMEWORK_VECTROSITY`, for example by creating the file `smcs.rsp` in the
root of your *Assets* directory with the contents

.. code-block::

   -define:GRID_FRAMEWORK_VECTROSITY

This is needed so the scripts don't throw errors for users who don't have
Vectrosity installed.

Files overview
##############

=================  =======================================================
Name               Description
=================  =======================================================
Bounding.cs        The grid computes a line and then makes all the points
                   local.
ColorSwapping.cs   Rotates in place and changes colour of lines.
Resizing.cs        A hex grid that grows and shrinks its radius.
Materials/         A laser line style texture and material.
PhysicsMaterials/  Simply physics material for the bouncing grids.
=================  =======================================================


How it works
############

For this I will assume that you already know how to use Vectrosity. The only
thing left to do is get the points for the Vectrosity API, which can be done
using the official extension methods. Extension methods need to be explicitly
imported from their namespace.

.. code-block:: c#

   using GridFramework.Extensions.Vectrosity;

In UnityScript you use the `import` statement instead of `using`, otherwise it
is the same. We can then get the points with one one of code:

.. code-block:: c#

   _renderer = GetComponent<GridRenderer>();
   var points = _renderer.GetVectrosityPoints();

   var line = new VectorLine(lineName, points, texture, width);

From there on it is business as usual. In some examples the lines follow the
movement of the grid, so we have to make the point coordinates local and
assigne the grid's `transform` to the line:

.. code-block:: c#

   for (var i = 0; i < points.Count; ++i) {
      var point = points[i];
      var local = transform.InverseTransformPoint(point);
      points[i] = local;
   }
   
   line.drawTransform = transform;
