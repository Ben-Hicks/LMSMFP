using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StExpressionNeutral : StExpression{

    

    public StExpressionNeutral(ContExpressions _contExpressions) : base(_contExpressions) {

    }

    public override bool CanTransToNeutral() {
        //We don't want to continually transition on every frame
        return false;
    }

    public override void DecideSprite() {

        switch (curDirectionFacing) {
            case DirectionFacing.UP:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprNeutralUp;
                break;
            case DirectionFacing.LEFT:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprNeutralSide;
                contExpressions.spriteRendererExpression.transform.localScale = new Vector3(-0.5f, contExpressions.spriteRendererExpression.transform.localScale.y);
                break;
            case DirectionFacing.RIGHT:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprNeutralSide;
                contExpressions.spriteRendererExpression.transform.localScale = new Vector3(0.5f, contExpressions.spriteRendererExpression.transform.localScale.y);
                break;
            case DirectionFacing.DOWN:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprNeutralDown;
                break;
            default:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprNeutral;
                break;
        }
    }

}
