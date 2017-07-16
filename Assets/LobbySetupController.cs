using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;


public class LobbySetupController : MonoBehaviour
{
    
    public GameObject lobbyControllerPrefab;
    private bool spawned = false;
//    private static LobbySetupController instance;

    // Use this for initialization
    void Start()
    {
//        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.player == null || !Player.player.initialized)
        {
            return;
        }

        if (!spawned)
        {
            GameObject.Find("LobbyStatus").transform.Find("ConfigScreen").gameObject.active = true;
            SpinUpUnitConfigs();
            spawned = true;
            if (!NetworkServer.active)
            {
                return;
            }
            GameObject lobby = Instantiate(lobbyControllerPrefab, Vector3.zero, Quaternion.identity);
            lobby.GetComponent<LobbyController>().seed = (int)System.DateTime.Now.Ticks;
            NetworkServer.Spawn(lobby);
        }
    }

    void SpinUpUnitConfigs()
    {
        List<UnitConfig.UnitInitParams> unitParamList = new List<UnitConfig.UnitInitParams>();
        
        print(Player.player.playerIndex);

        if (Player.player.playerIndex == 0)
        {
            unitParamList.Add(new UnitConfig.UnitInitParams("Ash", new Warrior(), Color.red));
            unitParamList.Add(new UnitConfig.UnitInitParams("Red", new Archer(), Color.red));
            unitParamList.Add(new UnitConfig.UnitInitParams("Charizard", new Mage(), Color.red));
            unitParamList.Add(new UnitConfig.UnitInitParams("Ness", new Medic(), Color.red));
        }
        else
        {
            unitParamList.Add(new UnitConfig.UnitInitParams("Blue", new Warrior(), Color.blue));
            unitParamList.Add(new UnitConfig.UnitInitParams("Gary", new Archer(), Color.blue));
            unitParamList.Add(new UnitConfig.UnitInitParams("Squirtle", new Mage(), Color.blue));
            unitParamList.Add(new UnitConfig.UnitInitParams("Porky", new Medic(), Color.blue));
        }
        

        int i = 0;

        foreach (Transform config in GameObject.Find("LobbyStatus").transform.Find("ConfigScreen")
            .Find("UnitConfigs"))
        {
            config.GetComponent<UnitConfig>().SetParams(unitParamList[i]);
            i++;
        }
    }

    public static void ShowUnitConfig()
    {
        GameObject.Find("LobbyStatus").transform.Find("ConfigScreen").Find("UnitConfigs").gameObject.active = true;
        foreach (Transform unitConfig in GameObject.Find("LobbyStatus").transform.Find("ConfigScreen")
            .Find("UnitConfigs"))
        {
            unitConfig.GetComponent<UnitConfig>().Refresh();
        }
    }
    
    public static void HideUnitConfig()
    {
        GameObject.Find("LobbyStatus").transform.Find("ConfigScreen").Find("UnitConfigs").gameObject.active = false;
    }
    
    public static void ShowActionConfig(UnitConfig unitConfig, int index, ActionConfig.ConfigTypes configType)
    {
        GameObject.Find("LobbyStatus").transform.Find("ConfigScreen").Find("ActionScreen").gameObject.active = true;
        GameObject.Find("LobbyStatus").transform.Find("ConfigScreen").Find("ActionScreen").GetComponent<ActionConfig>()
            .SetUnit(unitConfig, index, configType);
    }
    
    public static void HideActionConfig()
    {
        GameObject.Find("LobbyStatus").transform.Find("ConfigScreen").Find("ActionScreen").gameObject.active = false;
    }
}