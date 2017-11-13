using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathInformation : MonoBehaviour {

  public static Canvas display;

	// Use this for initialization
	void Start () {
    display = gameObject.GetComponent<Canvas>();
		display.enabled = false;
	}

  public static void Refresh(){
		print("refreshing...");
		Unit unit = Unit.current;

		if(unit){
				display.enabled = unit.dead;

				string message = unit.unitName + " is dead, but will be back soon!";

				display.transform.Find("Panel").Find("Text").GetComponent<Text>().text = message;
		}
  }
}
