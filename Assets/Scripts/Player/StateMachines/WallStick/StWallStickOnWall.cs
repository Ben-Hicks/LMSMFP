using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StWallStickOnWall : StWallStick {

    
    public float fTimeOnWall;

    public StWallStickOnWall(ContWallStick _contWallsStick, STICKDIR _stuckDir) : base(_contWallsStick) {
        stuckDir = _stuckDir;
    }

    public override void OnEnter() {
        base.OnEnter();

        //Reset our jumping cooldown when touching a wall
        contWallStick.plyrOwner.contJumping.cooldown.ResetCooldown();
    }

    public void CheckStillOnWall() {

        if ((stuckDir == STICKDIR.LEFT && contWallStick.plyrOwner.curCollision.bLeftWall) ||
            (stuckDir == STICKDIR.RIGHT && contWallStick.plyrOwner.curCollision.bRightWall)) {
            //Then we're still on the wall

        } else {
            //Then we're no longer on a wall
            Transition(new StWallStickNeutral(contWallStick));
        }

    }


    public override void PseudoFixedUpdate() {

        CheckStillOnWall();

    }
}
