using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedProfile : MonoBehaviour {

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
    display.enabled = true;

    int i = 0;

    foreach(Transform actionTransform in display.transform.Find("Panel").Find("Actions")){
      if(i < newUnit.Actions().Count){
        string actionName = "????";
        if(newUnit.Actions()[i].GetComponent<Action>().used || newUnit.playerIndex == Player.player.playerIndex){
          actionName = newUnit.Actions()[i].GetComponent<IAction>().Name();
        }
        actionTransform.GetComponent<Text>().text = actionName;
      }else{
        actionTransform.GetComponent<Text>().text = "";
      }
      i++;
    }

    i = 0;

    foreach(Transform stanceTransform in display.transform.Find("Panel").Find("Stances")){
      Unit unit = newUnit.GetComponent<Unit>();

      if(i < unit.Stances().Count){
        string stanceName = "????";
        print(newUnit.Stances()[i].GetComponent<Stance>());
        print(newUnit.Stances()[i].GetComponent<Stance>().used);
        if(newUnit.Stances()[i].GetComponent<Stance>().used || newUnit.playerIndex == Player.player.playerIndex){
          stanceName = newUnit.Stances()[i].GetComponent<IStance>().Name();
        }
        stanceTransform.GetComponent<Text>().text = stanceName;
      }else{
        stanceTransform.GetComponent<Text>().text = "";
      }
      i++;
    }
  }
}
