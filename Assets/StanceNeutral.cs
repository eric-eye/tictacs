using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceNeutral : Stance {

  public override int NegotiateDamage(int damage){
    print("stanceNeutral");
    return(damage);
  }

  public override int NegotiateMoveLength(int moveLength){
    return(moveLength);
  }

  public override string Name(){
    return("Neutral");
  }
}
