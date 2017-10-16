using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUIBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void OnMouseEnter() {
    Profile.Show(GetComponent<Unit>());
    AdvancedProfile.Show(GetComponent<Unit>());
    Unit.hovered = GetComponent<Unit>();
    SetHighlight();
  }

  void OnMouseExit() {
    Profile.Hide();
    AdvancedProfile.Hide();
    Unit.hovered = null;
    UnsetHighlight();
  }

  void SetHighlight(){
    gameObject.transform.Find("Body").GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red * 100);
    gameObject.transform.Find("Body").GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
  }

  void UnsetHighlight(){
    gameObject.transform.Find("Body").GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
    gameObject.transform.Find("Body").GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
  }

  public void SetMarker(){
    transform.Find("Marker").GetComponent<MeshRenderer>().enabled = true;
  }

  public void UnsetMarker(){
    transform.Find("Marker").GetComponent<MeshRenderer>().enabled = false;
  }
}
