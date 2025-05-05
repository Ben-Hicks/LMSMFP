using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StExpressionFast : StExpression { 

    public StExpressionFast(ContExpressions _contExpressions) : base(_contExpressions) {

    }


    public override void UpdateTransitions() {

        if (CheckNearFly()) {
            Transition(new StExpressionNearFly(contExpressions));
        } else if (CheckNearHazard()) {
            Transition(new StExpressionFear(contExpressions));
        } else if (!CheckGoingFast()) {
            Transition(new StExpressionNeutral(contExpressions));
        }
        //If none of the above happen, then we don't need to transition
    }


    public override void DecideSprite() {

        switch (curDirectionFacing) {
            case DirectionFacing.UP:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprFastUp;
                break;
            case DirectionFacing.LEFT:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprFastSide;
                contExpressions.spriteRendererExpression.transform.localScale = new Vector3(-0.5f, contExpressions.spriteRendererExpression.transform.localScale.y, 1f);
                break;
            case DirectionFacing.RIGHT:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprFastSide;
                contExpressions.spriteRendererExpression.transform.localScale = new Vector3(0.5f, contExpressions.spriteRendererExpression.transform.localScale.y, 1f);
                break;
            case DirectionFacing.DOWN:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprFastDown;
                break;
            default:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprFast;
                break;
        }
    }
}
