using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionIntimidate : UnitAction
{
    public GameObject effectPrefab;

    public override string Name()
    {
        return("Intimidate");
    }

    public override string Description()
    {
        return("Intimidate target. They won't be able to move within a radius of 2 of the caster.");
    }

    public override int MaxDistance()
    {
        return(6);
    }

    public override int MpCost(){
        return(5);
    }

    public override void ReceiveVisualFeedback(Cursor cursor)
    {
        if (cursor.standingUnit)
        {
            GameObject effect = Instantiate(effectPrefab, Vector3.zero, Quaternion.identity);
            cursor.standingUnit.ReceiveBuff(effect);
            effect.GetComponent<BuffIntimidate>().unapproachable = Unit();
        }

        Unit().FinishAction();
    }

    public override bool NeedsLineOfSight()
    {
        return(true);
    }

    public override bool HeightAssisted()
    {
        return(true);
    }

    protected override void DoAction(Cursor cursor)
    {
        CreateVisual(cursor, Unit().transform.position);
    }

    public override ActionType actionType()
    {
        return(ActionType.Ranged);
    }
}