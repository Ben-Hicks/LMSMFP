using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StJumpReady : StJump {

    public StJumpReady(ContJumping _contJumping) : base(_contJumping){
        
    }

    public override void PseudoFixedUpdate() {

        //If we're no longer touching the ground
        if (plyrOwner.curCollision.bFloor == false) {
            Transition(new StJumpAirborne(contJumping));
        } else {
            //If we are touching the ground, then we can reset the jump cooldown
            contJumping.cooldown.ResetCooldown();
        }
    }

    public override void HandleJumpInput() {

        //If we're pressing jump and it's off cooldown
        if (plyrOwner.bMovementLocked == false) {
            if (plyrOwner.contInput.bJump && contJumping.cooldown.CanUse()) {

                Transition(new StJumpMidJump(contJumping));

            }
        }

    }

}
