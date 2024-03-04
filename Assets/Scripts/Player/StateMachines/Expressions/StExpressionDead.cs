using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StExpressionDead : StExpression {

    public StExpressionDead(ContExpressions _contExpressions) : base(_contExpressions) {

    }


    public override void OnFixedUpdate() {
        //We don't need to update our time in this state, or look to transition out of it
    }

    public override void DecideSprite() {

        if (contExpressions.spriteRendererExpression.sprite != contExpressions.sprDead) {
            Debug.Log("Switching to dead sprite");
            contExpressions.spriteRendererExpression.sprite = contExpressions.sprDead;
        }

    }


}
