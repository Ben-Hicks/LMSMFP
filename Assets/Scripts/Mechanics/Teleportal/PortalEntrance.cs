using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEntrance : MonoBehaviour {

    public Teleportal teleportalOwner;

    public void OnTriggerEnter2D(Collider2D collision) {

        Debug.Log("Detected a collision with " + collision.name);

        teleportalOwner.OnPortalEnter(this, collision.gameObject);

    }
}
