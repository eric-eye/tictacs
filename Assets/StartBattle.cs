using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StartBattle : NetworkBehaviour
{
  // Use this for initialization
  void Start()
  {
    transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => LobbyController.instance.StartBattle());
  }

  // Update is called once per frame
  void Update()
  {
    bool readyToStart = NetworkServer.active;// && Player.players.Count == 2;
    GetComponent<Canvas>().enabled = readyToStart;
  }
}
