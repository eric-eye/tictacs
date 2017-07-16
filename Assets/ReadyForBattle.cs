using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ReadyForBattle : MonoBehaviour
{
  
  // Use this for initialization
  void Start()
  {
    transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => LobbyController.instance.ReadyForBattle());
  }

  // Update is called once per frame
  void Update()
  {
    if (Player.player)
    {
        GetComponent<Canvas>().enabled = !Player.player.readyToBattle && UnitConfig.readyForBattle;
    }
  }
}

