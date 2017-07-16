.. This document is using the reStructuredText markup format
.. default-role:: code

#################
Rotary phone dial
#################

A rotary phone dial that  rotates around its  origin and reports the number the
player has clicked. The animation is generated at runtime using a coroutine.


Files overview
##############

===================  =========================================================
Name                 Description
===================  =========================================================
RotaryDial.cs        Behaviour of the dial object.
Materials/           A number of image resource for the texture.
===================  =========================================================


How it works
############

First the  player clicks  the sprite  and we convert the  clicked point to grid
coordinates to get  the sector that was clicked.  The sector corresponds to the
number on the dial,  minus some offset if the numbers don't  start in the first
sector.

.. code-block:: c#

   var face = _grid.NearestFace(hit.point, system);
   var number = Mathf.FloorToInt(face.y) - (_offset - 1);

Once we  have the  number  we  can use  it for  our gameplay  purpose,  such as
generating the rotation or logging the number to the console.
