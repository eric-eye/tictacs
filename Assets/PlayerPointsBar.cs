using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerPointsBar : MonoBehaviour {

	public int playerIndex;
	public static List<PlayerPointsBar> pointsBars = new List<PlayerPointsBar>();


	// Use this for initialization
	void Start () {
		pointsBars.Add(this);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public static void ResizeByIndex(int playerIndex){
		PlayerPointsBar pointsBar = pointsBars.Find(p => p.playerIndex == playerIndex);
		if(pointsBar){
			pointsBar.Resize();
		}
	}

  void Resize(){
    int points = Unit.All().Where(unit => unit.playerIndex == playerIndex).Aggregate(0, (acc, x) => acc + x.points);
    float ratio = (float)points / (float)GameController.pointsToWin;
    RectTransform bar = transform.Find("Fill").GetComponent<RectTransform>();
    bar.localScale = new Vector2(ratio, 1);
    transform.Find("PointsAmount").GetComponent<Text>().text = points.ToString();
	}
}
