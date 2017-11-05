using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionRazz : Action, IAction {
  public GameObject effectPrefab;

  public int TpCost(){
    return(40);
  }

  public int MpCost(){
    return(5);
  }

  public int RadialDistance(){
    return(0);
  }

  public string Name(){
    return("Razz");
  }
  
  public string Description(){
    return("Annoy your target. Reduces attack for three turns.");
  }

  public int MaxDistance(){
    return(4);
  }

  public bool CanTargetSelf(){
    return(false);
  }

  public void ReceiveVisualFeedback(Cursor cursor){
    if(cursor.standingUnit){
      GameObject effect = Instantiate(effectPrefab, Vector3.zero, Quaternion.identity);
      cursor.standingUnit.ReceiveBuff(effect);
    }
    Unit().FinishAction();
  }

  public bool NeedsLineOfSight(){
    return(false);
  }
}
