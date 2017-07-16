using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRazz : MonoBehaviour, IAction {
  public GameObject effectPrefab;

  public int TpCost(){
    return(40);
  }

  public int MpCost(){
    return(5);
  }

  public string Name(){
    return("Razz");
  }

  public int MaxDistance(){
    return(4);
  }

  public bool CanTargetSelf(){
    return(false);
  }

  public void DoAction(Cursor cursor){
    if(cursor.standingUnit){
      GameObject effect = Instantiate(effectPrefab, Vector3.zero, Quaternion.identity);
      cursor.standingUnit.ReceiveBuff(effect);
    }
  }

  public bool NeedsLineOfSight(){
    return(false);
  }
}
