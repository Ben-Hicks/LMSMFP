using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StLaunchReady : StLaunch {

    public StLaunchReady(ContLaunchShooter _contLaunchShooter) :base (_contLaunchShooter) { }

    public override void PseudoFixedUpdate(){

        //If we're touching the ground
        if (plyrOwner.curCollision.bFloor) {
            //Then refresh our launching ability
            //contLaunchShooter.cooldown.ResetCooldown();
        }

    }

    public override void HandleLaunchInput() {

        if (plyrOwner.bMovementLocked == false) {
            //Check if we're pressing the shoot button and that we have enough charges to actually shoot
            if (plyrOwner.contInput.bShootLaunchingWeb && contLaunchShooter.cooldown.CanUse()) {
                ShootLaunchingWeb(plyrOwner.contInput.v2WebReticle);
            }
        }

    }

    public void ShootLaunchingWeb(Vector2 v2MousePosition) {

        contLaunchShooter.goCurLaunchWeb = GameObject.Instantiate(contLaunchShooter.pfLaunchingWeb, contLaunchShooter.transform.position, Quaternion.identity);

        Web launchWeb = contLaunchShooter.goCurLaunchWeb.GetComponent<Web>();
        
        launchWeb.fSpeed = contLaunchShooter.fWebShotSpeed;
        launchWeb.SetOwner(contLaunchShooter.gameObject);
        launchWeb.SetTarget(v2MousePosition, contLaunchShooter.fMaxWebShotLength);
        launchWeb.webType = Web.WebType.LAUNCHING;

        contLaunchShooter.cooldown.SetCooldown(contLaunchShooter.fCooldownBetweenShots);

        Transition(new StLaunchShooting(contLaunchShooter));

    }

}
