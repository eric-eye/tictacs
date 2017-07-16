using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionMendWounds : UnitAction
{
    public override string Name()
    {
        return("Mend Wounds");
    }

    public override string Description()
    {
        return("Mend your own wounds, healing 33% of the caster's missing HP.");
    }

    public override int MaxDistance()
    {
        return(0);
    }

    public override bool CanTargetSelf()
    {
        return(true);
    }

    public override int MpCost(){
        return(5);
    }

    public override void ReceiveVisualFeedback(Cursor cursor)
    {
        if (cursor.standingUnit)
        {
            cursor.standingUnit.HealDamage((cursor.standingUnit.maxHp - cursor.standingUnit.currentHp) / 3);
        }

        Unit().FinishAction();
    }

    public override ActionType actionType()
    {
        return(ActionType.Support);
    }

    private int HpToHeal(){
        return(Mathf.RoundToInt(Unit().maxHp - Unit().currentHp) / 3);
    }
}