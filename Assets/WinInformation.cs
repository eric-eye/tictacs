using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinInformation : MonoBehaviour {

	private static Text text;
	private static Canvas display;

	void Start(){
		display = GetComponent<Canvas>();
		text = transform.Find("Panel").Find("Text").GetComponent<Text>();
	}

	// Use this for initialization
	public static void Show () {
		List<Player> sudoPlayers = Player.players;
		sudoPlayers.Sort((a, b) => b.CurrentPoints().CompareTo(a.CurrentPoints()));
		display.enabled = true;
		text.text = "Player " + (sudoPlayers[0].playerIndex + 1) + " Wins!";
	}
	
	// Update is called once per frame
	public static void Hide () {
		display.enabled = false;
	}
}
