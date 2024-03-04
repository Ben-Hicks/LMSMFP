using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralManager : SingletonPersistent<GeneralManager>{


    public Subject subUpdate;

    public static int nSavedTotalStarsEarned {
        get {
            return PlayerPrefs.GetInt("nTotalStarsEarned");
        }
        set {
            PlayerPrefs.SetInt("nTotalStarsEarned", value);
        }
    }
    public static Subject subTotalStarsChanged;

    public GeneralManager() {
        subUpdate = new Subject();
        
    }

    public override void Init() {
        Screen.fullScreen = true;

        subTotalStarsChanged = new Subject(Subject.SubType.ALL);

        Debug.Log("Currently setting webs to have no collision with the player");
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("ShootingWeb"));

        Debug.Log("Currently setting webs to have no collision with other webs");
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("ShootingWeb"), LayerMask.NameToLayer("ShootingWeb"));

    }

    public void FixedUpdate() {

        //Let non-MonoBehaviours know when an update call occurs
        subUpdate.NotifyObs();

    }

}
