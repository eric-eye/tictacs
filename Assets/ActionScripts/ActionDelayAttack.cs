using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionDelayAttack : UnitAction
{
    private int tp = 1;
    private int damage = 5;

    public override string Name()
    {
        return("Delay Attack");
    }

    public override string Description()
    {
        return("Stuns target, reducing their TP by " + tp + ". Causes " + damage + " damage.");
    }

    public override int MaxDistance()
    {
        return(1);
    }

    public override void ReceiveVisualFeedback(Cursor cursor)
    {
        StartCoroutine(DoScript(cursor));
    }

    private IEnumerator DoScript(Cursor cursor)
    {
        if (cursor.standingUnit)
        {
            SendDamage(damage, cursor.standingUnit);
        }

        yield return new WaitForSeconds(3f);
        if (cursor.standingUnit)
        {
            cursor.standingUnit.DecreaseTurnPoint(tp);
        }

        Unit().FinishAction();
    }

    public override ActionType actionType()
    {
        return(ActionType.Melee);
    }

    public override int MaxHeightDifference(){
        return(1);
    }
}