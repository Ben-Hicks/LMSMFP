using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StSwing : State {

    public ContSwingShooter contSwingShooter;

    //Effectively wrap the shooter's player owner and present it as ours as well
    public Player plyrOwner {
        get {
            return contSwingShooter.plyrOwner;
        }
    }

    public GameObject goCurSwingWeb {
        get {
            return contSwingShooter.goCurSwingWeb;
        }
    }

    public virtual void Detach() {
        //By default, do nothing
    }

    public StSwing(ContSwingShooter _contSwingShooter) {
        contSwingShooter = _contSwingShooter;
    }

    public virtual void HandleSwingInput() {
        //By default, do nothing
    }

}
