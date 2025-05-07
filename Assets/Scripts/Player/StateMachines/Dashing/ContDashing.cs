using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContDashing : MonoBehaviour {

    public enum DashDirection { NONE, LEFT, RIGHT};

    [Header("Configurable")]
    public int nMaxSideCharges = 1;
    public float fCooldownBetweenDashes;

    public float fDashTime;
    public float fDashDistance;

    public float fDashUpwardsNudge;
    public float fDashSideNudge;



    [Header("Properties")]
    public DashDirection curDashDirection;
    public float fDashSpeed;
    public float fCurDashTime;

    public Cooldown cooldown;

    public Player plyrOwner;
    public StateMachine<StDash> stmachDash;

    public void Start() {
        plyrOwner = GetComponent<Player>();

        cooldown = new Cooldown(nMaxSideCharges);

        stmachDash = new StateMachine<StDash>(new StDashReady(this));

        fDashSpeed = fDashDistance / fDashTime;
    }

    public void HandleDashInput() {

        stmachDash.stateCur.HandleDashInput();

    }

    public void FixedUpdate() {
        stmachDash.stateCur.PseudoFixedUpdate();
    }

}
