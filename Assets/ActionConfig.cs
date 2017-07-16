using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.Collections;
using UnityEngine.UI;

public class ActionConfig : MonoBehaviour
{
	public enum ConfigTypes
	{
		Ability,
		Stance
	};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetUnit(UnitConfig unitConfig, int index, ConfigTypes configType)
	{
		transform.Find("InfoPanel").Find("UnitName").GetComponent<Text>().text = unitConfig.unitName;
		transform.Find("InfoPanel").Find("UnitClass").GetComponent<Text>().text = unitConfig.unitClass.Name();
		
		int i = 0;
		List<string> actions = unitConfig.unitClass.Actions();
        List<string> unitActions = unitConfig.actions;
		
		if (configType == ConfigTypes.Stance)
		{
			actions = unitConfig.unitClass.Stances();
			unitActions = unitConfig.stances;
		}
			
		foreach (Transform entry in transform.Find("Grid"))
		{
			bool enableClick = true;
			int c = i;
			
            entry.GetComponent<Button>().onClick.RemoveAllListeners();
			
			if (i < actions.Count)
			{
				entry.gameObject.active = true;

				string label = "";
				string description = "";
				string mpCost = "";

				if (configType == ConfigTypes.Stance)
				{
					label = unitConfig.stanceMap[actions[c]].Name();
					description = unitConfig.stanceMap[actions[c]].Description();
					mpCost = "0";
				}
				else
				{
					UnitAction action =unitConfig.actionMap[actions[c]];
					label = unitConfig.actionMap[actions[c]].Name();
          description = action.actionType().ToString() + " -- " + action.Description();
					if(action.VariableMp()){
						mpCost = "Variable";
					}else{
						mpCost = unitConfig.actionMap[actions[c]].MpCost().ToString();
					}
				}

        entry.Find("Text").GetComponent<Text>().text = label;

        entry.Find("Text").GetComponent<Text>().color = Color.white;

				if (actions[c] == unitActions[index])
				{
					entry.Find("Text").GetComponent<Text>().color = Color.green;
				}else if(unitActions.Contains(actions[c]))
				{
					enableClick = false;
					entry.Find("Text").GetComponent<Text>().color = new Color(1, 1, 1, .5f);
				}

				ActionConfigButton configButton = entry.GetComponent<ActionConfigButton>();
				configButton.actionName = label;
				configButton.description = description;
				configButton.mpCost = mpCost;
				
				if (enableClick)
				{
                    entry.GetComponent<Button>().onClick.AddListener(
                        () => {
	                        ActionInformation.Hide();
	                        this.SetAction(unitConfig, index, actions[c], configType);
                        });
				}
			}
			else
			{
				entry.gameObject.active = false;
			}
			
            i++;
		}
	}

	private void SetAction(UnitConfig unitConfig, int index, string actionName, ConfigTypes configType)
	{
		if (configType == ConfigTypes.Ability)
		{
            unitConfig.actions[index] = actionName;
		}
		else
		{
            unitConfig.stances[index] = actionName;
		}
		LobbySetupController.HideActionConfig();
		LobbySetupController.ShowUnitConfig();
	}
}
