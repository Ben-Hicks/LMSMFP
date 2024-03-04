using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContSwingShooter : MonoBehaviour {

    [Header("Configurable")]
    public float fWebShotSpeed = 1f;
    public float fExtendSpeed = 1f;
    public float fRetractSpeed = 1f;
    public float fMaxWebShotLength = 5f;
    public float fMaxWebLength = 5f;
    public float fMinWebLength = 1f;
    [Space]
    public float fSwingCooldown;
    public float fCooldownBetweenShots;
    [Space]
    public float fElasticity = 26.0f;            //The force at which the web pulls back if extended too far
    public float fMaxElasticDistance = 0.5f;     //The maximum length the web can be extended by
    public float fTimeMaxElasticity = 1f;      //The maximum time the web will stay bouncy and elastic

    [Header("Prefab References")]
    public GameObject pfSwingingWeb;
    public GameObject pfWeight;

    [Header("Properties")]
    public StateMachine<StSwing> stmachSwing;
    public GameObject goCurSwingWeb;

    public Player plyrOwner;
    public Cooldown cooldown;

    public void Start() {
        plyrOwner = GetComponent<Player>();

        cooldown = new Cooldown();

        stmachSwing = new StateMachine<StSwing>(new StSwingReady(this));
    }

    public void HandleSwingInput() {

        //Let the current state decide what to do
        stmachSwing.stateCur.HandleSwingInput();
        
    }

    public void FixedUpdate() {
        stmachSwing.stateCur.OnFixedUpdate();
    }

    public void Detach() {
        stmachSwing.stateCur.Detach();
    }

    public void OnDestroy() {
        if (GeneralManager.Get() != null) {
            GeneralManager.Get().subUpdate.UnSubscribe(cooldown.cbUpdate);
        }
    }
}

