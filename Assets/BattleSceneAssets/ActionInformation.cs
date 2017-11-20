using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionInformation : MonoBehaviour {

  public static Canvas display;

  private static Text actionName;
  private static Text tpCost;
  private static Text mpCost;
  private static Text description;

	// Use this for initialization
	void Start () {
    display = gameObject.GetComponent<Canvas>();
    actionName = transform.Find("Panel").Find("Name").GetComponent<Text>();
    tpCost = transform.Find("Panel").Find("TpCost").GetComponent<Text>();
    mpCost = transform.Find("Panel").Find("MpCost").GetComponent<Text>();
    description = transform.Find("Panel").Find("Description").GetComponent<Text>();
    Hide();
	}
	
  public static void Show(string name, string tp, string mp, string actionDescription){
    display.enabled = true;

    actionName.text = name;
    tpCost.text = "TP: " + tp;
    mpCost.text = "MP: " + mp;
    description.text = actionDescription;
  }

  public static void Hide(){
    display.enabled = false;
  }
}
