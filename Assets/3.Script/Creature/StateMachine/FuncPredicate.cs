using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncPredicate : IPredicate

{
    readonly Func<bool> func;

    public FuncPredicate(Func<bool> func)
    {
        this.func = func;
    }

    private bool test;
    public bool Test => test;

    public bool Evaluate() => func.Invoke();
}

public interface IPredicate 
{
    bool Evaluate();
}