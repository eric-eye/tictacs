using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class ActionPunish : Action
{
    private int targetsToResolve = 0;
    private int maxTargetsToResolve = 1;

    public override string Name()
    {
        return ("Punish");
    }

    public override int LineDistance()
    {
        return (2);
    }

    public override string Description()
    {
        return ("A basic attack with decent range. If the target is using a learned stance, more damage is incurred.");
    }

    public override CursorModes CursorMode()
    {
        return (CursorModes.Line);
    }

    public override void ReceiveVisualFeedback(Cursor cursor)
    {
        if (cursor.standingUnit)
        {
            Stance stance = cursor.standingUnit.Stance();
            int damage = stance.used ? 50 : 30;
            cursor.standingUnit.ReceiveDamage(damage);
            targetsToResolve -= 1;
        }
        if(targetsToResolve == 0) Unit().FinishAction();
    }

    protected override void DoAction(Cursor cursor)
    {
        List<Cursor> allTiles = Helpers.GetLineTiles(Unit().xPos, Unit().zPos, cursor.xPos, cursor.zPos, LineDistance());
        List<Cursor> tiles = allTiles.Where(tile => tile.standingUnit).ToList();
        foreach(Cursor tile in tiles) {
            if(tile.standingUnit) targetsToResolve++;
            if(targetsToResolve == maxTargetsToResolve) break;
        }

        if (targetsToResolve == 0)
        {
            Unit().FinishAction();
        }
        else
        {
            StartCoroutine(DoScript(tiles));
        }
    }

    private IEnumerator DoScript(List<Cursor> tiles)
    {
        int i = 0;
        foreach (Cursor tile in tiles)
        {
            if (i >= maxTargetsToResolve) break;
            CreateVisual(tile, tile.transform.position);
            yield return new WaitForSeconds(1f);
            i++;
        }
    }
}
