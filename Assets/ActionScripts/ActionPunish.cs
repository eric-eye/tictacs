using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class ActionPunish : UnitAction
{
    int highDamage = 30;
    int lowDamage = 20;

    public override string Name()
    {
        return ("Punish");
    }

    public override int MpCost(){
        return(5);
    }

    public override string Description()
    {
        return ("A precision attack which anticipates the defender's stances. If the defender's stance is a known stance, the defender incurs " + highDamage + ". Otherwise, " + lowDamage + " is incurred. If the opponent is using a neutral stance, " + lowDamage + " is incurred.");
    }

    public override void ReceiveVisualFeedback(Cursor cursor)
    {
        if (cursor.standingUnit)
        {
            Stance stance = cursor.standingUnit.Stance();
            int damage = stance.used && stance.Name() != "Neutral" ? highDamage : lowDamage;
            SendDamage(damage, cursor.standingUnit);
        }
        Unit().FinishAction();
    }

    public override ActionType actionType(){
        return(ActionType.Melee);
    }

    public override int MaxHeightDifference()
    {
        return (1);
    }
}
