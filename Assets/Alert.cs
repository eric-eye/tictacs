using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour {

	private static Text text;
	private static Canvas display;
	private static float timer = 0;

	// Use this for initialization
	void Start () {
		display = GetComponent<Canvas>();
		text = transform.Find("Panel").Find("Text").GetComponent<Text>();
		display.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer > 3) {
			display.enabled = false;
		}
	}

	public static void Show(string inputText){
		text.text = inputText;
		display.enabled = true;
		timer = 0;
	}
}
