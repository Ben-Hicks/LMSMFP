using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StJumpAirborneMidJump : StJump {

    bool bEndedJump;
    public bool bIsWallJump;
    public bool bWallJumpingLeft;

    public StJumpAirborneMidJump(ContJumping _contJumping, bool _bIsWallJump, bool _bWallJumpingLeft = false) : base(_contJumping) {
        bIsWallJump = _bIsWallJump;
        bWallJumpingLeft = _bWallJumpingLeft;
    }

    public override void OnEnter() {
        bEndedJump = false;

        plyrOwner.OnMovementInput();

        contJumping.fCurJumpTime = 0f;
        SetPlayerYVelocity(contJumping.fAirborneJumpVelocity);

        //Let go of a swinging web if we're holding one
        contJumping.GetComponent<ContSwingShooter>().Detach();

        

        //Set the cooldown
        if(bIsWallJump == false) {
            //Only incur the cooldown if this isn't a wall jump
            contJumping.cooldown.SetCooldown(contJumping.fJumpCooldown);
        }
        contJumping.cooldown.SetCooldown(contJumping.fCooldownBetweenJumps);
    }

    public override void PseudoFixedUpdate() {

        contJumping.fCurJumpTime += Time.fixedDeltaTime;

        //Check if we're walljumping, and thus should be maintaining sideways momentum for a second after starting the jump
        if(bIsWallJump && contJumping.fCurJumpTime < contJumping.plyrOwner.contWallStick.fMinJumpTime) {

            if (bWallJumpingLeft) {
                SetPlayerXVelocity(-contJumping.plyrOwner.contWallStick.fWallJumpShoveX);
            } else {
                SetPlayerXVelocity(contJumping.plyrOwner.contWallStick.fWallJumpShoveX);
            }
        }

        //Check if we've been jumping for too long
        if (plyrOwner.bMovementLocked || (contJumping.fCurJumpTime >= contJumping.fMaxAirborneJumpTime && bEndedJump == false)) {
            bEndedJump = true;
            EndJump();
        }
    }

    public override void PseudoUpdate() {

        //If we're trying to stop jumping, ensure we've jumped for at least the minimum
        if (plyrOwner.bMovementLocked || (plyrOwner.contInput.bJump == false && contJumping.fCurJumpTime >= contJumping.fMinAirborneJumpTime)) {

            Transition(new StJumpAirborne(contJumping));
        }
    }

    void SetPlayerXVelocity(float fVelocity) {
        contJumping.GetComponent<Rigidbody2D>().velocity = new Vector2(fVelocity, contJumping.GetComponent<Rigidbody2D>().velocity.y);
    }

    void SetPlayerYVelocity(float fVelocity) {
        contJumping.GetComponent<Rigidbody2D>().velocity = new Vector2(contJumping.GetComponent<Rigidbody2D>().velocity.x, fVelocity);
    }

    void EndJump() {
        SetPlayerYVelocity(Mathf.Max(contJumping.fAirborneFloatVelocity, plyrOwner.rb.velocity.y));
        //contJumping.GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    public override void OnLeave() {

        //End the jump (if we haven't already due to time)
        if (bEndedJump == false) {
            bEndedJump = true;
            EndJump();
        }
    }
}
