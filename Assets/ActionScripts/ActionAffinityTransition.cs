using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionAffinityTransition : UnitAction
{
  private int damage = 2;
  private int targetsToResolve = 0;
  private List<Unit> unitsAffected = new List<Unit>();

  public override string Name()
  {
    return ("Ripple");
  }

  public override string Description()
  {
    return ("Transitions affinity backwards in the triangle and causes " + damage + " damage. Hops 2 squares radially.");
  }

  public override int MaxDistance()
  {
    return (4);
  }

  public override void ReceiveVisualFeedback(Cursor cursor)
  {
    if (cursor.standingUnit)
    {
      targetsToResolve--;
      SendDamage(damage, cursor.standingUnit);
      DoThing(cursor.standingUnit);

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

  private void DoThing(Unit unit)
  {
    Helpers.Affinity newAffinity = Helpers.Affinity.None;

    if (unit.affinity == Helpers.Affinity.Fire)
    {
      newAffinity = Helpers.Affinity.Earth;
    }
    else if (unit.affinity == Helpers.Affinity.Earth)
    {
      newAffinity = Helpers.Affinity.Water;
    }
    else if (unit.affinity == Helpers.Affinity.Water)
    {
      newAffinity = Helpers.Affinity.Fire;
    }

    unit.SetAffinity(newAffinity);
  }

  public override ActionType actionType()
  {
    return (ActionType.Magic);
  }
}

