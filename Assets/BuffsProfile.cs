using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffsProfile : MonoBehaviour {

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

  public static void Show(Unit newUnit){
		if(newUnit.Buffs().Count == 0){
			Hide();
			return;
		}

		int i = 0;
    display.enabled = true;

    foreach(Transform buffText in display.transform.Find("Panel")){
			buffText.GetComponent<Text>().text = "";
		}

		foreach(Buff buff in newUnit.Buffs()){
			i++;

			display.transform.Find("Panel").Find("Buff" + i).GetComponent<Text>().text = buff.Name() + ": " + buff.Description() + ". Lasts " + buff.turnsLeft + " more turn(s).";
		}
  }
}
