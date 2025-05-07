using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StJumpMidJump : StJump {

    bool bEndedJump;

    public StJumpMidJump(ContJumping _contJumping) : base(_contJumping) {

    }

    public override void OnEnter() {
        bEndedJump = false;

        plyrOwner.OnMovementInput();

        //Let go of a swinging web if we're holding one
        contJumping.GetComponent<ContSwingShooter>().Detach();

        contJumping.fCurJumpTime = 0f;
        //contJumping.GetComponent<Rigidbody2D>().gravityScale = 0;
        SetPlayerYVelocity(contJumping.fJumpVelocity);

        //Since this is our first jump, our cooldown is small so that we just don't chain two jumps together
        contJumping.cooldown.SetCooldown(contJumping.fCooldownBetweenJumps);
    }

    public override void PseudoFixedUpdate() {

        contJumping.fCurJumpTime += Time.fixedDeltaTime;

        //Check if we've been jumping for too long
        if(plyrOwner.bMovementLocked || (contJumping.fCurJumpTime >= contJumping.fMaxJumpTime && bEndedJump == false)) {
            bEndedJump = true;
            EndJump();
        }
        
    }

    public override void HandleJumpInput() {

        //If we're trying to stop jumping, ensure we've jumped for at least the minimum
        if(plyrOwner.bMovementLocked || (plyrOwner.contInput.bJump == false && contJumping.fCurJumpTime >= contJumping.fMinJumpTime)) {

            Transition(new StJumpAirborne(contJumping));
        } 
    }

    void SetPlayerYVelocity(float fVelocity) {
        contJumping.GetComponent<Rigidbody2D>().velocity = new Vector2(contJumping.GetComponent<Rigidbody2D>().velocity.x, fVelocity);
    }

    void EndJump() {
        SetPlayerYVelocity(contJumping.fFloatVelocity);
        //contJumping.GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    public override void OnLeave() {

        //End the jump (if we haven't already due to time)
        if(bEndedJump == false) {
            bEndedJump = true;
            EndJump();
        }
    }
}
