using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StExpressionFear : StExpression {

    public StExpressionFear(ContExpressions _contExpressions) : base(_contExpressions) {

    }

    public override void UpdateTransitions() {

        if (CheckNearFly()) {
            Transition(new StExpressionNearFly(contExpressions));
        } else if (CheckNearHazard()) {
            //If we're still near a hazard, then we don't need to look further for new transitions
            return;
        } else if (CheckGoingFast()) {
            Transition(new StExpressionFast(contExpressions));
        } else {
            Transition(new StExpressionNeutral(contExpressions));
        }
    }

    public override void DecideSprite() {
        contExpressions.spriteRendererExpression.sprite = contExpressions.sprFear;
    }
}
