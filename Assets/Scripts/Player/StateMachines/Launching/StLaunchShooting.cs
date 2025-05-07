using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StLaunchShooting : StLaunch {

    public StLaunchShooting(ContLaunchShooter _contLaunchShooter) : base(_contLaunchShooter) { }

    public override void OnEnter() {
        base.OnEnter();

        plyrOwner.OnMovementInput();
    }

    public override void PseudoFixedUpdate() {

        //Check if the web has collided with a surface
        Web.CollisionType collision = contLaunchShooter.goCurLaunchWeb.GetComponent<Web>().ReachedSurface();
        if (collision == Web.CollisionType.STICKABLE) {

            //Debug.Log("Hit a stickable wall - switching to kinematic");
            Rigidbody2D rb = contLaunchShooter.goCurLaunchWeb.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;

            Transition(new StLaunchAttached(contLaunchShooter));

            //Otherwise check if we've collided with a non-stickable surface, or we've travelled the maximum distance
        }else if (collision == Web.CollisionType.NONSTICKABLE ||  contLaunchShooter.goCurLaunchWeb.GetComponent<Web>().HasReachedMaximumLength()) {

            //Debug.Log("Should despawn this web");

            GameObject.Destroy(contLaunchShooter.goCurLaunchWeb);
            contLaunchShooter.goCurLaunchWeb = null;
            Transition(new StLaunchReady(contLaunchShooter));

        } else { 
            //If we haven't reached our maximum distance yet, then keep moving the web
            contLaunchShooter.goCurLaunchWeb.GetComponent<Web>().MoveToTarget();
        }

    }
}
