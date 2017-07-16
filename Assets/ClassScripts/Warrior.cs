using System.Collections;
using System.Collections.Generic;

public class Warrior : UnitClass {

	public override List<string> Actions()
	{
		return(new List<string>
			{
				"ActionAttack",
				"ActionSpinAttack",
				"ActionPunish",
				"ActionHamstring",
				"ActionDelayAttack",
				"ActionTaunt",
				"ActionShout",
				"ActionIntimidate",
				"ActionMendWounds",
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
		return("Warrior");
	}
}
