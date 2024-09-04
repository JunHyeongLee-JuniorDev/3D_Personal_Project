using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Transition : ITransition
{
    public IState To { get; }

    public IPredicate Condition { get; }

    public Transition(IState to, IPredicate condition)
    {
        this.To = to;
        this.Condition = condition;
    }
}

public interface ITransition
{
    IState To { get; }
    IPredicate Condition { get; }
}
