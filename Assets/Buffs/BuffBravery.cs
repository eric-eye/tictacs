using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBravery : Buff {

  float mod = 1.5f;

  public override string Name(){
    return("Bravery");
  }

  public override float MeleeResistMod(){
    return(mod);
  }

  public override string Description(){
    return("Increases melee resistance by " + (mod - 1) * 100 + "%. Lasts 2 turns.");
  }

}
