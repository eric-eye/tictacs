using System.Collections;
using UnityEngine;

public class ActionDash : UnitAction {
  int visualExecutions = 0;

  public override string Name(){
    return("Dash");
  }

  public override string Description(){
    return("This unit dashes to the target loation. This does not count as a move.");
  }

  public override int MaxDistance(){
    return(3);
  }

  public override bool CanTargetOthers(){
    return(false);
  }

  public override void ReceiveVisualFeedback(Cursor cursor){
    visualExecutions++;
    if(visualExecutions <= 1) {
      StartCoroutine(DoScript(cursor));
    }
  }

  protected override void DoAction(Cursor cursor){
    visualExecutions = 0;
    CreateVisual(cursor, Unit().transform.position);
    Unit().SetPosition(-99999, -9999);
    Unit().RenderPosition();
  }

  private IEnumerator DoScript(Cursor tile){
      yield return new WaitForSeconds(1f);
      CreateVisual(tile, tile.transform.position);
      Unit().SetPosition(tile.xPos, tile.zPos);
      Unit().yPos = tile.yPos;
      Unit().RenderPosition();
      yield return new WaitForSeconds(1f);
      Unit().FinishAction();
  }

  public override ActionType actionType() {
    return(ActionType.Support);
  }
}

