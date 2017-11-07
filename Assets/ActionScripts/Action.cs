using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

abstract public class Action : NetworkBehaviour {
  public enum CursorModes { Radial, Line };
  public GameObject visualPrefab;

  [SyncVar]
  public bool used = false;

  [SyncVar]
  public NetworkInstanceId parentNetId;

  void Start(){
    if (parentNetId != null) { 
      GameObject parentObject = ClientScene.FindLocalObject(parentNetId);
      transform.SetParent(parentObject.transform.Find("Actions"));
    }
  }

  public Unit Unit(){
    return(transform.parent.transform.parent.GetComponent<Unit>());
  }

  public void BeginAction(GameObject targetObject){
    if(NetworkServer.active) used = true;

    DoAction(targetObject.GetComponent<Cursor>());
  }

  public virtual int TpCost(){
    return(25);
  }

  public virtual int MpCost(){
    return(0);
  }

  public virtual int MaxDistance(){
    return(1);
  }

  public virtual int RadialDistance(){
    return(0);
  }

  public virtual bool CanTargetSelf(){
    return(false);
  }

  public virtual bool NeedsLineOfSight(){
    return(false);
  }

  public virtual CursorModes CursorMode(){
    return(CursorModes.Radial);
  }

  abstract public string Name();
  abstract public string Description();
  abstract public void ReceiveVisualFeedback(Cursor cursor);

  protected virtual void DoAction(Cursor cursor){
    CreateVisual(cursor, cursor.transform.position);
  }

  protected void CreateVisual(Cursor target, Vector3 visualPosition){
    GameObject visualObject = Instantiate(visualPrefab, visualPosition, Quaternion.identity);
    Visual visual = visualObject.transform.Find("Main").GetComponent<Visual>();
    visual.action = this.GetComponent<Action>();
    visual.cursor = target;
  }
}
