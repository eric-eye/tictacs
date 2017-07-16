using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ActionLightningStab : UnitAction
{
    private int targetsToResolve = 0;

    public override string Name()
    {
        return ("Lightning Stab");
    }

    public override string Description()
    {
        return ("Strike all units with lightning in a line");
    }

    public override int MaxDistance()
    {
        return (1);
    }

    public override CursorModes CursorMode()
    {
        return (CursorModes.Line);
    }

    public override int LineDistance()
    {
        return (9999);
    }

    public override void ReceiveVisualFeedback(Cursor cursor)
    {
        if (cursor.standingUnit)
        {
            SendDamage(5, cursor.standingUnit);
            targetsToResolve -= 1;
        }
        if(targetsToResolve == 0) Unit().FinishAction();
    }

    protected override void DoAction(Cursor cursor)
    {
        List<Cursor> allTiles = Helpers.GetLineTiles(Unit().xPos, Unit().zPos, cursor.xPos, cursor.zPos, LineDistance());
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

    public override ActionType actionType(){
        return(ActionType.Magic);
    }
}
