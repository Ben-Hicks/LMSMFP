using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StExpressionHappy : StExpression {

    public StExpressionHappy(ContExpressions _contExpressions) : base(_contExpressions) {

    }

    public override void PseudoFixedUpdate() {

        //Only look for new transitions if we've finished our happy duration
        if (FinishedHappy()) {
            UpdateTransitions();
        }

    }

    bool FinishedHappy() {
        //Currently always staying happy
        return false;
        return fTimeInState >= contExpressions.fTimeHappy;
    }


    public override void DecideSprite() {
        contExpressions.spriteRendererExpression.sprite = contExpressions.sprHappy;
    }
}
