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

    units.Sort((a, b) => a.GetComponent<Unit>().turnIndex.CompareTo(b.GetComponent<Unit>().turnIndex));

    foreach(Transform actionTransform in display.transform.Find("Panel").Find("Players")){
      if(i < units.Count){
        actionTransform.GetComponent<Text>().text = (i + 1) + ". " + units[i].unitName;
        i++;
      }else{
        actionTransform.GetComponent<Text>().text = "";
      }
    }
  }
}
