using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StWallStickNeutral : StWallStick {

    public StWallStickNeutral(ContWallStick _contWallsStick) : base(_contWallsStick) {

    }

    public void CheckOnWall() {

        if (contWallStick.plyrOwner.curCollision.bLeftWall) {
            Transition(new StWallStickOnWall(contWallStick, STICKDIR.LEFT));
        }else if (contWallStick.plyrOwner.curCollision.bRightWall) {
            Transition(new StWallStickOnWall(contWallStick, STICKDIR.RIGHT));
        }
    }

    public override void PsuedoUpdate() {

        CheckOnWall();

    }
}
