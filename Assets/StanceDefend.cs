using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceDefend : Stance, IStance {

  public int NegotiateDamage(int damage){
    return(damage - 4);
  }

  public int NegotiateMoveLength(int moveLength){
    return(moveLength/2);
  }

  public string Name(){
    return("Defend");
  }
}
