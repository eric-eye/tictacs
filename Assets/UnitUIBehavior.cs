using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUIBehavior : MonoBehaviour {

  public void Focus() {
    Profile.Show(GetComponent<Unit>());
    AdvancedProfile.Show(GetComponent<Unit>());
    BuffsProfile.Show(GetComponent<Unit>());
    TurnOrder.Show();
    Unit.hovered = GetComponent<Unit>();
    SetHighlight();
  }

  public void Blur() {
    Profile.Hide();
    AdvancedProfile.Hide();
    BuffsProfile.Hide();
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
    HighlightHat();
  }

  void UnsetHighlight(){
    UnsetHighlightBodyPart("ArmLeft1");
    UnsetHighlightBodyPart("ArmRight1");
    UnsetHighlightBodyPart("Body1");
    UnsetHighlightBodyPart("Head1");
    UnsetHighlightBodyPart("LegLeft1");
    UnsetHighlightBodyPart("LegRight1");
    UnhighlightHat();
  }

  public void SetMarker(){
    transform.Find("Marker").GetComponent<MeshRenderer>().enabled = true;
  }

  public void UnsetMarker(){
    transform.Find("Marker").GetComponent<MeshRenderer>().enabled = false;
  }
  
  private void HighlightHat(){
    foreach (Transform hat in gameObject.transform.Find("Body").Find("Hats"))
    {
        hat.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red * 100);
        hat.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }
  }
  
  private void UnhighlightHat(){
    foreach (Transform hat in gameObject.transform.Find("Body").Find("Hats"))
    {
        hat.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
        hat.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }
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
