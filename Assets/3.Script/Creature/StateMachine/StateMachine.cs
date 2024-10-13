using System.Collections.Generic;
using System;

/// <summary>
/// StateMachine : state는 노드 형태로 보관, 각 노드는 딕셔너리에 key(IState), value(StateNode)로 보관
/// </summary>
public abstract class StateMachine
{
    protected IState current;
    public HashSet<ITransition> anyTransitions { get; protected set; } = new HashSet<ITransition>(); 

    public void Update()
    {
        var transition = GetTransition();

        if (transition != null && !current.Equals(transition.To))
            ChangeState(transition.To);

        current?.Update();
    }

    public void PhysicsUpdate()
    {
        current?.PhysicsUpdate();
    }

    public void LateUpdate()
    {
        current?.LateUpdate();
    }

    public void SetState(IState state)
    {
        current = state;
        current?.Enter();
    }

    public void OnHurt()
    {
        current?.OnHurt();
    }

    public void Clear()
    {
        current?.Clear();
    }

    public void ChangeState(IState state)
    {
        var previousState = current;
        var nextState = state;

        previousState?.Exit();
        nextState?.Enter();

        current = nextState;
    }

    public ITransition GetTransition()
    {
        foreach (var transition in anyTransitions)
            if (transition.Condition.Evaluate())
                return transition;

        return null;
    }


    /// <summary>
    /// State와 이에 따른 조건 등록
    /// </summary>
    public void AddAnyTransition(IState to, IPredicate condition)
    {
        anyTransitions.Add(new Transition(to, condition));
    }

}
public interface IState
{
    public void Enter();
    public void Exit();
    public void Update();
    public void LateUpdate();
    public void PhysicsUpdate();
    public void OnHurt();
    public void Clear();
}