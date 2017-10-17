using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceNeutral : Stance, IStance {

  public int NegotiateDamage(int damage){
    return(damage);
  }

  public int NegotiateMoveLength(int moveLength){
    return(moveLength);
  }

  public string Name(){
    return("Neutral");
  }
}
