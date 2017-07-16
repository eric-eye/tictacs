using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ActionFear : UnitAction
{
    int distance = 5;

    public override string Name()
    {
        return ("Fear");
    }

    public override string Description()
    {
        return ("Scare a target away from the caster. Scared targets move away from the caster in a straight line up to " + distance + " tiles.");
    }

    public override int MaxDistance()
    {
        return (4);
    }

    public override void ReceiveVisualFeedback(Cursor cursor)
    {
        if (cursor.standingUnit)
        {
            cursor.standingUnit.ForceWalkAwayFrom(distance, Unit().xPos, Unit().zPos);
        }

        Unit().FinishAction();
    }

    public override ActionType actionType(){
        return ActionType.Support;
    }
}
