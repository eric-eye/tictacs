using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathInformation : MonoBehaviour
{
    public static Canvas display;

    // Use this for initialization
    void Start()
    {
        display = gameObject.GetComponent<Canvas>();
        display.enabled = false;
    }

    public static void Refresh()
    {
        Unit unit = Unit.current;

        if (unit)
        {
            display.enabled = unit.dead;

            int turnsLeft = Unit.current.MaxTurnsDead() - Unit.current.turnsDead + 1;

            string fragment = "in " + turnsLeft + " turns";

            if (turnsLeft <= 1)
            {
                fragment = "next turn";
            }

            string message = unit.unitName + " is dead, but will be back " + fragment + ".";

            display.transform.Find("Panel").Find("Text").GetComponent<Text>().text = message;
        }
    }
}