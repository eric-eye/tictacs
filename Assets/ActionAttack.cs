using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAttack : MonoBehaviour, IAction {
  public int TpCost(){
    return(25);
  }

  public int MpCost(){
    return(0);
  }

  public string Name(){
    return("Attack");
  }

  public int MaxDistance(){
    return(1);
  }

  public bool CanTargetSelf(){
    return(false);
  }

  public void DoAction(Cursor cursor){
    if(cursor.standingUnit){
      cursor.standingUnit.ReceiveDamage(15);
    }
  }

  public bool NeedsLineOfSight(){
    return(false);
  }
}
