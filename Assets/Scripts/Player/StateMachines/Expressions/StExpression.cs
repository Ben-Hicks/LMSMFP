using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StExpression : State {

    public ContExpressions contExpressions;

    public enum DirectionFacing { NEUTRAL, LEFT, UP, RIGHT, DOWN };
    public DirectionFacing curDirectionFacing;

    public float fIdleTime;
    public float fTimeInState;

    public StExpression(ContExpressions _contExpressions) {
        contExpressions = _contExpressions;
    }

    public override void OnEnter() {

        if (LevelType.Get() != null) {
            LevelType.Get().subCollected.Subscribe(cbAteFly);
        }

        fIdleTime = 0f;
        fTimeInState = 0f;

        //Initialize our facing direction
        UpdateDirectionFacing();

        //Initialize our sprite
        DecideSprite();
    }

    public override void OnLeave() {

        if (LevelType.Get() != null) {
            LevelType.Get().subCollected.UnSubscribe(cbAteFly);
        }
    }

    public override void PseudoFixedUpdate() {

        if (contExpressions.GetComponent<Rigidbody2D>().velocity.magnitude < 0.1) {
            fIdleTime += Time.fixedDeltaTime;
        } else {
            fIdleTime = 0f;
        }

        fTimeInState += Time.fixedDeltaTime;

        UpdateDirectionFacing();

        UpdateTransitions();
       
    }

    public virtual void UpdateTransitions() {

        if (CheckBored()) {
            //Currently, the bored -> sleep transition is just a sleep transition
            Transition(new StExpressionSleep(contExpressions));
        }else if (CheckNearFly()) {
            Transition(new StExpressionNearFly(contExpressions));
        } else if (CheckNearHazard()) {
            Transition(new StExpressionFear(contExpressions));
        } else if (CheckGoingFast()) {
            Transition(new StExpressionFast(contExpressions));
        } else if (CanTransToNeutral()) {
            Transition(new StExpressionNeutral(contExpressions));
        }
        //If none of the above happen, then we don't need to transition
    }

    public void cbAteFly(Object target, params object[] args) {
        Transition(new StExpressionChewing(contExpressions));
    }

    public void OnDeath() {
        Transition(new StExpressionDead(contExpressions));
    }

    public void UpdateDirectionFacing() {

        ContInput contInput = contExpressions.GetComponent<ContInput>();
        DirectionFacing newDirectionFacing;

        if(contInput.bMoveUp && contInput.bMoveDown == false) {
            newDirectionFacing = DirectionFacing.UP;
        }else if(contInput.bMoveUp == false && contInput.bMoveDown) {
            newDirectionFacing = DirectionFacing.DOWN;
        } else if (contInput.bMoveLeft && contInput.bMoveRight == false) {
            newDirectionFacing = DirectionFacing.LEFT;
        } else if (contInput.bMoveLeft == false && contInput.bMoveRight) {
            newDirectionFacing = DirectionFacing.RIGHT;
        } else {
            newDirectionFacing = DirectionFacing.NEUTRAL;
        }

        //Check if the direction has changed (possibly warranting an updated sprite)
        if(newDirectionFacing != curDirectionFacing) {
            curDirectionFacing = newDirectionFacing;
            DecideSprite();
        }

    }

    public virtual bool CheckBored() {
        return fIdleTime >= contExpressions.fBoredDelay;
    }

    public virtual bool CheckNearFly() {

        int nCollectablesLayer = 1 << LayerMask.NameToLayer("Collectable");

        if(Physics2D.OverlapCircle(contExpressions.transform.position, contExpressions.fNearFlyThreshold, nCollectablesLayer)) {
            return true;
        }

        return false;
    }

    public virtual bool CheckNearHazard() {

        int nHazardLayer = 1 << LayerMask.NameToLayer("Hazard");

        if (Physics2D.OverlapCircle(contExpressions.transform.position, contExpressions.fNearHazardThreshold, nHazardLayer)) {
            Debug.Log("Near a hazard!");
            return true;
        }

        return false;
    }

    public virtual bool CheckGoingFast() {

        if(contExpressions.GetComponent<Rigidbody2D>().velocity.magnitude >= contExpressions.fMinFastThreshold) {
            return true;
        }

        return false;
    }

    public virtual bool CanTransToNeutral() {
        return true;
    }

    public abstract void DecideSprite();
}
