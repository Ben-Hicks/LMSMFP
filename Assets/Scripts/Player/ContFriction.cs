using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContFriction : MonoBehaviour {

    [Header("Configurable")]
    public float fAirborneFriction;
    public float fGroundFriction;
    public float fMinVelocityThreshold;

    public float fSwingFriction;
    public float fMinSwingVelocityThreshold;

    [Header("Properties")]
    public Player player;
    public float fTargetVelocityX;

    public enum FRICTIONTYPE { NONE, FLOOR, SWINGING, AIR }
    public FRICTIONTYPE fricType;

    public void Start() {
        player = GetComponent<Player>();
    }

    public void LimitMaxVelocity() {

        //Do a check to make sure we're not accelerating too fast in one direction
        if (player.rb.velocity.x < -player.fMaxMoveVelocity + fTargetVelocityX) {
            player.fxForceToAdd = Mathf.Max(player.fxForceToAdd, 0);
        } else if (player.rb.velocity.x > player.fMaxMoveVelocity + fTargetVelocityX) {
            player.fxForceToAdd = Mathf.Min(player.fxForceToAdd, 0);
        }

        //Do a check to make sure we're not rotating too fast in one direction
        if (player.rb.angularVelocity < -player.fMaxRotateVelocity) {
            player.fTorqueToAdd = Mathf.Max(player.fTorqueToAdd, 0);
        } else if (player.rb.angularVelocity > player.fMaxRotateVelocity) {
            player.fTorqueToAdd = Mathf.Min(player.fTorqueToAdd, 0);
        }

    }

    public void ApplyFriction() {
        if (player.curCollision.bFloor) {
            ApplyGroundFriction();
        } else if (GetComponent<ContSwingShooter>().stmachSwing.stateCur.GetType().ToString() == "StSwingAttached") {
            ApplySwingingFriction();
        } else {
            ApplyAirborneFriction();
        }

    }



    void ApplyCounterForce(float fMagnitude) {
        if (player.rb.velocity.x - fTargetVelocityX < -fMinVelocityThreshold) {
            player.rb.AddForce(new Vector2(fMagnitude, 0));
            //Debug.Log("Applied a counter force of " + fMagnitude);
        }else if (player.rb.velocity.x - fTargetVelocityX > fMinVelocityThreshold) {
            player.rb.AddForce(new Vector2(-fMagnitude, 0));
            //Debug.Log("Applied a counter force of " + (-fMagnitude));
        } else {
            Debug.LogError("Not enough velocity to apply friction");
        }


    }

    void ApplySwingingFriction() {
        fricType = FRICTIONTYPE.SWINGING;

        Vector2 v2CurVelocity = GetComponent<Rigidbody2D>().velocity;

        //Currently only applying friction if you're pressing neither direction while swinging
        if(!player.contInput.bMoveLeft && !player.contInput.bMoveRight && v2CurVelocity.magnitude > fMinSwingVelocityThreshold) {
            player.rb.AddForce(v2CurVelocity.normalized * -fSwingFriction);
        }
    }

     void ApplyAirborneFriction() {
        fricType = FRICTIONTYPE.AIR;

        if(player.rb.velocity.x - fTargetVelocityX < -fMinVelocityThreshold && !player.contInput.bMoveLeft) {
            ApplyCounterForce(fAirborneFriction);
        } else if (player.rb.velocity.x - fTargetVelocityX > fMinVelocityThreshold && !player.contInput.bMoveRight) {
            ApplyCounterForce(fAirborneFriction);
        }

    }

    void ApplyGroundFriction() {
        fricType = FRICTIONTYPE.FLOOR;

        if (player.rb.velocity.x - fTargetVelocityX < -fMinVelocityThreshold && !player.contInput.bMoveLeft) {
            ApplyCounterForce(fGroundFriction);
        } else if (player.rb.velocity.x - fTargetVelocityX > fMinVelocityThreshold && !player.contInput.bMoveRight) {
            ApplyCounterForce(fGroundFriction);
        }
    }
}
