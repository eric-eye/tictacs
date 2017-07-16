using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnMenu : MonoBehaviour {

	private static GameObject instance;

	// Use this for initialization
	void Start () {
		instance = gameObject;
		Transform panel = instance.transform.Find("Panel");
		panel.transform.Find("Yes").GetComponent<Button>().onClick.AddListener(
			() => {
				Hide();
				GameController.EndTurn();
			}
		);
		panel.transform.Find("No").GetComponent<Button>().onClick.AddListener(
			() => Cancel()
		);
		Hide();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void Show(){
		instance.GetComponent<Canvas>().enabled = true;
		instance.transform.Find("Panel").Find("Text").GetComponent<Text>().text = "Really end your turn?";
		Menu.Hide();
	}

	public static void Hide(){
		instance.GetComponent<Canvas>().enabled = false;
	}

	public static void Cancel(){
		Hide();
		Menu.Refresh();
	}
}
