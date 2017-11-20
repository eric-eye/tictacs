using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceDefend : Stance {

  public override int NegotiateDamage(int damage){
    return(damage - 4);
  }

  public override int NegotiateMoveLength(int moveLength){
    return(moveLength/2);
  }

  public override string Name(){
    return("Defend");
  }
}
