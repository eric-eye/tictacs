using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ActionSickness : UnitAction
{
    public GameObject effectPrefab;

    public override string Name()
    {
        return ("Sickness");
    }

    public override string Description()
    {
        return ("Inflicts targets with sickness, causing them to lose 5 MP per turn and recover no MP. Sick targets infect other targets with whom melee contact is made.");
    }

    public override int MpCost(){
        return(2);
    }

    public override int MaxDistance()
    {
        return (4);
    }

    public override void ReceiveVisualFeedback(Cursor cursor)
    {
        if(cursor.standingUnit){
            GameObject effect = Instantiate(effectPrefab, Vector3.zero, Quaternion.identity);
            cursor.standingUnit.ReceiveBuff(effect);
        }

        Unit().FinishAction();
    }

    public override ActionType actionType(){
        return ActionType.Support;
    }

}
