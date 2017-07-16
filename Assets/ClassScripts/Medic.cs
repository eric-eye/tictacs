using System.Collections;
using System.Collections.Generic;

public class Medic : UnitClass {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override List<string> Actions()
	{
		return(new List<string>
			{
				"ActionHeal",
				"ActionPanacea",
				"ActionBravery",
				"ActionSickness",
				"ActionFear",
				"ActionResurrection",
				"ActionHaste",
				"ActionStaffAttack",
			}
		);
	}
	
	public override List<string> Stances()
	{
		return(new List<string>
			{
				"StanceDefend",
				"StanceDefendMelee",
				"StanceDefendRanged",
				"StanceDefendMagic",
			}
		);
	}

	public override string Name()
	{
		return("Medic");
	}
}
