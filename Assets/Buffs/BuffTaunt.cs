using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTaunt : Buff {
  public override string Name(){
    return("Taunt");
  }

  public Unit lockedTarget;

  public override bool CanTarget(Cursor target){
    return(target.standingUnit == lockedTarget);
  }

  public override string Description(){
    return("Can only target " + lockedTarget.unitName + " with abilities.");
  }
}
