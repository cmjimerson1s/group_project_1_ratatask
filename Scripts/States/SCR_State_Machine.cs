using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [HideInInspector] public State currentState;
    [SerializeField] private List<State> myStates = new();


    protected void StartStateMachine()
    {
        foreach (State state in myStates)
        {
            state.myStateMachine = this;
        }
    }


    protected void UpdateStateMachine()
    {
        currentState?.UpdateState();
    }


    public void SwitchState<T>() where T : State
    {
        foreach (State state in myStates)
        {
            if(state is T)
            {
                currentState?.ExitState();
                currentState = state;
                currentState?.EnterState();
                return;
            }
        }
    }
}
