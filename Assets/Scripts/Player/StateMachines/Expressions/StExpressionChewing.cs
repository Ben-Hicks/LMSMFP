using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StExpressionChewing : StExpression {

    public StExpressionChewing(ContExpressions _contExpressions) : base(_contExpressions) {

    }

    public override void UpdateTransitions() {

        if (CheckNearFly()) {
            //Let us transition to nearfly, even if we haven't finished chewing
            Transition(new StExpressionNearFly(contExpressions));
        } else if (FinishedChewing()) {
            if (LevelType.Get().curStateLevel == LevelType.StateLevel.ENDING
                || LevelType.Get().curStateLevel == LevelType.StateLevel.SHOWINGSTARS) {
                Transition(new StExpressionHappy(contExpressions));
            } else {
                //Only look for more transitions if we're done chewing
                base.UpdateTransitions(); ;
            }
        } 

    }

    bool FinishedChewing() {
        return fTimeInState >= contExpressions.fTimeChewing;
    }

    public override void DecideSprite() {

        switch (curDirectionFacing) {
            case DirectionFacing.UP:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprChewingUp;
                break;
            case DirectionFacing.LEFT:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprChewingSide;
                contExpressions.spriteRendererExpression.transform.localScale = new Vector3(-0.5f, contExpressions.spriteRendererExpression.transform.localScale.y);
                break;
            case DirectionFacing.RIGHT:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprChewingSide;
                contExpressions.spriteRendererExpression.transform.localScale = new Vector3(0.5f, contExpressions.spriteRendererExpression.transform.localScale.y);
                break;
            case DirectionFacing.DOWN:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprChewingDown;
                break;
            default:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprChewing;
                break;
        }
    }

}
