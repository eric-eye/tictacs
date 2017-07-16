using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSickness : Buff {
  int damage = 5;

  public override string Name(){
    return("Sickness");
  }

  public override void OnTurnStart(){
    unit.DamageMp(damage);
  }

  public override float ReturnMpMod(){
    return(0);
  }

  public override UnitAction.ActionType CommunicableType()
  {
    return (UnitAction.ActionType.Melee);
  }

  public override string Description(){
    return("Lose " + damage + " MP and recover no MP per turn. Other units who make melee contact with this unit will contract " + Name() + ".");
  }
}
