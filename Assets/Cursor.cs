using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridFramework.Grids;
using GridFramework.Extensions.Align;

public class Cursor : MonoBehaviour {

  public static Cursor hovered;

  public int xPos;
  public int zPos;
  public int yPos;
  public Unit standingUnit;
  public bool attack = false;
  public Color originalColor;
  public bool movable = false;

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

  public void NegotiateColor(){
    if(hovered == this){
      SetColor(Color.red);
    }else if(attack){
      SetColor(Color.green);
    }else if(path){
      SetColor(Color.blue);
    }else if(movable){
      SetColor(Color.yellow);
    }else {
      SetColor(originalColor);
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

  public void UnsetAttack(){
    attack = false;
    NegotiateColor();
  }

  void SetColor(Color color) {
    gameObject.GetComponent<Renderer>().material.color = color;
  }
}
