using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionMeteor : UnitAction {

  public override int RadialDistance(){
    return(2);
  }

  public override string Name(){
    return("Meteor");
  }
  
  public override string Description(){
    return("Summon a meteor");
  }

  public override int MinDistance(){
    return(3);
  }

  public override int MaxDistance(){
    return(5);
  }

  public override bool CanTargetSelf(){
    return(true);
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    List<Cursor> tiles = Helpers.GetRadialTiles(cursor.xPos, cursor.zPos, this.RadialDistance(), 10, true);
    foreach(Cursor tile in tiles){
      if(tile.standingUnit){
        SendDamage(12, tile.standingUnit);
      }
    }
    Unit().FinishAction();
  }

  public override ActionType actionType(){
    return(ActionType.Magic);
  }
}