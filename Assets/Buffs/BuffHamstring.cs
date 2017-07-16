using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHamstring : Buff {

  float mod = 0.5f;

  public override string Name(){
    return("Hamstring");
  }

  public override float MoveMod(){
    return(mod);
  }

  public override string Description(){
    return("Movement reduced by " + (mod * 100) + ".%");
  }
}
