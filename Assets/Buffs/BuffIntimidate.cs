using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIntimidate : Buff {
  public Unit unapproachable;
  private int radius = 2;

  public override string Name(){
    return("Intimidate");
  }

  public override bool CanMoveTo(Cursor target){
    Cursor cursor = Helpers.GetTile(unapproachable.xPos, unapproachable.zPos);
    return(Helpers.Distance(cursor, target) > radius);
  }

  public override string Description(){
    return("Cannot move within " + radius + " tiles of " + unapproachable.unitName);
  }
}
