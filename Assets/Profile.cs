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
  private Text unitTp;
  private Text unitStance;
  private Text unitBuffs;
  private static bool activated = false;

  // Use this for initialization
  void Start () {
    display = gameObject.GetComponent<Canvas>();
    unitName = transform.Find("Panel").Find("UnitName").GetComponent<Text>();
    unitHp = transform.Find("Panel").Find("HitPoints").GetComponent<Text>();
    unitMp = transform.Find("Panel").Find("MagicPoints").GetComponent<Text>();
    unitTp = transform.Find("Panel").Find("TurnPoints").GetComponent<Text>();
    unitStance = transform.Find("Panel").Find("Stance").GetComponent<Text>();
    unitBuffs = transform.Find("Panel").Find("Buffs").GetComponent<Text>();
    Hide();
  }

  // Update is called once per frame
  void Update () {
    if(activated){
      unitName.text = "Name: " + unit.name;
      unitHp.text = "HP: " + unit.currentHp.ToString() + "/" + unit.maxHp.ToString();
      unitMp.text = "MP: " + unit.currentMp.ToString() + "/" + unit.maxMp.ToString();
      unitTp.text = "TP: " + unit.CurrentTp().ToString() + "/" + unit.maxTp.ToString();
      if(unit.Stance() != null){
        unitStance.text = "Stance: " + StanceName();
      }else{
        unitStance.text = "Stance: None";
      }
      unitBuffs.text = "Buffs: " + String.Join(", ", unit.Buffs().Select(a => a.name).ToArray());
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
