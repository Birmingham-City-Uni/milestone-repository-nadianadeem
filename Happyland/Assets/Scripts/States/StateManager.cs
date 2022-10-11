using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    [Header("State Manager Settings")]
    public bool IsStackBased;
    public State currState { get; private set; }
    private Stack stack;

    //Stack based methods
    public State GetCurrStateOnStack()
    {
        return stack.Count > 0 ? (State)stack.Peek() : null;
    }

    public bool PopState()
    {
        if (stack.Count > 0)
        {
            GetCurrStateOnStack().Exit();
            stack.Pop();
            return true;
        }
        else return false;
    }

    public bool PushState(State _pushme)
    {
        if (stack.Peek() != _pushme)
        {
            stack.Push(_pushme);
            GetCurrStateOnStack().Enter();
            return true;
        }
        else return false;
    }

    //Non Stack methods

    public void ChangeState(State newState)
    {
        if (currState != null)
        {
            currState.Exit();
        }
        currState = newState;
        newState.Enter();
    }

    //General methods
    public void Init(State startState)
    {
        if (IsStackBased)
        {
            this.stack = new Stack();
            stack.Push(startState);
            startState.Enter();
        }        
    }

    // Update is called once per frame
    public void Update()
    {
        if(GetCurrStateOnStack() != null)
        {
            GetCurrStateOnStack().Execute();
        }
        
        if(currState != null)
        {
            currState.Execute();
        }
    }
}
