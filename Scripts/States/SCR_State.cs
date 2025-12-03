using UnityEngine;

public class State : MonoBehaviour
{
    [HideInInspector] public StateMachine myStateMachine = null;
    

    public virtual void EnterState(){ }

    public virtual void UpdateState(){ }

    public virtual void ExitState(){ }
}
