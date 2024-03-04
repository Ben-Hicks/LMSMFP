using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportal : MonoBehaviour {

    public const float fTimeBetweenTeleports = 0.05f;

    public PortalEntrance entrance1;
    public PortalEntrance entrance2;

    public Dictionary<GameObject, float> mapTeleportedTime;

    public Teleportal() {
        mapTeleportedTime = new Dictionary<GameObject, float>();
    }

    void Start() {
        entrance1.teleportalOwner = this;
        entrance2.teleportalOwner = this;
    }

    public PortalEntrance GetOtherEntrance(PortalEntrance entrance) {

        if(entrance == entrance1) {
            return entrance2;
        }else if(entrance == entrance2) {
            return entrance1;
        } else {
            Debug.LogError("Somehow we got passed an entrance that is neither of our registered endpoints");
            return null;
        }
    }

    public void Teleport(PortalEntrance from, GameObject goEntered) {

        PortalEntrance to = GetOtherEntrance(from);

       

        ContSwingShooter swingShoot = goEntered.GetComponent<ContSwingShooter>();
        if(swingShoot != null) {
            //If this object has a swing shooter, then dettach from the web (if it's attached to one)
            swingShoot.Detach();
        }
        ContLaunchShooter launchShoot = goEntered.GetComponent<ContLaunchShooter>();
        if (launchShoot != null) {
            //If this object has a launch shooter, then dettach from the web (if it's attached to one)
            launchShoot.Detach();
        }


        //Now we can move the game object directly to the other entrance (at the same relative offset that we are from the entrance)
        Vector3 v3Offset = goEntered.transform.position - from.transform.position;

        goEntered.GetComponent<Rigidbody2D>().MovePosition(to.transform.position - v3Offset);
    }

    public void OnPortalEnter(PortalEntrance entrance, GameObject goEntered) {

        //If we have teleported this gameobject before, and it was very recently, then we shouldn't teleport it again
        if (mapTeleportedTime.ContainsKey(goEntered) && Time.timeSinceLevelLoad - mapTeleportedTime[goEntered] < fTimeBetweenTeleports) {
            Debug.Log("Shouldn't teleport " + goEntered.name + " since we just teleported it");
        } else {
            //Otherwise, we are safe to teleport this gameobject

            //Save the time we performed this teleport at
            mapTeleportedTime[goEntered] = Time.timeSinceLevelLoad;
            Teleport(entrance, goEntered);
            
        } 
    }

}
