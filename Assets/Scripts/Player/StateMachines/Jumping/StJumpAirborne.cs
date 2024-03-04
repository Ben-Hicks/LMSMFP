using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StJumpAirborne : StJump {

    public StJumpAirborne(ContJumping _contJumping) : base(_contJumping) {

    }

    public override void OnFixedUpdate(){

        if (plyrOwner.curCollision.bFloor) {
            Transition(new StJumpReady(contJumping));
        }
    }

    public override void HandleJumpInput() {

        if (plyrOwner.bMovementLocked == false) {
            if (plyrOwner.contInput.bJump && contJumping.cooldown.CanUse()) {
                
                if(plyrOwner.contWallStick.stmachWallStick.stateCur.stuckDir == StWallStick.STICKDIR.LEFT && //If we're against a wall
                    ((contJumping.bWallJumpToward && plyrOwner.contInput.bMoveLeft) ||   //And either doing a valid inverted wall jump
                        (contJumping.bWallJumpToward == false && plyrOwner.contInput.bMoveRight))) {   //Or a valid non-inverted wall jump

                        //If we're on a lefthand wall, then we should launch ourselves off of it to the right
                        plyrOwner.rb.velocity = new Vector2(plyrOwner.contWallStick.fWallJumpShoveX, plyrOwner.rb.velocity.y);

                        Transition(new StJumpAirborneMidJump(contJumping, true, false));
                   
                } else if (plyrOwner.contWallStick.stmachWallStick.stateCur.stuckDir == StWallStick.STICKDIR.RIGHT && //If we're against a wall
                    ((contJumping.bWallJumpToward && plyrOwner.contInput.bMoveRight) ||    //And either doing a valid inverted wall jump
                        (contJumping.bWallJumpToward == false && plyrOwner.contInput.bMoveLeft))) {  //or a valid non-inverted wall jump

                        //If we're on a righthand wall, then we should launch ourselves off of it to the left
                        plyrOwner.rb.velocity = new Vector2(-plyrOwner.contWallStick.fWallJumpShoveX, plyrOwner.rb.velocity.y);

                        Transition(new StJumpAirborneMidJump(contJumping, true, true));
                    
                } else {
                    Transition(new StJumpAirborneMidJump(contJumping, false));
                }
                
            }
        }
    }

}

