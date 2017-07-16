using System.Collections;
using System.Collections.Generic;

public class Archer : UnitClass {

	public override List<string> Actions()
	{
		return(new List<string>
			{
				"ActionNormalShot",
				"ActionBlindShot",
				"ActionHeadshot",
				"ActionSpy",
				"ActionBounty",
				"ActionMark",
				"ActionReload",
				"ActionDash",
				"ActionFlatfoot",
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
		return("Archer");
	}
}
