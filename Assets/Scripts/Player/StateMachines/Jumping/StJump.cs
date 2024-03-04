using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StJump : State {

    public ContJumping contJumping;

    //Effectively wrap the jumper's player owner and present it as ours as well
    public Player plyrOwner {
        get {
            return contJumping.plyrOwner;
        }
    }

    public StJump(ContJumping _contJumping) {
        contJumping = _contJumping;
    }

    public virtual void HandleJumpInput() {
        //By default, do nothing
    }

}
