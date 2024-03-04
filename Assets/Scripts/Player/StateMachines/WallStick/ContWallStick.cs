using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContWallStick : MonoBehaviour {

    public float fMinJumpTime;
    public float fWallJumpShoveX;

    public Player plyrOwner;
    public StateMachine<StWallStick> stmachWallStick;


    public void Start() {
        plyrOwner = GetComponent<Player>();

        stmachWallStick = new StateMachine<StWallStick>(new StWallStickNeutral(this));

    }

    public void HandleWallStick() {

        stmachWallStick.stateCur.OnUpdate();

    }

}
