using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceDefendRanged : Stance {
  public override float NegotiateDamage(float damage, UnitAction action){
    if (action.actionType() == UnitAction.ActionType.Ranged)
    {
      return(damage / 2);
    }
    else if(action.actionType() == UnitAction.ActionType.Magic)
    {
      return(damage * 2);
    }
    else
    {
      return(damage);
    }
  }

  public override string Name(){
    return("Defend vs. Ranged");
  }

  public override string Description(){
    return("Reduces incoming ranged damage by half, but magic damage is doubled.");
  }
}
