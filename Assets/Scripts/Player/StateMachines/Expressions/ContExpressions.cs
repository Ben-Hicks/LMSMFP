using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContExpressions : MonoBehaviour {

    [Header("Configurable")]
    public SpriteRenderer spriteRendererExpression;

    public Sprite sprNeutral;
    public Sprite sprNeutralDown;
    public Sprite sprNeutralSide;
    public Sprite sprNeutralUp;

    public Sprite sprFast;
    public Sprite sprFastDown;
    public Sprite sprFastSide;
    public Sprite sprFastUp;
    public float fMinFastThreshold;

    public Sprite sprNearFly;
    public Sprite sprNearFlyDown;
    public Sprite sprNearFlySide;
    public Sprite sprNearFlyUp;
    public float fNearFlyThreshold;

    public Sprite sprChewing;
    public Sprite sprChewingDown;
    public Sprite sprChewingSide;
    public Sprite sprChewingUp;
    public float fTimeChewing;

    public Sprite sprHappy;
    public Sprite sprHappySide;
    public float fTimeHappy;

    public Sprite sprFear;
    public float fNearHazardThreshold;

    public Sprite sprBored;
    public float fBoredDelay;

    public Sprite sprSleep1;
    public Sprite sprSleep2;
    public float fSleepDelay;
    public float fSnoreTime;

    public Sprite sprDead;

    [Header("Properties")]
    public StateMachine<StExpression> stmachExpressions;

    public string sCurState;

    public Player plyrOwner;

    void Start() {
        plyrOwner = GetComponent<Player>();

        stmachExpressions = new StateMachine<StExpression>(new StExpressionNeutral(this));
 
    }

    private void FixedUpdate() {
        sCurState = stmachExpressions.stateCur.ToString();
        stmachExpressions.stateCur.OnFixedUpdate();
    }
}
