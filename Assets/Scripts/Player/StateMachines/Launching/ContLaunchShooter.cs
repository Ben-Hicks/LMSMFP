using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContLaunchShooter : MonoBehaviour{

    [Header("Configurable")]
    public float fWebShotSpeed = 1f;
    public float fLaunchVelocity = 10f;
    public float fLaunchForce = 1200f;
    public float fMaxWebShotLength = 5f;
    public float fMaxWebLength = 5f;
    public float fMinWebLength = 1f;
    [Space]
    public float fLaunchCooldown;
    public float fCooldownBetweenShots;

    [Header("Prefab References")]
    public GameObject pfLaunchingWeb;
    public GameObject pfWeight;

    [Header("Properties")]
    StateMachine<StLaunch> stmachLaunch;
    public GameObject goCurLaunchWeb;
    public Player plyrOwner;
    public Cooldown cooldown;

    public void Start() {
        plyrOwner = GetComponent<Player>();

        cooldown = new Cooldown();

        stmachLaunch = new StateMachine<StLaunch>(new StLaunchReady(this));
    }

    public void HandleLaunchInput() {

        //Let the current state decide what to do
        stmachLaunch.stateCur.HandleLaunchInput();
    }

    public void FixedUpdate() {
        stmachLaunch.stateCur.PseudoFixedUpdate();
        //Debug.LogFormat("Current velocity is {0}", GetComponent<Rigidbody2D>().velocity);
    }

    public void Detach() {
        stmachLaunch.stateCur.Detach();
    }

    public void OnDestroy() {
        if (GeneralManager.Get() != null) {
            GeneralManager.Get().subUpdate.UnSubscribe(cooldown.cbUpdate);
        }
    }

}
