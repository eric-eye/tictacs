using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionFire : Action, IAction {
  public int TpCost(){
    return(35);
  }

  public CursorModes CursorMode(){
    return(CursorModes.Radial);
  }

  public int MpCost(){
    return(5);
  }

  public int RadialDistance(){
    return(2);
  }

  public string Name(){
    return("Fire");
  }
  
  public string Description(){
    return("Cast a fire spell. Direct damage.");
  }

  public int MaxDistance(){
    return(5);
  }

  public bool CanTargetSelf(){
    return(true);
  }

  public void ReceiveVisualFeedback(Cursor cursor){
    foreach(Cursor tile in Helpers.GetRadialTiles(cursor.xPos, cursor.zPos, this.RadialDistance(), true)){
      if(tile.standingUnit){
        tile.standingUnit.ReceiveDamage(12);
      }
    }
    Unit().FinishAction();
  }

  public bool NeedsLineOfSight(){
    return(false);
  }
}
