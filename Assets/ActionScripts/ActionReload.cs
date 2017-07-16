using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionReload : UnitAction {

  public override string Name(){
    return("Reload");
  }
  
  public override string Description(){
    return("Replenishes all of this unit's MP.");
  }

  public override int MaxDistance(){
    return(0);
  }

  public override bool CanTargetSelf(){
    return(true);
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    Unit().HealMp(Unit().maxMp);
    Unit().FinishAction();
  }

  public override ActionType actionType(){
    return(ActionType.Support);
  }
}
