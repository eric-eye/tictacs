using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelModeButton : MonoBehaviour {

	private static GameObject instance;

	void Start(){
		ToggleText(false);
	}

	void Awake () {
		instance = this.gameObject;
	}

	public static void ToggleText(bool inModelMode){
    string text = inModelMode ? "Exit" : "Enter";

    SetText(text + " Model Mode");

    if (inModelMode)
    {
      instance.transform.Find("Text").GetComponent<Text>().color = Color.yellow;
    }else{
      instance.transform.Find("Text").GetComponent<Text>().color = Color.white;
		}
	}

  private static void SetText(string inputText){
		instance.transform.Find("Text").GetComponent<Text>().text = inputText;
	}

	
}
