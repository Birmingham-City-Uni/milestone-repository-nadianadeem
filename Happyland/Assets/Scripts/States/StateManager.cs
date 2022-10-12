using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable()]
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
            GetCurrStateOnStack().IsReEntering = true;
            GetCurrStateOnStack().currentReEnteringTime = GetCurrStateOnStack().ReEnteringTime;
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
        else
        {
            ChangeState(startState);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (GetCurrStateOnStack().IsReEntering)
        {
            GetCurrStateOnStack().ReEnteringTime -= Time.deltaTime;
            GetCurrStateOnStack().ReEnter();
        }
        else if (GetCurrStateOnStack() != null || ( GetCurrStateOnStack().currentReEnteringTime > 0 && !GetCurrStateOnStack().IsReEntering ))
        {
            GetCurrStateOnStack().currentReEnteringTime -= Time.deltaTime;
            GetCurrStateOnStack().Execute();
        }
        
        if(currState != null)
        {
            currState.Execute();
        }
    }
}
