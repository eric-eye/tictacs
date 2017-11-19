using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionMeteor : Action {

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
    List<Cursor> tiles = Helpers.GetRadialTiles(cursor.xPos, cursor.zPos, this.RadialDistance(), true);
    foreach(Cursor tile in tiles){
      if(tile.standingUnit){
        tile.standingUnit.ReceiveDamage(12, Unit());
      }
    }
    Unit().FinishAction();
  }
}