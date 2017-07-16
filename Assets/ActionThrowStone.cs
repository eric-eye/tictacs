using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionThrowStone : MonoBehaviour, IAction {
  public int TpCost(){
    return(30);
  }

  public int MpCost(){
    return(0);
  }

  public string Name(){
    return("Throw Stone");
  }

  public int MaxDistance(){
    return(5);
  }

  public bool CanTargetSelf(){
    return(false);
  }

  public void DoAction(Cursor cursor){
    if(cursor.standingUnit){
      cursor.standingUnit.ReceiveDamage(5);
    }
  }

  public bool NeedsLineOfSight(){
    return(true);
  }
}
