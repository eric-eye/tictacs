using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridFramework.Grids;
using GridFramework.Extensions.Align;
using cakeslice;

public class Cursor : MonoBehaviour {

  public static Cursor hovered;

  public int xPos;
  public int zPos;
  public int yPos;
  public Unit standingUnit;
  public bool attack = false;
  public bool attackConfirm = false;
  public bool attackInRange = false;
  public Color originalColor;
  public bool movable = false;
  public bool debug = false;

  private RectGrid _grid;
  private Color imprintColor;
  private bool path = false;

	// Use this for initialization
	void Start () {
    _grid = GameObject.Find("Grid").GetComponent<RectGrid>();

    Vector3 position = transform.position;

    position.x = xPos + 0.5f;
    position.z = zPos + 0.5f;
    position.y = yPos + 1.1f;

    transform.position = position;

    NegotiateColor();

    //_grid.AlignTransform(transform);
	}
	
  void OnMouseEnter() {
    hovered = this;
    NegotiateColor();
  }

  void OnMouseExit() {
    hovered = null;
    NegotiateColor();
  }

  void RefreshBorder(bool show){
    GetComponent<Outline>().enabled = show;
  }

  public void NegotiateColor(){
    RefreshBorder(hovered == this);

    if(debug){
      SetColor(new Color(1, 1, 1, 1));
    }else if(attackConfirm){
      SetColor(new Color(1, 0, 1, 0.75f));
    }else if(attackInRange){
      SetColor(new Color(1, 0, 1, 0.5f));
    }else if(attack){
      SetColor(new Color(0, 1, 0, 0.75f));
    }else if(path){
      SetColor(new Color(0, 0, 1, 0.75f));
    }else if(movable){
      SetColor(new Color(1, 1, 0, 0.5f));
    }else {
      SetColor(new Color(0, 0, 0, 0));
    }
  }

  public void SetMovement(){
    movable = true;
    NegotiateColor();
  }

  public void UnsetMovement(){
    movable = false;
    NegotiateColor();
  }

  public void SetPath(){
    path = true;
    NegotiateColor();
  }

  public void UnsetPath(){
    path = false;
    NegotiateColor();
  }

  public void SetAttack(){
    attack = true;
    NegotiateColor();
  }

  public void SetAttackConfirm(){
    attackConfirm = true;
    NegotiateColor();
  }

  public void SetAttackInRange(){
    attackInRange = true;
    NegotiateColor();
  }

  public void UnsetAttack(){
    attack = false;
    NegotiateColor();
  }

  public void UnsetAttackConfirm(){
    attackConfirm = false;
    NegotiateColor();
  }

  public void UnsetAttackInRange(){
    attackInRange = false;
    NegotiateColor();
  }

  void SetColor(Color color) {
    gameObject.GetComponent<Renderer>().material.color = color;
  }
}
