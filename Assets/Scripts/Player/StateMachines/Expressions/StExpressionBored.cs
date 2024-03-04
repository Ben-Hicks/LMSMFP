using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StExpressionBored : StExpression {

    public StExpressionBored(ContExpressions _contExpressions) : base(_contExpressions) {
        
    }

    public override void UpdateTransitions() {



        if (CheckSleep()) {
            Transition(new StExpressionSleep(contExpressions));
        } else if (CheckNearFly()) {
            Transition(new StExpressionNearFly(contExpressions));
        } else if (CheckNearHazard()) {
            Transition(new StExpressionFear(contExpressions));
        } else if (CheckGoingFast()) {
            Transition(new StExpressionFast(contExpressions));
        } else if (CanTransToNeutral()) {
            Transition(new StExpressionNeutral(contExpressions));
        }
        //If none of the above happen, then we don't need to transition
    }

    public bool CheckSleep() {
        return fTimeInState >= contExpressions.fSleepDelay;
    }


    public override bool CanTransToNeutral() {

        return contExpressions.GetComponent<Rigidbody2D>().velocity.magnitude >= 0.1;
    }

    public override void DecideSprite() {
        contExpressions.spriteRendererExpression.sprite = contExpressions.sprBored;
    }
}
