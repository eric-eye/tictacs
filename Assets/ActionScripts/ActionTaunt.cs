using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionTaunt : UnitAction
{
    public GameObject effectPrefab;

    public override string Name()
    {
        return("Taunt");
    }

    public override string Description()
    {
        return("Annoy your target. Causes them to only be able to target the caster. Lasts 3 turns.");
    }

    public override int MpCost(){
        return(5);
    }

    public override int MaxDistance()
    {
        return(4);
    }

    public override void ReceiveVisualFeedback(Cursor cursor)
    {
        if (cursor.standingUnit)
        {
            GameObject effect = Instantiate(effectPrefab, Vector3.zero, Quaternion.identity);
            cursor.standingUnit.ReceiveBuff(effect);
            effect.GetComponent<BuffTaunt>().lockedTarget = Unit();
        }

        Unit().FinishAction();
    }

    public override ActionType actionType()
    {
        return(ActionType.Support);
    }
}