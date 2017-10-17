using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionThrowStone : Action, IAction {
  public GameObject stonePrefab;

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

  public void BeginAction(GameObject targetObject){
    if(NetworkServer.active) used = true;

    Cursor cursor = targetObject.GetComponent<Cursor>();
    GameObject stoneObject = Instantiate(stonePrefab, Unit().transform.position, Quaternion.identity);
    Stone stone = stoneObject.transform.Find("Stone").GetComponent<Stone>();
    stone.action = this;
    stone.cursor = cursor;
  }

  public void DoAction(Cursor cursor){
    if(cursor.standingUnit){
      cursor.standingUnit.ReceiveDamage(5);
    }
    Unit().FinishAction();
  }

  public bool NeedsLineOfSight(){
    return(true);
  }
}
