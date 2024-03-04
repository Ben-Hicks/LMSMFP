using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {


    public void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(collision.gameObject.name + " touched " + this.gameObject.name + "'s hazard");

        collision.gameObject.GetComponent<Player>().OnHazardHit(this);
    }
}
