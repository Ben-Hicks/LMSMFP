using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StDashMidDash : StDash{

    public StDashMidDash(ContDashing _contDashing) : base(_contDashing) {

    }

    public override void OnEnter() {

        contDashing.fCurDashTime = 0f;

        plyrOwner.OnMovementInput();

        contDashing.GetComponent<Rigidbody2D>().gravityScale = 0;

    }

    public override void OnFixedUpdate() {

        contDashing.fCurDashTime += Time.fixedDeltaTime;

        if(contDashing.fCurDashTime >= contDashing.fDashTime) {
            Transition(new StDashReady(contDashing));
        } else {
            DashMovement();
        }
       
    }

    void DashMovement() {
        switch (contDashing.curDashDirection) {
            case ContDashing.DashDirection.LEFT:

                contDashing.GetComponent<Rigidbody2D>().velocity = new Vector2(-contDashing.fDashSpeed, 0);

                break;

            case ContDashing.DashDirection.RIGHT:

                contDashing.GetComponent<Rigidbody2D>().velocity = new Vector2(contDashing.fDashSpeed, 0);

                break;
        }
    }

    void EndDash() {

        float fUpwardNudge = contDashing.fDashUpwardsNudge;

        if (plyrOwner.curCollision.bFloor) {
            //If we're touching the ground, then we don't need to nudge ourselves upwards
            fUpwardNudge = 0f;
        }

        float fSideNudge = contDashing.fDashSideNudge;

        if(contDashing.curDashDirection == ContDashing.DashDirection.LEFT) {
            fSideNudge *= -1;
        }

        //Stop the character and apply a slight upward force when ending the dash
        contDashing.GetComponent<Rigidbody2D>().velocity = new Vector2(fSideNudge, fUpwardNudge);

        contDashing.curDashDirection = ContDashing.DashDirection.NONE;
        
    }


    public override void OnLeave() {

        EndDash();

        contDashing.fCurDashTime = 0f;

        contDashing.cooldown.SetCooldown(contDashing.fCooldownBetweenDashes);

        contDashing.GetComponent<Rigidbody2D>().gravityScale = 1;
        
    }
}
