using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionFire : UnitAction {

  public override int RadialDistance(){
    return(2);
  }

  public override string Name(){
    return("Fire");
  }
  
  public override string Description(){
    return("Cast a fire spell. Direct damage.");
  }

  public override int MaxDistance(){
    return(5);
  }

  public override bool CanTargetSelf(){
    return(true);
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    List<Cursor> tiles = Helpers.GetRadialTiles(cursor.xPos, cursor.zPos, 10, this.RadialDistance(), true);
    foreach(Cursor tile in tiles){
      if(tile.standingUnit){
        SendDamage(12, cursor.standingUnit);
      }
    }
    Unit().FinishAction();
  }

  public override ActionType actionType(){
    return(ActionType.Magic);
  }
}
