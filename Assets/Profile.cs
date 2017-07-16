using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour {

  public static Unit unit;
  public static Canvas display;

  private Text unitName;
  private Text unitHits;
  private static bool activated = false;

  // Use this for initialization
  void Start () {
    display = gameObject.GetComponent<Canvas>();
    unitName = transform.Find("UnitName").GetComponent<Text>();
    unitHits = transform.Find("HitPoints").GetComponent<Text>();
    Hide();
  }

  // Update is called once per frame
  void Update () {
    if(activated){
      unitName.text = unit.name;
      unitHits.text = unit.currentHp.ToString() + "/" + unit.maxHp.ToString();
    }
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
