using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StWallStick : State {

    public enum STICKDIR { NONE, LEFT, RIGHT };
    public STICKDIR stuckDir;

    public ContWallStick contWallStick;

    public StWallStick(ContWallStick _contWallStick) {
        contWallStick = _contWallStick;
    }

}
