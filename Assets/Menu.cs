using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

  public static Canvas display;
  public static Menu menu;
  public GameObject actionButtonPrefab;
  public GameObject stanceButtonPrefab;

	// Use this for initialization
	void Start () {
    display = gameObject.GetComponent<Canvas>();
    menu = this;
	}

  public void PickAction(int stanceIndex){
    if(!GameController.inputsFrozen){
      GameController.PickAction(stanceIndex);
      Hide();
    }
  }

  public void PickStance(int stanceIndex){
    if(!GameController.inputsFrozen){
      Player.player.PickStance(stanceIndex);
    }
  }

  public static void Hide(){
    display.enabled = false;

    foreach(Transform button in menu.transform.Find("Panel").Find("Actions")){
      Destroy(button.gameObject);
    }

    foreach(Transform button in menu.transform.Find("Panel").Find("Stances")){
      Destroy(button.gameObject);
    }
  }

  public static void Refresh(){
    if(GameController.inputsFrozen){
      Hide();
      return;
    }

    display.enabled = true;

    float yStart = 35;
    int i = 0;

    if(Unit.current && !Unit.current.hasActed){
      foreach(GameObject actionObject in Unit.current.Actions()){
        int localIndex = i;
        GameObject buttonObject = Instantiate(menu.actionButtonPrefab, Vector3.zero, Quaternion.identity);
        buttonObject.transform.parent = menu.transform.Find("Panel").Find("Actions");

        //buttonObject.transform.position = Vector3.zero;
        IAction action = Unit.current.Actions()[i].GetComponent<IAction>();
        buttonObject.transform.Find("Text").GetComponent<Text>().text = action.Name();
        buttonObject.transform.position = menu.transform.Find("Panel").transform.position;
        buttonObject.transform.localScale = new Vector3(1, 1, 1);
        buttonObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-88, yStart - (i * 30));
        buttonObject.GetComponent<Button>().onClick.AddListener(() => Menu.menu.PickAction(localIndex));

        if(Unit.current.currentMp < action.MpCost()){
          buttonObject.SetActive(false);
        }

        i++;
      }
    }

    int x = 0;

    if(Unit.current && !Unit.current.hasActed && !Unit.current.hasMoved){
      foreach(GameObject actionObject in Unit.current.Stances()){
        int localIndex = x;
        GameObject buttonObject = Instantiate(menu.stanceButtonPrefab, Vector3.zero, Quaternion.identity);
        buttonObject.transform.parent = menu.transform.Find("Panel").Find("Stances");
        IStance stance = Unit.current.Stances()[x].GetComponent<IStance>();
        string newName = stance.Name();
        if(stance == Unit.current.Stance()) newName = newName + " *";
        buttonObject.transform.position = menu.transform.Find("Panel").transform.position;
        buttonObject.transform.localScale = new Vector3(1, 1, 1);
        buttonObject.transform.Find("Text").GetComponent<Text>().text = newName;
        buttonObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(80, yStart - (x * 30));
        buttonObject.GetComponent<Button>().onClick.AddListener(() => Menu.menu.PickStance(localIndex));

        x++;
      }
    }
  }
}
