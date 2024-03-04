using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StDash : State {

    public ContDashing contDashing;

    //Effectively wrap the jumper's player owner and present it as ours as well
    public Player plyrOwner {
        get {
            return contDashing.plyrOwner;
        }
    }

    public StDash(ContDashing _contDashing) {
        contDashing = _contDashing;
    }

    public virtual void HandleDashInput() {
        //By default, do nothing
    }

}
