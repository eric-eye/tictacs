using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceDefend : Stance {
  public override float NegotiateDamage(float damage, UnitAction action){
    return(damage/2);
  }
  
  public override float NegotiateMoveLength(float moveLength){
    return(moveLength * 0);
  }

  public override string Name(){
    return("Defend vs. All");
  }

  public override string Description(){
    return("Reduces all incoming damage by half, but this unit cannot move.");
  }
}
