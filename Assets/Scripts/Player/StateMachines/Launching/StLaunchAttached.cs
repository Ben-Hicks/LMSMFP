using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StLaunchAttached : StLaunch {

    public StLaunchAttached(ContLaunchShooter _contLaunchShooter) : base(_contLaunchShooter) { }

    public override void HandleLaunchInput() {
        
        //Wait for releasing the web
        if(plyrOwner.bMovementLocked || plyrOwner.contInput.bShootLaunchingWebHeld == false) {

            //Launch ourselves
            if (plyrOwner.bMovementLocked == false) {
                Launch();
            }

            Transition(new StLaunchReady(contLaunchShooter));

        }

    }

    public void Launch() {
        //Get the direction of the owner to the target of the web 
        Vector2 v2LaunchDirection = new Vector2(contLaunchShooter.goCurLaunchWeb.transform.position.x - plyrOwner.transform.position.x,
                                                contLaunchShooter.goCurLaunchWeb.transform.position.y - plyrOwner.transform.position.y);

        v2LaunchDirection.Normalize();

        //A velocity version of launching
        plyrOwner.GetComponent<Rigidbody2D>().velocity = (v2LaunchDirection * contLaunchShooter.fLaunchVelocity);

        //A force version of launching
        //goOwner.GetComponent<Rigidbody2D>().AddForce(v2LaunchDirection * fLaunchForce);

        contLaunchShooter.goCurLaunchWeb.GetComponent<Web>().AddWeight(contLaunchShooter.pfWeight);
    }

    public override void Detach() {

        //TODO::
        //Could also spawn a more complex chain of objects to be more realistic

        contLaunchShooter.goCurLaunchWeb.GetComponent<Web>().AddWeight(contLaunchShooter.pfWeight);

        Transition(new StLaunchReady(contLaunchShooter));
    }

    public override void OnLeave() {
        base.OnLeave();

        contLaunchShooter.cooldown.SetCooldown(contLaunchShooter.fLaunchCooldown);
        contLaunchShooter.GetComponent<ContJumping>().cooldown.ResetCooldown();

        //Let the shooter owner know that its no longer holding a web
        contLaunchShooter.goCurLaunchWeb = null;
    }

}
