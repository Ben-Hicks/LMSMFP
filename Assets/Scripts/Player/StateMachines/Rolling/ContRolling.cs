using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContRolling : MonoBehaviour {

    [Header("Configurable")]
    public float fStartRollingThreshold = 300f;
    public float fStopRollingThreshold = 300f;
    public float fRotateSpeed;

    [Header("Properties")]
    public StateMachine<StRoll> stmachRoll;
    public Player plyrOwner;

    public void Start() {
        plyrOwner = GetComponent<Player>();

        stmachRoll = new StateMachine<StRoll>(new StRollSteady(this));
    }

    public void FixedUpdate() {
        stmachRoll.stateCur.PseudoFixedUpdate();
    }

    public virtual void HandleRollingInput() {
        stmachRoll.stateCur.HandleRollInput();
    }



}
