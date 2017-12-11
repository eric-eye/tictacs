using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUIBehavior : MonoBehaviour {

  private bool dragging = false;
  private float minDragTimer = 0.25f;
  private float dragTimer = 0;

  public GameObject model;
  private Unit unit;

	// Use this for initialization
	void Start () {
    unit = GetComponent<Unit>();
		model = GameObject.Instantiate(transform.Find("Body").Find("CharacterModel").gameObject, transform.position, Quaternion.identity);
    model.transform.parent = transform.Find("Model");
    model.transform.localScale = transform.Find("Body").Find("CharacterModel").localScale;
    model.transform.parent.gameObject.SetActive(false);
    Helpers.GetTile(unit.xPos, unit.zPos).standingModel = model;
	}

  private void DisplayModel(GameObject model){
    model.transform.parent.gameObject.SetActive(true);
    model.transform.Find("ArmLeft1").GetComponent<Renderer>().material = unit.transparentMaterial;
    model.transform.Find("ArmLeft1").GetComponent<Renderer>().material.color = new Color(unit._color.r, unit._color.g, unit._color.b, 0.1f);
    model.transform.Find("ArmRight1").GetComponent<Renderer>().material = unit.transparentMaterial;
    model.transform.Find("ArmRight1").GetComponent<Renderer>().material.color = new Color(unit._color.r, unit._color.g, unit._color.b, 0.1f);
    model.transform.Find("Body1").GetComponent<Renderer>().material = unit.transparentMaterial;
    model.transform.Find("Body1").GetComponent<Renderer>().material.color = new Color(unit._color.r, unit._color.g, unit._color.b, 0.1f);
    model.transform.Find("LegLeft1").GetComponent<Renderer>().material = unit.transparentMaterial;
    model.transform.Find("LegLeft1").GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
    model.transform.Find("LegRight1").GetComponent<Renderer>().material = unit.transparentMaterial;
    model.transform.Find("LegRight1").GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
    model.transform.Find("Head1").GetComponent<Renderer>().material = unit.transparentMaterial;
    model.transform.Find("Head1").GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
  }
	
	// Update is called once per frame
	void Update () {
    bool clicked = Input.GetMouseButton(0);
    if(clicked){
      dragTimer += Time.deltaTime;
      dragging = dragging || dragTimer >= minDragTimer && Cursor.hovered && Cursor.hovered.standingModel == model;
      print("dragging? " + dragging);
      if(dragging) {
        DisplayModel(model);
      };
    }else{
      dragTimer = 0;
      dragging = false;
      if(dragging){
        Cursor.hovered.standingModel = model;
      }
    }

    if(dragging){
      if(Cursor.hovered){
        print("moving model for " + unit.unitName);
        Vector3 target = new Vector3(Cursor.hovered.xPos + 0.5f, Cursor.hovered.yPos + 1.5f, Cursor.hovered.zPos + 0.5f);
        Vector3 position = Vector3.Lerp(model.transform.parent.position, target, 0.75f);
        model.transform.parent.position = position;
      }
    }
	}

  public void Focus() {
    Profile.Show(GetComponent<Unit>());
    AdvancedProfile.Show(GetComponent<Unit>());
    TurnOrder.Show();
    Unit.hovered = GetComponent<Unit>();
    SetHighlight();
  }

  public void Blur() {
    Profile.Hide();
    AdvancedProfile.Hide();
    TurnOrder.Hide();
    Unit.hovered = null;
    UnsetHighlight();
  }

  void SetHighlight(){
    HighlightBodyPart("ArmLeft1");
    HighlightBodyPart("ArmRight1");
    HighlightBodyPart("Body1");
    HighlightBodyPart("Head1");
    HighlightBodyPart("LegLeft1");
    HighlightBodyPart("LegRight1");
  }

  void UnsetHighlight(){
    UnsetHighlightBodyPart("ArmLeft1");
    UnsetHighlightBodyPart("ArmRight1");
    UnsetHighlightBodyPart("Body1");
    UnsetHighlightBodyPart("Head1");
    UnsetHighlightBodyPart("LegLeft1");
    UnsetHighlightBodyPart("LegRight1");
  }

  public void SetMarker(){
    transform.Find("Marker").GetComponent<MeshRenderer>().enabled = true;
  }

  public void UnsetMarker(){
    transform.Find("Marker").GetComponent<MeshRenderer>().enabled = false;
  }

  private void HighlightBodyPart(string bodyPart){
    gameObject.transform.Find("Body").Find("CharacterModel").Find(bodyPart).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red * 100);
    gameObject.transform.Find("Body").Find("CharacterModel").Find(bodyPart).GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
  }

  private void UnsetHighlightBodyPart(string bodyPart){
    gameObject.transform.Find("Body").Find("CharacterModel").Find(bodyPart).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
    gameObject.transform.Find("Body").Find("CharacterModel").Find(bodyPart).GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
  }
}
