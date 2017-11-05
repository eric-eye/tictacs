using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionFire : Action, IAction {
  public int TpCost(){
    return(35);
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
    List<Cursor> affectedCursors = new List<Cursor>();
    foreach(int[] coordinates in Helpers.GetAllPaths(cursor.xPos, cursor.zPos, this.RadialDistance(), true)){
      print(coordinates[0] + ", " + coordinates[1]);
      Cursor tile = Helpers.GetTile(coordinates[0], coordinates[1]);
      if(tile) {
        if(affectedCursors.Contains(tile)) {
          continue;
        }

        affectedCursors.Add(tile);

        if(tile.standingUnit){
          tile.standingUnit.ReceiveDamage(12);
        }
      }
    }
    Unit().FinishAction();
  }

  public bool NeedsLineOfSight(){
    return(false);
  }
}
