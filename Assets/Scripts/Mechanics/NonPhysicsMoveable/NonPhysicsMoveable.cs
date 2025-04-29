using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPhysicsMoveable : MonoBehaviour {


    public Vector3 v3MovingForce;


    public void SetMovingForce(Vector3 _v3MovingForce) {
        BeforeSettingMovingForce();
        v3MovingForce = _v3MovingForce;
        AfterSettingMovingForce();

    }

    public virtual void AfterSettingMovingForce() {
    
    }

    public virtual void BeforeSettingMovingForce() {

    }

    public virtual void FixedUpdate() {


    }
    
}
