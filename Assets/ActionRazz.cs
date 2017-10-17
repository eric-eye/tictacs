using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionRazz : Action, IAction {
  public GameObject effectPrefab;
  public GameObject razzPrefab;

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

  public void BeginAction(GameObject targetObject){
    if(NetworkServer.active) used = true;

    Cursor cursor = targetObject.GetComponent<Cursor>();
    GameObject razzObject = Instantiate(razzPrefab, cursor.transform.position, Quaternion.identity);
    Razz razz = razzObject.transform.Find("Razz").GetComponent<Razz>();
    razz.action = this;
    razz.cursor = cursor;
  }

  public void DoAction(Cursor cursor){
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
