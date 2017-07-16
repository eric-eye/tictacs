using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffRazz : Buff {

  float attackMod = 1.25f;
  float resistMod = 0.75f;

  public override string Name(){
    return("Razz");
  }

  public override float MeleeAttackMod(){
    return(1.25f);
  }

  public override float MeleeResistMod(){
    return(0.75f);
  }

  public override string Description(){
    return(
      "Increases melee ability damage by " + (attackMod - 1 ) * 100 + "%. " +
      "Reduces melee resistance by " + (1 - resistMod) * 100 + "%."
      );
  }

}
