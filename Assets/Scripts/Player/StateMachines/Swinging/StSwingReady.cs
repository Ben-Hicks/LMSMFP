using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StSwingReady : StSwing {

    public StSwingReady(ContSwingShooter _contSwingShooter) : base(_contSwingShooter) { }

    public override void PseudoFixedUpdate() {

        //If we're touching the ground
        if (plyrOwner.curCollision.bFloor) {
            //Then refresh our webshooting ability
            //contSwingShooter.cooldown.ResetCooldown();
        }

    }

    public override void PsuedoUpdate() {

        if (plyrOwner.bMovementLocked == false) {
            //Check if we're pressing the shoot button and that we have enough charges to actually shoot
            if (plyrOwner.contInput.bShootSwingingWeb && contSwingShooter.cooldown.CanUse()) {
                ShootSwingingWeb(plyrOwner.contInput.v2WebReticle);
            }
        }

    }

    public void ShootSwingingWeb(Vector2 v2MousePosition) {

        contSwingShooter.goCurSwingWeb = GameObject.Instantiate(contSwingShooter.pfSwingingWeb, contSwingShooter.transform.position, Quaternion.identity);

        Web web = contSwingShooter.goCurSwingWeb.GetComponent<Web>();

        web.fSpeed = contSwingShooter.fWebShotSpeed;
        web.SetOwner(contSwingShooter.gameObject);
        web.SetTarget(v2MousePosition, contSwingShooter.fMaxWebShotLength);
        web.webType = Web.WebType.SWINGING;

        contSwingShooter.cooldown.SetCooldown(contSwingShooter.fCooldownBetweenShots);

        Transition(new StSwingShooting(contSwingShooter));

    }
}
