using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class ActionSpinAttack : Action
{
    private int targetsToResolve = 0;

    public override string Name()
    {
        return ("Spin Attack");
    }

    public override string Description()
    {
        return ("Attack all units in adjacent tiles");
    }

    public override void ReceiveVisualFeedback(Cursor cursor)
    {
        if (cursor.standingUnit)
        {
            cursor.standingUnit.ReceiveDamage(15);
            targetsToResolve -= 1;
        }
        if(targetsToResolve == 0) Unit().FinishAction();
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
        List<Cursor> allTiles = Helpers.GetRadialTiles(cursor.xPos, cursor.zPos, this.RadialDistance(), true);
        List<Cursor> tiles = allTiles.Where(tile =>
        {
            return tile.standingUnit && tile.standingUnit != Unit();
        }).ToList();
        targetsToResolve += tiles.Count;

        foreach (Cursor tile in tiles)
        {
            CreateVisual(tile, tile.transform.position);
        }
    }
}
