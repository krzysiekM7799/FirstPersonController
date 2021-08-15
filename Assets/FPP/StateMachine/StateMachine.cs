using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<T>
{
    T currrentState;
    T previousState;
    protected Selector root;
    public T CurrentState { get { return currrentState; } set { currrentState = value; } }
    public T PreviousState { get { return previousState; } set { previousState = value; } }

    public bool IsStateMachineStopped { get; set; }

    public abstract void MakeStateMachine();
    protected abstract void Evaluate();
    public  void EvaluateStateMachine()
    {
        if (!IsStateMachineStopped)
        {
            Evaluate();
            previousState = currrentState;
        }
    }
}
