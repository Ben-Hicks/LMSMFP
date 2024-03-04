using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StExpressionNearFly : StExpression {

    public StExpressionNearFly(ContExpressions _contExpressions) : base(_contExpressions) {

    }

    public override void UpdateTransitions() {

        // Search for normal transitions only if there's no flies nearby

        if (!CheckNearFly()) {
            base.UpdateTransitions();
        }

    }

    public override void DecideSprite() {

        switch (curDirectionFacing) {
            case DirectionFacing.UP:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprNearFlyUp;
                break;
            case DirectionFacing.LEFT:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprNearFlySide;
                contExpressions.spriteRendererExpression.transform.localScale = new Vector3(-0.5f, contExpressions.spriteRendererExpression.transform.localScale.y);
                break;
            case DirectionFacing.RIGHT:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprNearFlySide;
                contExpressions.spriteRendererExpression.transform.localScale = new Vector3(0.5f, contExpressions.spriteRendererExpression.transform.localScale.y);
                break;
            case DirectionFacing.DOWN:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprNearFlyDown;
                break;
            default:
                contExpressions.spriteRendererExpression.sprite = contExpressions.sprNearFly;
                break;
        }
    }
}
