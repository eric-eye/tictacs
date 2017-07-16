using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceNeutral : Stance {

  public override float NegotiateDamage(float damage, UnitAction action){
    return(damage);
  }

  public override float NegotiateMoveLength(float moveLength){
    return(moveLength);
  }

  public override string Name(){
    return("Neutral");
  }

  public override string Description(){
    return("No effect when attacked. This stance is automatically used when successfully back-attacked.");
  }
}
