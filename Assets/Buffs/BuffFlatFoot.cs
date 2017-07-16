using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffFlatFoot : Buff {

  public override string Name(){
    return("Flat Footed");
  }

  public override float JumpMod(){
    return(0);
  }

  public override string Description(){
    return("Cannot jump.");
  }
}
