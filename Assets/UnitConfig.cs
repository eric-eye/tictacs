using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitConfig : MonoBehaviour
{
    public List<string> actions = new List<string>();
    public List<string> stances = new List<string>();
    public string unitName;
    public UnitClass unitClass;
    public Color color;
    public Dictionary<string, UnitAction> actionMap = new Dictionary<string, UnitAction>();
    public Dictionary<string, Stance> stanceMap = new Dictionary<string, Stance>();
    public static List<UnitConfig> unitConfigs = new List<UnitConfig>();
    public bool finished = false;
    public static bool readyForBattle = false;
    public string actionDefault = "Unselected";
    private bool addedToPlayer = false;
    public static UnitConfig instance;

    public struct UnitInitParams
    {
        public string unitName;
        public UnitClass unitClass;
        public Color color;

        public UnitInitParams(string _unitName, UnitClass _unitClass, Color _color)
        {
            unitName = _unitName;
            unitClass = _unitClass;
            color = _color;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        instance = this;
        int i = 0;
        foreach (Transform actionBox in transform.Find("ConfigBox").Find("Actions"))
        {
            actions.Add(unitClass.Actions()[i]);
            i++;
        }

        i = 0;
        foreach (Transform actionBox in transform.Find("ConfigBox").Find("Stances"))
        {
            stances.Add(unitClass.Stances()[i]);
            i++;
        }

        unitConfigs.Add(this);

        Refresh();
    }

    private void UpdateReadyStatus()
    {
        if (!finished)
        {
            finished = actions.Concat(stances).Where((x) => x == actionDefault).Count() == 0;
        }

        if (!readyForBattle)
        {
            readyForBattle = unitConfigs.Where((x) => !x.finished).Count() == 0;
        }
    }

    public void Refresh()
    {
        UpdateReadyStatus();

        int i = 0;

        foreach (Transform actionBox in transform.Find("ConfigBox").Find("Actions"))
        {
            int c = i;

            actionBox.GetComponent<Button>().onClick.RemoveAllListeners();
            actionBox.GetComponent<Button>().onClick.AddListener(
                () => this.ShowActionScreen(c, this, ActionConfig.ConfigTypes.Ability));

            string label = actions[c];

            if (actionMap.ContainsKey(actions[c]))
            {
                label = actionMap[actions[c]].Name();
            }

            actionBox.Find("Text").GetComponent<Text>().text = label;

            i++;
        }

        i = 0;

        foreach (Transform actionBox in transform.Find("ConfigBox").Find("Stances"))
        {
            int c = i;

            actionBox.GetComponent<Button>().onClick.RemoveAllListeners();
            actionBox.GetComponent<Button>().onClick.AddListener(
                () => this.ShowActionScreen(c, this, ActionConfig.ConfigTypes.Stance));

            string label = stances[c];

            if (stanceMap.ContainsKey(stances[c]))
            {
                label = stanceMap[stances[c]].Name();
            }

            actionBox.Find("Text").GetComponent<Text>().text = label;

            i++;
        }

        transform.Find("ConfigBox").Find("InfoPanel").Find("Checkmark").gameObject.active = finished;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShowActionScreen(int i, UnitConfig unitConfig, ActionConfig.ConfigTypes configType)
    {
        LobbySetupController.ShowActionConfig(unitConfig, i, configType);
        LobbySetupController.HideUnitConfig();
    }

    public void SetParams(UnitInitParams unitInit)
    {
        unitName = unitInit.unitName;
        unitClass = unitInit.unitClass;
        color = unitInit.color;

        transform.Find("ConfigBox").Find("InfoPanel").Find("UnitName").GetComponent<Text>().text = unitName;
        transform.Find("ConfigBox").Find("InfoPanel").Find("UnitClass").GetComponent<Text>().text = unitClass.Name();

        foreach (string actionName in unitClass.Actions())
        {
            GameObject action = Instantiate(
                Resources.Load("Actions/" + actionName), Vector3.zero, Quaternion.identity) as GameObject;
            action.transform.parent = GameObject.Find("Actions").transform;
            actionMap.Add(actionName, action.GetComponent<UnitAction>());
        }

        foreach (string stanceName in unitClass.Stances())
        {
            GameObject action = Instantiate(
                Resources.Load("Stances/" + stanceName), Vector3.zero, Quaternion.identity) as GameObject;
            action.transform.parent = GameObject.Find("Actions").transform;
            stanceMap.Add(stanceName, action.GetComponent<Stance>());
        }

        Transform model = transform.Find("ConfigBox").Find("InfoPanel").Find("UnitModel").Find("Body").Find("CharacterModel");
        
        model.Find("ArmLeft1").GetComponent<Renderer>().material.color = color;
        model.Find("ArmRight1").GetComponent<Renderer>().material.color = color;
        model.Find("Body1").GetComponent<Renderer>().material.color = color;
        
        transform.Find("ConfigBox").Find("InfoPanel").Find("UnitModel").Find("Body").Find("Hats").Find(unitClass.Name()).gameObject.active = true;
    }
    
    public static List<string> AllActions()
    {
        List<string> toReturn = new List<string>();

        foreach (UnitConfig unitConfig in unitConfigs)
        {
            toReturn = toReturn.Concat(unitConfig.actions).ToList();
        }

        return(toReturn);
    }
    
    public static List<string> AllStances()
    {
        List<string> toReturn = new List<string>();

        foreach (UnitConfig unitConfig in unitConfigs)
        {
            toReturn = toReturn.Concat(unitConfig.stances).ToList();
        }

        return(toReturn);
    }
    
    public static List<string> AllNames()
    {
        List<string> toReturn = new List<string>();

        foreach (UnitConfig unitConfig in unitConfigs)
        {
            toReturn.Add(unitConfig.unitName);
        }

        return(toReturn);
    }
    
    public static List<string> AllClasses()
    {
        List<string> toReturn = new List<string>();

        foreach (UnitConfig unitConfig in unitConfigs)
        {
            toReturn.Add(unitConfig.unitClass.ToString());
        }

        return(toReturn);
    }
}