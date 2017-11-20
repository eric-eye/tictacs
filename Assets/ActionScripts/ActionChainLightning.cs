using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ActionChainLightning : Action
{
    private int targetsToResolve = 0;
    private List<Unit> unitsAffected = new List<Unit>();

    public override string Name()
    {
        return ("Chain Lightning");
    }

    public override string Description()
    {
        return ("Strike all units with lightning in a line");
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
            cursor.standingUnit.ReceiveDamage(50, Unit());
        }

        List<Cursor> tiles = Helpers.GetRadialTiles(cursor.xPos, cursor.zPos, 2, true);
        foreach (Cursor tile in tiles)
        {
            if (tile.standingUnit && !unitsAffected.Contains(tile.standingUnit))
            {
                unitsAffected.Add(tile.standingUnit);
                targetsToResolve++;
                StartCoroutine(DoScript(tile));
            }
        }

        if(targetsToResolve == 0) Unit().FinishAction();
    }

    protected override void DoAction(Cursor cursor){
        if(cursor.standingUnit) {
            targetsToResolve++;
            unitsAffected.Add(cursor.standingUnit);
        }

        CreateVisual(cursor, cursor.transform.position);
    }

    private IEnumerator DoScript(Cursor tile){
        CreateVisual(tile, tile.transform.position);
        yield return new WaitForSeconds(1f);
    }
}
