using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBlank : Buff {

  public override string Name(){
    return("Blank");
  }

  public override string Description(){
    return("Cannot use magic or support abilities. Lasts 2 turns.");
  }

  public override bool CanUseMagic(){
    return(false);
  }
  
  public override bool CanUseSupport(){
    return(false);
  }
}
