using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionAffinityWipe : UnitAction {
  public GameObject effectPrefab;
  private int targetsToResolve = 0;
  private List<Unit> unitsAffected = new List<Unit>();

  public override string Name(){
    return("Absorb Affinity");
  }

  public override string Description(){
    return("Absorb target's affinity. Target loses affinity. Earn 2 MP per water affinity, 5 HP per earth affinity, and 1 TP per fire affinity. Hops radially 2 squares.");
  }

  public override int MaxDistance(){
    return(4);
  }

  public override void ReceiveVisualFeedback(Cursor cursor)
  {
    if (cursor.standingUnit)
    {
      targetsToResolve--;
      DoThing(cursor);

      List<Cursor> tiles = Helpers.GetRadialTiles(cursor.xPos, cursor.zPos, 2, 10, true);
      foreach (Cursor tile in tiles)
      {
        if (tile.standingUnit && !unitsAffected.Contains(tile.standingUnit))
        {
          unitsAffected.Add(tile.standingUnit);
          targetsToResolve++;
          StartCoroutine(DoScript(tile));
        }
      }
    }

    if (targetsToResolve == 0) Unit().FinishAction();
  }

  protected override void DoAction(Cursor cursor)
  {
    if (cursor.standingUnit)
    {
      targetsToResolve++;
      unitsAffected.Add(cursor.standingUnit);
    }

    CreateVisual(cursor, cursor.transform.position);
  }

  private IEnumerator DoScript(Cursor tile)
  {
    CreateVisual(tile, tile.transform.position);
    yield return new WaitForSeconds(1f);
  }

  private void DoThing(Cursor cursor){
    if(cursor.standingUnit){
      if(cursor.standingUnit.affinity == Helpers.Affinity.Water){
        Unit().HealMp(2);
      }else if(cursor.standingUnit.affinity == Helpers.Affinity.Earth){
        Unit().HealDamage(5);
      }else if(cursor.standingUnit.affinity == Helpers.Affinity.Fire){
        Unit().IncreaseTurnPoint(1);
      }

      cursor.standingUnit.SetAffinity(Helpers.Affinity.None);
    }
    Unit().FinishAction();
  }

  public override ActionType actionType() {
    return(ActionType.Magic);
  }
}

