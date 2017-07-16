using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionConfigButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string actionName;
    public string mpCost;
    public string description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ActionInformation.Show(actionName, mpCost, description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ActionInformation.Hide();
    }
}