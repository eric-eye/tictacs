using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBlind : Buff {

  public override string Name(){
    return("Blind");
  }

  public override bool CanUseRanged(){
    return(false);
  }

  public override string Description(){
    return("Cannot use ranged abilities.");
  }
}
