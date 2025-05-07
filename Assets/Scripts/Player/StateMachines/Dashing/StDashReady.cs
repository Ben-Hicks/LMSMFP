using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StDashReady : StDash {

    public StDashReady(ContDashing _contDashing) : base(_contDashing) {

    }

    public override void PseudoFixedUpdate() {

        //If we're no longer touching the ground
        if (plyrOwner.curCollision.bFloor) {
            //If we are touching the ground, then we can reset our charges
            contDashing.cooldown.ResetCharges();
        }
    }

    public override void PseudoUpdate() {

        if (plyrOwner.bMovementLocked == false) {
            if (plyrOwner.contInput.bDashLeft && contDashing.cooldown.CanUse()) {
                StartDash(ContDashing.DashDirection.LEFT);
            } else if (plyrOwner.contInput.bDashRight && contDashing.cooldown.CanUse()) {
                StartDash(ContDashing.DashDirection.RIGHT);
            }
        }

    }


    void StartDash(ContDashing.DashDirection dashDir) {

        Debug.Assert(dashDir != ContDashing.DashDirection.NONE);

        contDashing.curDashDirection = dashDir;

        //Let go of a swinging web if we're holding one
        contDashing.GetComponent<ContSwingShooter>().Detach();

        contDashing.cooldown.UseCharge();

        Transition(new StDashMidDash(contDashing));
    }
}
