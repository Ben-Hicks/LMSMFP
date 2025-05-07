using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContJumping : MonoBehaviour {

    [Header("Configurable")]
    public float fJumpCooldown;
    public float fCooldownBetweenJumps;
    public float fMinJumpTime;
    public float fMaxJumpTime;
    public float fJumpVelocity;
    public float fFloatVelocity;
    public bool bWallJumpToward;

    [Space]
    public float fMinAirborneJumpTime;
    public float fMaxAirborneJumpTime;
    public float fAirborneJumpVelocity;
    public float fAirborneFloatVelocity;


    [Header("Properties")]
    public float fCurJumpTime;

    StateMachine<StJump> stmachJump;

    public Player plyrOwner;

    public Cooldown cooldown;

    // Start is called before the first frame update
    void Start(){

        plyrOwner = GetComponent<Player>();

        cooldown = new Cooldown();

        stmachJump = new StateMachine<StJump>(new StJumpReady(this));
    }

    public void HandleJumpInput() {

        //Let the current state decide what to do
        stmachJump.stateCur.HandleJumpInput();
    }

    public void FixedUpdate() {
        stmachJump.stateCur.PseudoFixedUpdate();
    }

    public void OnDestroy() {
        GeneralManager.Get().subUpdate.UnSubscribe(cooldown.cbUpdate);
    }
}
