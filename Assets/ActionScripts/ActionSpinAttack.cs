using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class ActionSpinAttack : UnitAction
{
    int damage = 15;
    private int targetsToResolve = 0;

    public override string Name()
    {
        return ("Spin Attack");
    }

    public override string Description()
    {
        return ("Attack all units in cardinal adjacent tiles with a spinning attack. Hits for " + damage + ".");
    }

    public override void ReceiveVisualFeedback(Cursor cursor)
    {
        if (cursor.standingUnit)
        {
            SendDamage(15, cursor.standingUnit);
            targetsToResolve -= 1;
        }

        if (targetsToResolve == 0) Unit().FinishAction();
    }

    public override int MaxDistance()
    {
        return (0);
    }

    public override bool CanTargetSelf()
    {
        return (true);
    }

    public override int RadialDistance()
    {
        return (1);
    }

    protected override void DoAction(Cursor cursor)
    {
        List<Cursor> allTiles = Helpers.GetRadialTiles(cursor.xPos, cursor.zPos, this.RadialDistance(), 1, true);
        List<Cursor> tiles = allTiles.Where(tile => { return tile.standingUnit && tile.standingUnit != Unit(); })
            .ToList();
        targetsToResolve += tiles.Count;

        foreach (Cursor tile in tiles)
        {
            CreateVisual(tile, tile.transform.position);
        }

        if (targetsToResolve == 0) Unit().FinishAction();
    }

    public override ActionType actionType()
    {
        return(ActionType.Melee);
    }

    public override int MaxHeightDifference()
    {
        return (1);
    }
}