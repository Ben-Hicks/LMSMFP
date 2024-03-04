using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StLaunch : State {

    public ContLaunchShooter contLaunchShooter;

    //Effectively wrap the shooter's player owner and present it as ours as well
    public Player plyrOwner {
        get {
            return contLaunchShooter.plyrOwner;
        }
    }

    public StLaunch(ContLaunchShooter _contLaunchShooter) {
        contLaunchShooter = _contLaunchShooter;
    }

    public virtual void HandleLaunchInput() {
        //By default, do nothing
    }

    public virtual void Detach() {
        //By default, do nothing
    }

}
