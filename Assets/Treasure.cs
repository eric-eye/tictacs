using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour {

	public int points = 50;
	public Cursor cursor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Remove(){
		cursor.standingTreasure = null;
		GameController.treasureCount--;
		Destroy(gameObject);
	}
}
