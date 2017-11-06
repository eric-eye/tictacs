using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ActionLightningStab : Action, IAction
{

    int targetsToResolve = 0;

    public int TpCost()
    {
        return (35);
    }

    public int MpCost()
    {
        return (0);
    }

    public int RadialDistance()
    {
        return (0);
    }

    public string Name()
    {
        return ("Lightning Stab");
    }

    public string Description()
    {
        return ("Strike all units with lightning in a line");
    }

    public int MaxDistance()
    {
        return (1);
    }

    public bool CanTargetSelf()
    {
        return (false);
    }

    public CursorModes CursorMode()
    {
        return (CursorModes.Line);
    }

    protected override void DoAction(Cursor cursor)
    {
        List<Cursor> allTiles = Helpers.GetLineTiles(Unit().xPos, Unit().zPos, cursor.xPos, cursor.zPos);

        List<Cursor> tiles = allTiles.Where(tile => tile.standingUnit).ToList();

        targetsToResolve += tiles.Count;

        if (targetsToResolve == 0)
        {
            Unit().FinishAction();
        }
        else
        {
            StartCoroutine(DoScript(tiles));
        }
    }

    private IEnumerator DoScript(List<Cursor> tiles){
        foreach (Cursor tile in tiles)
        {
            CreateVisual(tile, tile.transform.position);
            yield return new WaitForSeconds(1f);
        }
    }

    public void ReceiveVisualFeedback(Cursor cursor)
    {
        if (cursor.standingUnit)
        {
            cursor.standingUnit.ReceiveDamage(5);
            targetsToResolve -= 1;
        }
        if(targetsToResolve == 0) Unit().FinishAction();
    }

    public bool NeedsLineOfSight()
    {
        return (false);
    }
}
