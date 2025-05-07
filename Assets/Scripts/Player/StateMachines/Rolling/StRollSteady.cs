using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StRollSteady : StRoll{

    public StRollSteady(ContRolling _contRolling) : base(_contRolling) { }

    public override void PseudoFixedUpdate() {

        //Debug.Log("Angular velocity is " + plyrOwner.GetComponent<Rigidbody2D>().angularVelocity);

        //If we go above the threshold angular velocity, then move to the Rolling state
        if (Mathf.Abs(plyrOwner.GetComponent<Rigidbody2D>().angularVelocity) > contRolling.fStartRollingThreshold) {
            Transition(new StRollRolling(contRolling));
        }
    }

}
