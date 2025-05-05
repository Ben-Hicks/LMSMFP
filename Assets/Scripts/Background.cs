using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    public SpriteRenderer sprren;

    void Awake() {
        sprren.receiveShadows = true;
        Debug.Log("Receive shadows set to " + sprren.receiveShadows);
    }
}
