using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public bool bCollected;

    private void Despawn() {

        //Currently, just moving the object offscreen
        Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (bCollected == true) return; //Ignore collisions if we've already been connected

        //Let the Score Manager know to collect this
        LevelType.Get().Collected(this);

        //After collecting, despawn this collectable
        Despawn();
    }




}
