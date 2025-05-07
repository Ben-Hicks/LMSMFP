using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StRoll : State{

    public ContRolling contRolling;

    //Effectively wrap the shooter's player owner and present it as ours as well
    public Player plyrOwner {
        get {
            return contRolling.plyrOwner;
        }
    }

    public StRoll(ContRolling _contRolling) {
        contRolling = _contRolling;
    }

    public void HandleRollInput() {

        Debug.LogError("Rolling is not currently supported");

        if (plyrOwner.contInput.bDashLeft) {
            //plyrOwner.fTorqueToAdd += contRolling.fRotateSpeed;
        }
        if (plyrOwner.contInput.bDashRight) {
            //plyrOwner.fTorqueToAdd -= contRolling.fRotateSpeed;
        }
    }

}
