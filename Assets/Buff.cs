using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Buff : MonoBehaviour
{
  public int turnsLeft;
  public Unit unit;

  public abstract string Name();

  public abstract string Description();

  public virtual bool ExpiresOnDeath(){
    return(true);
  }

  public virtual float ReturnMpMod(){
    return(1);
  }

  public virtual UnitAction.ActionType CommunicableType(){
    return(UnitAction.ActionType.None);
  }

  public virtual bool ExpiresOnRevive(){
    return(false);
  }

  public void Remove(){
    Destroy(gameObject);
  }

  public virtual float TurnsDeadMod(){
    return(1);
  }

  public void DeductTurn(){
    turnsLeft--;
  }

  public int TurnsLeft(){
    return(turnsLeft);
  }

  public virtual float MeleeResistMod(){
    return(1);
  }

  public virtual float MeleeAttackMod(){
    return(1);
  }

  public virtual float MoveMod(){
    return(1);
  }
  
  public virtual float JumpMod(){
    return(1);
  }

  public virtual bool CanTarget(Cursor target){
    return(true);
  }

  public virtual bool CanMoveTo(Cursor target){
    return(true);
  }

  public virtual bool CanUseRanged(){
    return(true);
  }
  
  public virtual bool CanUseMagic(){
    return(true);
  }
  
  public virtual bool CanUseSupport(){
    return(true);
  }

  public virtual bool CanBeSeenThroughObjects(){
    return(false);
  }

  public virtual void OnTurnStart(){
  }
}