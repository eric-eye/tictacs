using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

  public static Canvas display;
  public static Menu menu;
  public GameObject actionButtonPrefab;
  public GameObject stanceButtonPrefab;

  public void EndTurn(){
    EndTurnMenu.Show();
  }

	// Use this for initialization
	void Start () {
    display = gameObject.GetComponent<Canvas>();
    menu = this;
	}

  public void PickAction(Unit unit, int stanceIndex){
    StanceInformation.Hide();
    if(!GameController.inputsFrozen){
      GameController.PickAction(unit, stanceIndex);
      Hide();
    }
  }

  public void PickStance(Unit unit, int stanceIndex){
    if(!GameController.inputsFrozen){
      Player.player.PickStance(unit, stanceIndex);
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
    Hide();

    if(GameController.inputsFrozen){
      return;
    }

    Unit unit = Unit.Subject();

    display.transform.Find("Panel").Find("End Turn").gameObject.SetActive(!ModelController.inModelMode);

    display.enabled = ModelController.inModelMode || Player.player && Unit.current && Unit.current.playerIndex == Player.player.playerIndex && !Unit.current.dead && (!Unit.current.hasActed || !Unit.current.hasMoved);

    float yStart = 25;
    int i = 0;

    if(unit && (ModelController.inModelMode || !unit.hasActed && !unit.dead)){
      foreach(GameObject actionObject in unit.Actions()){
        int localIndex = i;
        GameObject buttonObject = Instantiate(menu.actionButtonPrefab, Vector3.zero, Quaternion.identity);
        buttonObject.transform.parent = menu.transform.Find("Panel").Find("Actions");

        //buttonObject.transform.position = Vector3.zero;
        UnitAction action = unit.Actions()[i].GetComponent<UnitAction>();
        buttonObject.transform.Find("Text").GetComponent<Text>().text = action.Name();
        buttonObject.transform.position = menu.transform.Find("Panel").transform.position;
        buttonObject.transform.localScale = new Vector3(1, 1, 1);
        buttonObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-88, yStart - (i * 50));
        buttonObject.GetComponent<Button>().onClick.AddListener(() => Menu.menu.PickAction(unit, localIndex));
        buttonObject.GetComponent<ActionButton>().actionIndex = localIndex;
        buttonObject.SetActive(unit.Actions()[i].GetComponent<UnitAction>().used || unit.playerIndex == Player.player.playerIndex);

        if(unit.currentMp < action.MpCost()){
          buttonObject.SetActive(false);
        }

        i++;
      }
    }

    int x = 0;

    if(unit && (ModelController.inModelMode || !unit.hasActed && !unit.hasMoved)){
      foreach(GameObject actionObject in unit.Stances()){
        int localIndex = x;
        GameObject buttonObject = Instantiate(menu.stanceButtonPrefab, Vector3.zero, Quaternion.identity);
        buttonObject.transform.parent = menu.transform.Find("Panel").Find("Stances");
        Stance stance = unit.Stances()[x].GetComponent<Stance>();
        string newName = stance.Name();
        Stance unitStance = ModelController.inModelMode ? unit.GetComponent<ModelBehavior>().Stance() : unit.Stance();
        if(stance == unitStance) newName = newName + " *";
        buttonObject.transform.position = menu.transform.Find("Panel").transform.position;
        buttonObject.transform.localScale = new Vector3(1, 1, 1);
        buttonObject.transform.Find("Text").GetComponent<Text>().text = newName;
        buttonObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(80, yStart - (x * 50));
        buttonObject.GetComponent<Button>().onClick.AddListener(() => Menu.menu.PickStance(unit, localIndex));
        buttonObject.GetComponent<StanceButton>().stanceIndex = localIndex;
        buttonObject.SetActive(unit.Stances()[x].GetComponent<Stance>().used || unit.playerIndex == Player.player.playerIndex);
        x++;
      }
    }
  }
}
