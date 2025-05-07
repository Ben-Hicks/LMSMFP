using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StExpressionSleep : StExpression {

    public StExpressionSleep(ContExpressions _contExpressions) : base(_contExpressions) {

    }

    public override void PseudoFixedUpdate() {
        base.PseudoFixedUpdate();

        DecideSprite();
    }

    public override void UpdateTransitions() {

        if (CheckNearFly()) {
            Transition(new StExpressionNearFly(contExpressions));
        } else if (CheckNearHazard()) {
            Transition(new StExpressionFear(contExpressions));
        } else if (CheckGoingFast()) {
            Transition(new StExpressionFast(contExpressions));
        } else if (WakingUp()) {
            Transition(new StExpressionNeutral(contExpressions));
        }
        //If none of the above happen, then we don't need to transition
    }

    public bool WakingUp() {

        return contExpressions.GetComponent<Rigidbody2D>().velocity.magnitude >= 0.1;

    }

    public override void DecideSprite() {

        int nSnoreCycle = Mathf.FloorToInt(fTimeInState / contExpressions.fSnoreTime);

        if(nSnoreCycle % 2 == 1) {
            contExpressions.spriteRendererExpression.sprite = contExpressions.sprSleep1;
        } else {
            contExpressions.spriteRendererExpression.sprite = contExpressions.sprSleep2;
        }

    }
}
