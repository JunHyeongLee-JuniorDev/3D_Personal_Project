using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// StateMachine : state�� ��� ���·� ����, �� ���� ��ųʸ��� key(IState), value(StateNode)�� ����
/// </summary>
public abstract class StateMachine
{
    protected float crossFadeDuration = 0.1f;

    protected StateNode current;
    protected Dictionary<Type, StateNode> nodes = new Dictionary<Type, StateNode>();
    protected HashSet<ITransition> anyTransitions = new HashSet<ITransition>(); 

    /// <summary>
    /// �� ��� ���� ���� 
    /// </summary>
    protected class StateNode
    {
        public IState state;
        public HashSet<ITransition> transitions;

        public StateNode(IState state)
        {
            this.state = state;
            transitions = new HashSet<ITransition>();
        }

        public void AddTransition(IState to, IPredicate condition)
        {
            transitions.Add(new Transition(to, condition));
        }
    }

    public void Update()
    {
        var transition = GetTransition();

        if (transition != null)
            ChangeState(transition.To);

        current.state?.Update();
    }

    public void PhysicsUpdate()
    {
        current.state?.PhysicsUpdate();
    }

    public void SetState(IState state)
    {
        current = nodes[state.GetType()];
        current.state?.Enter();
    }

    public void ChangeState(IState state)
    {
        if (current.state.Equals(state)) return;

        var previousState = current.state;
        var nextState = state;

        previousState?.Exit();
        nextState?.Enter();

        current = nodes[state.GetType()];
        Debug.Log("���� State :" + current.state);

        var _state = state as PlayerBaseState;
        Debug.Log("���� �� : " + _state.stateMachine.player.isBattle);
        Debug.Log("�� �� : " + _state.stateMachine.player.isGrouded);
        Debug.Log("���� �� : " + _state.stateMachine.player.isJump);
        Debug.Log("������Ʈ �� : " + _state.stateMachine.player.isSprint);
    }

    public ITransition GetTransition()
    {
        foreach(var transition in anyTransitions)
            if (transition.Condition.Evaluate())
                return transition;

        foreach(var transition in current.transitions)
            if (transition.Condition.Evaluate())
                return transition;

        Debug.Log("�´� transition�� �����ϴ�");
        return null;
    }

    public void AddTransition(IState from, IState to, IPredicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).state, condition);
    }

    /// <summary>
    /// State�� �̿� ���� ���� ���
    /// </summary>
    public void AddAnyTransition(IState to, IPredicate condition)
    {
        anyTransitions.Add(new Transition(GetOrAddNode(to).state, condition));
    }

    protected StateNode GetOrAddNode(IState state)
    {
        var node = nodes.GetValueOrDefault(state.GetType());

        if (node == null)
        {
            node = new StateNode(state);
            nodes.Add(state.GetType(), node);   
        }

        return node;
    }
}
public interface IState
{
    public void Enter();
    public void Exit();
    public void Update();
    public void PhysicsUpdate();
    public void OnHurt();
}