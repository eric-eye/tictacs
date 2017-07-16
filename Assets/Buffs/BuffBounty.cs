using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBounty : Buff {

  int points = 20;

  public override string Name(){
    return("Bounty");
  }

  public override void OnTurnStart(){
    unit.AddPoints(points);
  }

  public override string Description(){
    return("Gains " + points +  " at the start of their turn.");
  }
}
