using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMark : Buff {
  public Unit lockedTarget;

  public override string Name(){
    return("Mark");
  }

  public override bool CanBeSeenThroughObjects(){
    return(true);
  }

  public override string Description(){
    return("Does not need line-of-sight to be targeted by ranged abilities.");
  }
}
