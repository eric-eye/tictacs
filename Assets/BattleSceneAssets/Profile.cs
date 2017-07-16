using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour {

  public static Unit unit;
  public static Canvas display;

  private Text unitName;
  private Text unitHp;
  private Text unitMp;
  private Text unitStance;
  private Text unitAffinity;
  private static bool activated = false;

  // Use this for initialization
  void Start () {
    display = gameObject.GetComponent<Canvas>();
    unitName = transform.Find("Panel").Find("UnitName").GetComponent<Text>();
    unitHp = transform.Find("Panel").Find("HitPoints").GetComponent<Text>();
    unitMp = transform.Find("Panel").Find("MagicPoints").GetComponent<Text>();
    unitStance = transform.Find("Panel").Find("Stance").GetComponent<Text>();
    unitAffinity = transform.Find("Panel").Find("Affinity").GetComponent<Text>();
    Hide();
  }

  // Update is called once per frame
  void Update () {
    if(activated){
      unitName.text = unit.unitName;
      unitHp.text = "HP: " + unit.currentHp.ToString() + "/" + unit.maxHp.ToString();
      unitMp.text = "MP: " + unit.currentMp.ToString() + "/" + unit.maxMp.ToString();
      unitAffinity.text = "Affinity: " + unit.affinity.ToString();
      if(unit.Stance() != null){
        unitStance.text = "Stance: " + StanceName();
      }else{
        unitStance.text = "Stance: None";
      }
    }
  }

  private string StanceName(){
    string stanceName = "";
    if(unit.stanceRevealed || unit.playerIndex == Player.player.playerIndex){
      stanceName = unit.Stance().Name().ToString();
    }else{
      stanceName = "?????";
    }
    return(stanceName);
  }

  public static void SetUnit(Unit newUnit){
    unit = newUnit;
  }

  public static void Hide(){
    display.enabled = false;
    activated = false;
  }

  public static void Show(Unit newUnit){
    unit = newUnit;
    display.enabled = true;
    activated = true;
  }
}
