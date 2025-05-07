using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlatformMoveable : NonPhysicsMoveable {

    public Platform platform;
    public Rigidbody2D rb;

    public void Start() {
        platform = GetComponent<Platform>();
        rb = GetComponent<Rigidbody2D>();
        platform.subAddedStandingOn.Subscribe(cbOnAddedStandingOn);
        platform.subRemovedStandingOn.Subscribe(cbOnRemovedStandingOn);
    }

    public void cbOnAddedStandingOn(Object tar, params object[] args) {
        ApplyImpulseToObjectStandingOn((Rigidbody2D)tar);
    }

    public override void AfterSettingMovingForce() {
        ApplyImpulseToAllObjectsStandingOn();
    }

    public void ApplyImpulseToAllObjectsStandingOn() {
        foreach (Rigidbody2D rbStandingOn in platform.dictrb2dStandingOn.Keys) {

            ApplyImpulseToObjectStandingOn(rbStandingOn);
        }
    }

    public void ApplyImpulseToObjectStandingOn(Rigidbody2D rbStandingOn) {

        if(rbStandingOn.transform.parent == this.transform.parent) {
            //If this rigidbody is already attached to us, then we don't need to apply any impulses to it
            return;
        }

        Debug.LogFormat("We want to apply an impulse to {0}", rbStandingOn.gameObject.name);
        rbStandingOn.AddForce(rbStandingOn.mass * v3MovingForce, ForceMode2D.Impulse);

        //If we have an object on top of ourselves that we want to pull alongside us as we move, 
        //   then we can direct it's friction component to maintain a desired matching X velocity
        ContFriction contFriction = rbStandingOn.GetComponent<ContFriction>();
        if(contFriction != null) {
            rbStandingOn.GetComponent<ContFriction>().fExternallyMaintainedVelocityX = v3MovingForce.x;
        }
    }

    public void cbOnRemovedStandingOn(Object tar, params object[] args) {
        CounteractObjectMovingForceWhenStopping((Rigidbody2D) tar);
    }

    public override void BeforeSettingMovingForce() {
        CounteractAllObjectMovingForceWhenStopping();
    }

    public void CounteractAllObjectMovingForceWhenStopping() {
        //Only apply a counteracting force if you are either moving the platform down or, not at all vertically
        // - this means that only if you are moving up, will you continue to be flung in that direction
        if (v3MovingForce.y > 0.1f) return;

        foreach (Rigidbody2D rbStandingOn in platform.dictrb2dStandingOn.Keys) {
            CounteractObjectMovingForceWhenStopping(rbStandingOn);
        }
    }
    public void CounteractObjectMovingForceWhenStopping(Rigidbody2D rbStandingOn) {

        if (rbStandingOn.transform.parent == this.transform.parent) {
            //If this rigidbody is already attached to us, then we don't need to apply any impulses to it
            return;
        }

        //Only apply a counteracting force if you are either moving the platform down or, not at all vertically
        // - this means that only if you are moving up, will you continue to be flung in that direction
        if (v3MovingForce.y > 0.1f) return;

        rbStandingOn.AddForce(rbStandingOn.mass * -v3MovingForce, ForceMode2D.Impulse);

        ContFriction contFriction = rbStandingOn.GetComponent<ContFriction>();
        if (contFriction != null) {
            rbStandingOn.GetComponent<ContFriction>().fExternallyMaintainedVelocityX = 0;
        }
    }


    public void PullAttached(Web web) {

        Rigidbody2D rbOwner = web.goOwner.GetComponent<Rigidbody2D>();

        if (rbOwner != null && web.webType == Web.WebType.SWINGING) {

            //rbOwner.AddForce((rbPlatform.position - rbOwner.position).normalized * 0.4f, ForceMode2D.Impulse);
            rbOwner.AddForce(this.rb.velocity * rbOwner.mass, ForceMode2D.Impulse);
        }
    }

    public void PullAttachedWebs() {

        StickableSurface stick = GetComponent<StickableSurface>();

        if (stick == null) return;

        foreach (Web web in stick.lstStuckWebs) {

            PullAttached(web);

        }

    }

    public override void FixedUpdate() {

        base.FixedUpdate();

    }

    
}
