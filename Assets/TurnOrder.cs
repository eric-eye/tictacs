using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrder : MonoBehaviour {

  public static Canvas display;

	// Use this for initialization
	void Start () {
    display = gameObject.GetComponent<Canvas>();

    Hide();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public static void Hide(){
    display.enabled = false;
  }

  public static void Show(){
    display.enabled = true;

    int i = 0;

    List<Unit> units = Unit.All();

    units.Sort((a, b) => a.GetComponent<Unit>().TpDiff().CompareTo(b.GetComponent<Unit>().TpDiff()));

    foreach(Transform actionTransform in display.transform.Find("Panel")){
      if(i < Unit.All().Count){
        actionTransform.GetComponent<Text>().text = i + 1 + ". " + Unit.All()[i].unitName + " - " + Unit.All()[i].CurrentTp();
        i++;
      }else{
        actionTransform.GetComponent<Text>().text = "";
      }
    }
  }
}
