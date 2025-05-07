using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StSwingAttached : StSwing {

    float fDesiredWebLength;
    float fTimeAttached;
    
    
    public StSwingAttached(ContSwingShooter _contSwingShooter) : base(_contSwingShooter) { }


    public override void OnEnter() {

        //Set our desired length to be the length at which we connected
        fDesiredWebLength = contSwingShooter.GetComponent<DistanceJoint2D>().distance;

        contSwingShooter.GetComponent<ContJumping>().cooldown.ResetCooldown();

        //Increase the length of the web slightly to allow the spider to stretch out the web if falling quickly
        contSwingShooter.GetComponent<DistanceJoint2D>().distance += contSwingShooter.fMaxElasticDistance;
    }

    public override void PseudoFixedUpdate() {
    fTimeAttached += Time.fixedDeltaTime;

        if (contSwingShooter.fMaxElasticDistance > 0) {
            //If we are enabling elasticity, then we need to ensure our weblength is correct
            ApplyElasticity();
        }
    }

    public override void PsuedoUpdate() {

        //Wait for releasing the web
        if (plyrOwner.bMovementLocked || plyrOwner.contInput.bShootSwingingWebHeld == false) {

            //Detach ourselves
            Detach();

        }else if (plyrOwner.contInput.bExtend) {
            //Then extend the web
            Debug.Assert(goCurSwingWeb != null);

            Extend();

        } else if (plyrOwner.contInput.bRetract) {
            //Then retract the web
            Debug.Assert(goCurSwingWeb != null);

            Retract();
        }

    }

    public void Extend() {
        //Only allow extensions if they wouldn't put us over the maximum (note that this doesn't shorten the length if it was already above the maximum)
        if (contSwingShooter.GetComponent<DistanceJoint2D>().distance < contSwingShooter.fMaxWebLength) {
            contSwingShooter.GetComponent<DistanceJoint2D>().distance += contSwingShooter.fExtendSpeed * Time.fixedDeltaTime;
            fDesiredWebLength += contSwingShooter.fExtendSpeed * Time.fixedDeltaTime;
        }
    }

    public void Retract() {
        //Only allow retractions if they wouldn't put us below the minimum (note that this doesn't extend the length if it was already below the minimum)
        if (contSwingShooter.GetComponent<DistanceJoint2D>().distance > contSwingShooter.fMinWebLength) {
            contSwingShooter.GetComponent<DistanceJoint2D>().distance -= contSwingShooter.fRetractSpeed * Time.fixedDeltaTime;
            fDesiredWebLength -= contSwingShooter.fRetractSpeed * Time.fixedDeltaTime;
        }
    }

    public void ApplyElasticity() {

        Vector2 v2Dist = goCurSwingWeb.transform.position - contSwingShooter.transform.position;

        //If we're extended too much, pull ourselves closer to the web
        if (v2Dist.magnitude - fDesiredWebLength > 0.1) {

            Vector2 v2Force = contSwingShooter.fElasticity * v2Dist.normalized;

            contSwingShooter.GetComponent<Rigidbody2D>().AddForce(v2Force);


            //If we're not too far extended, and enough time as passed, reinstate the desired joint distance
        } else if (fTimeAttached > contSwingShooter.fTimeMaxElasticity) {

            contSwingShooter.GetComponent<DistanceJoint2D>().distance = fDesiredWebLength;
        }

    }

    public override void Detach() {

        Rigidbody2D rbAttachedTo = goCurSwingWeb.GetComponent<Web>().goAttachedTo.GetComponent<Rigidbody2D>();

        plyrOwner.rb.AddForce(rbAttachedTo.velocity * plyrOwner.rb.mass * 1, ForceMode2D.Impulse);

        //Detach the owner's joint from this rigidbody (set its rigidbody to itself)
        contSwingShooter.GetComponent<DistanceJoint2D>().connectedBody = contSwingShooter.GetComponent<Rigidbody2D>();

        //TODO::
        //Could also spawn a more complex chain of objects to be more realistic

        goCurSwingWeb.GetComponent<Web>().AddWeight(contSwingShooter.pfWeight);

        Transition(new StSwingReady(contSwingShooter));
    }

    public override void OnLeave() {


        contSwingShooter.cooldown.SetCooldown(contSwingShooter.fSwingCooldown);

        //Let the shooter owner know that its no longer holding a web
        contSwingShooter.goCurSwingWeb = null;
    }
}
