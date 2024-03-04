using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : State {

    public T stateCur;

    public StateMachine(T stateStart) {

        Transition(stateStart);

    }

    public void Transition(T stateNew) {

        Debug.Assert(stateNew != null);

        if (stateCur != null) {
            stateCur.subTransitions.UnSubscribe(cbTransition);
            stateCur.OnLeave();
        }

        stateCur = stateNew;

        stateCur.subTransitions.Subscribe(cbTransition);
        stateCur.OnEnter();

    }

    //Allow the states to handle transitions by notifying the statemachine if they want to make a transition
    public void cbTransition(Object target, params object[] args) {
        Transition((T)args[0]);
    }
}
