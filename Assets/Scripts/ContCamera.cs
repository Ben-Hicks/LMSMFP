using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContCamera : MonoBehaviour {

    Animator anim;
    Animator animator {
        get {
            if (anim == null) {
                anim = GetComponent<Animator>();
            }
            return anim;
        }
    }


    public void ScreenShake() {
        animator.SetTrigger("trigScreenShake");
    }

}
