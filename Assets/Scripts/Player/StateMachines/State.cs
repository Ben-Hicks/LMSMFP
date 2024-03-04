using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State{

    public Subject subTransitions;

    public State() {
        subTransitions = new Subject();
    }

    public virtual void OnEnter() {

    }

    public virtual void OnUpdate() {

    }

    public virtual void OnFixedUpdate() {

    }

    public void Transition(State newState) {
        subTransitions.NotifyObs(null, newState);
    }

    public virtual void OnLeave() {

    }

}
