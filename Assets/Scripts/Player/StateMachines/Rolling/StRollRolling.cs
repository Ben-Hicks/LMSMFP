using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StRollRolling : StRoll{

    

    public StRollRolling(ContRolling _contRolling) : base(_contRolling) { }

    public override void PseudoFixedUpdate() {

        //If we drop below the threshold angular velocity, then move to the Steady state
        if (Mathf.Abs(plyrOwner.GetComponent<Rigidbody2D>().angularVelocity) < contRolling.fStopRollingThreshold){
            Transition(new StRollSteady(contRolling));
        }
    }
}
