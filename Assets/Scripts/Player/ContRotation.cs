using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContRotation : MonoBehaviour{

    public float fNearGround;
    public GameObject goBody;

    public float fGroundSensitivity;
    public float fAirSensitivity;
    public float fDashSensitivity;

    public float fMinVelocity;
    public float fLowVelocity;
    public float fMaxVelocity;

    float fSensitivity;


    public void Update() {

        Vector2 v2TargetRotation;

        if (GetComponent<ContDashing>().curDashDirection != ContDashing.DashDirection.NONE) {
            v2TargetRotation = GetRotationDashing();
        }else if(NearGround()) {
            v2TargetRotation = GetRotationGround();
        } else {
            v2TargetRotation = GetRotationVelocity();
        }

        float fCurRotation = goBody.transform.localEulerAngles.z;

        float fAngle = Mathf.Atan2(v2TargetRotation.y, v2TargetRotation.x);

        float fDesiredRotation = -90f + fAngle * Mathf.Rad2Deg;
        //Debug.Log(fSensitivity * Time.deltaTime); 
        goBody.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.LerpAngle(fCurRotation, fDesiredRotation, fSensitivity ));
    }

    bool NearGround() {

        foreach (GameObject go in GetComponent<Player>().lstGroundColliderCheckers) {

            int nLayerPlatforms = 1 << LayerMask.NameToLayer("Platforms");
            Debug.DrawRay(go.transform.position, Vector2.down * fNearGround, Color.green);
            if (Physics2D.Raycast(go.transform.position, Vector2.down, fNearGround, nLayerPlatforms)) {
                //Debug.Log("Found a collision with collider at position " + go.transform.position);
                Debug.DrawRay(go.transform.position, Vector2.down * fNearGround, Color.red);
                return true;
            }
        }

        return false;
    }

    Vector2 GetRotationDashing() {
        fSensitivity = fDashSensitivity;

        switch (GetComponent<ContDashing>().curDashDirection) {
            case ContDashing.DashDirection.LEFT:
                return Vector2.left;

            case ContDashing.DashDirection.RIGHT:
                return Vector2.right;
    
        }
        return Vector2.down;
    }

    float GetVelocityRotationConstant(float fVel) {

        //Debug.Log("SPEED: " + fVel);
        if (fVel < fLowVelocity) {
            return 0.05f;
        } else if (fVel > fMaxVelocity) {
            return 1;
        } else { 
            float fTempRotVel = (fVel - fLowVelocity) / (fMaxVelocity - fLowVelocity);
            return (fTempRotVel * fTempRotVel) + 0.05f;
        }
    }

    Vector2 GetRotationVelocity() {
        fSensitivity = fAirSensitivity * Time.deltaTime * GetVelocityRotationConstant(GetComponent<Rigidbody2D>().velocity.magnitude);
        //Debug.Log("Const is " + fSensitivity);

        if (GetComponent<Rigidbody2D>().velocity.magnitude >= fLowVelocity) {

            return GetComponent<Rigidbody2D>().velocity;
        }else if(GetComponent<Rigidbody2D>().velocity.magnitude >= fMinVelocity){
            //Debug.Log("SLOW!");
            return Vector2.up + GetComponent<Rigidbody2D>().velocity.normalized;
        } else {
            //Debug.Log("STOP!");
            return Vector2.up + GetComponent<Rigidbody2D>().velocity.normalized;
        }
        
    }

    Vector2 GetRotationGround() {
        fSensitivity = fGroundSensitivity * Time.deltaTime;

        return Vector2.up;
    }
}
