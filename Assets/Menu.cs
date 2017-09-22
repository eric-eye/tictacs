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

  public void PickAction(GameObject action){
    if(!GameController.inputsFrozen){
      GameController.PickAction(action);
      Hide();
    }
  }

  public void PickStance(int stanceIndex){
    if(!GameController.inputsFrozen){
      GameController.PickStance(stanceIndex);
      Hide();
      Show();
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

  public static void Show(){
    display.enabled = true;

    float yStart = 35;
    int i = 0;

    if(!Unit.current.hasActed){
      foreach(GameObject actionObject in Unit.current.actions){
        GameObject buttonObject = Instantiate(menu.actionButtonPrefab, Vector3.zero, Quaternion.identity);
        buttonObject.transform.parent = menu.transform.Find("Panel").Find("Actions");
        IAction action = Unit.current.actions[i].GetComponent<IAction>();
        buttonObject.transform.Find("Text").GetComponent<Text>().text = action.Name();
        buttonObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-88, yStart - (i * 30));
        buttonObject.GetComponent<Button>().onClick.AddListener(() => Menu.menu.PickAction(actionObject));

        if(Unit.current.currentMp < action.MpCost()){
          buttonObject.SetActive(false);
        }

        i++;
      }
    }

    int x = 0;

    if(!Unit.current.hasActed && !Unit.current.hasMoved){
      foreach(GameObject actionObject in Unit.current.stances){
        int localIndex = x;
        GameObject buttonObject = Instantiate(menu.stanceButtonPrefab, Vector3.zero, Quaternion.identity);
        buttonObject.transform.parent = menu.transform.Find("Panel").Find("Stances");
        IStance stance = Unit.current.stances[x].GetComponent<IStance>();
        string newName = stance.Name();
        if(stance == Unit.current.Stance()) newName = newName + " *";
        buttonObject.transform.Find("Text").GetComponent<Text>().text = newName;
        buttonObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(80, yStart - (x * 30));
        buttonObject.GetComponent<Button>().onClick.AddListener(() => Menu.menu.PickStance(localIndex));

        x++;
      }
    }
  }
}
