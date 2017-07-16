using System.Collections;
using System.Collections.Generic;

public class Mage : UnitClass {

	public override List<string> Actions()
	{
		return(new List<string>
			{
				"ActionEmber",
				"ActionEarthSlide",
				"ActionWave",
				"ActionManaBurn",
				"ActionAffinityTransition",
				"ActionAffinityWipe",
				"ActionSiphonMana",
				"ActionBlankStare",
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
		return("Mage");
	}
}
