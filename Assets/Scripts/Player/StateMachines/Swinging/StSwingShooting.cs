using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StSwingShooting : StSwing {

    public StSwingShooting(ContSwingShooter _contSwingShooter) : base(_contSwingShooter) { }

    public override void OnEnter() {
        base.OnEnter();

        plyrOwner.OnMovementInput();
    }

    public override void OnFixedUpdate() {

        //Check if the web has collided with a surface
        Web.CollisionType collision = contSwingShooter.goCurSwingWeb.GetComponent<Web>().ReachedSurface();
        if (collision == Web.CollisionType.STICKABLE) {

            Rigidbody2D rb = contSwingShooter.goCurSwingWeb.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;

            Attach();

            Transition(new StSwingAttached(contSwingShooter));

            //Otherwise check if we've collided with a non-stickable surface, or we've travelled the maximum distance
        } else if (collision == Web.CollisionType.NONSTICKABLE || contSwingShooter.goCurSwingWeb.GetComponent<Web>().HasReachedMaximumLength()) {

            //Debug.Log("Should despawn this web");

            GameObject.Destroy(contSwingShooter.goCurSwingWeb);
            contSwingShooter.goCurSwingWeb = null;
            Transition(new StSwingReady(contSwingShooter));

        } else {
            //If we haven't reached our maximum distance yet, then keep moving the web
            contSwingShooter.goCurSwingWeb.GetComponent<Web>().MoveToTarget();
        }

    }

    public void Attach() {

        Debug.Assert(goCurSwingWeb != null);

        //Attach the owner's joint to this rigidbody
        contSwingShooter.GetComponent<DistanceJoint2D>().distance = Vector2.Distance(contSwingShooter.transform.position, goCurSwingWeb.transform.position);
        contSwingShooter.GetComponent<DistanceJoint2D>().connectedBody = goCurSwingWeb.GetComponent<Rigidbody2D>();
    }
}
