using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Action : NetworkBehaviour {
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

    Cursor cursor = targetObject.GetComponent<Cursor>();
    GameObject visualObject = Instantiate(visualPrefab, cursor.transform.position, Quaternion.identity);
    Visual visual = visualObject.transform.Find("Main").GetComponent<Visual>();
    visual.action = this.GetComponent<IAction>();
    visual.cursor = cursor;
  }
}
