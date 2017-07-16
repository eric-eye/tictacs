using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffResurrection : Buff {
  float mod = 0.5f;

  public override string Name(){
    return("Resurrection");
  }

  public override float TurnsDeadMod(){
    return(mod);
  }

  public override bool ExpiresOnDeath(){
    return(false);
  }

  public override bool ExpiresOnRevive(){
    return(true);
  }

  public override string Description(){
    return("Death lasts " + (mod * 100) + "% as long as usual");
  }

}
