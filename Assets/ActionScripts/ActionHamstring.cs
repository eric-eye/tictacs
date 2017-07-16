using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionHamstring : UnitAction
{
    int damage = 5;

    public GameObject effectPrefab;

    public override string Name()
    {
        return("Hamstring");
    }

    public override string Description()
    {
        return("Attack your target's leg to reduce their movement distance by half. Lasts 2 turns. Causes " + damage + " damage.");
    }

    public override void ReceiveVisualFeedback(Cursor cursor)
    {
        if (cursor.standingUnit)
        {
            SendDamage(damage, cursor.standingUnit);
            GameObject effect = Instantiate(effectPrefab, Vector3.zero, Quaternion.identity);
            cursor.standingUnit.ReceiveBuff(effect);
        }

        Unit().FinishAction();
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