using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StartBattle : NetworkBehaviour
{
  private int playersNeeded = 2;
  
  // Use this for initialization
  void Start()
  {
    transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => LobbyController.instance.StartBattle());
  }

  // Update is called once per frame
  void Update()
  {
    bool readyToStart = NetworkServer.active && Player.players.Where((x) => x.TotallyReadyForBattle()).Count() >= playersNeeded;
    GetComponent<Canvas>().enabled = readyToStart;
  }
  
}
