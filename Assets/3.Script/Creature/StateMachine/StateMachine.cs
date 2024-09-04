using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// StateMachine : state는 노드 형태로 보관, 각 노드는 딕셔너리에 key(IState), value(StateNode)로 보관
/// </summary>
public abstract class StateMachine
{
    protected float crossFadeDuration = 0.1f;

    protected StateNode current;
    protected Dictionary<Type, StateNode> nodes = new Dictionary<Type, StateNode>();
    protected HashSet<ITransition> anyTransitions = new HashSet<ITransition>(); 

    /// <summary>
    /// 각 노드 별로 상태 
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
        Debug.Log("현재 State :" + current.state);

        var _state = state as PlayerBaseState;
        Debug.Log("전투 중 : " + _state.stateMachine.player.isBattle);
        Debug.Log("땅 중 : " + _state.stateMachine.player.isGrouded);
        Debug.Log("점프 중 : " + _state.stateMachine.player.isJump);
        Debug.Log("스프린트 중 : " + _state.stateMachine.player.isSprint);
    }

    public ITransition GetTransition()
    {
        foreach(var transition in anyTransitions)
            if (transition.Condition.Evaluate())
                return transition;

        foreach(var transition in current.transitions)
            if (transition.Condition.Evaluate())
                return transition;

        Debug.Log("맞는 transition이 없습니다");
        return null;
    }

    public void AddTransition(IState from, IState to, IPredicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).state, condition);
    }

    /// <summary>
    /// State와 이에 따른 조건 등록
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