using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLocoState : MonsterBaseState
{
    public MonsterLocoState(MonsterStateMachine stateMachine) : base(stateMachine) 
    {
        nodeStack = stateMachine.monster.nodeStack;
        nodeList = stateMachine.monster.nodeList;
    }

    /*
    * 노드를 계속 따라가도록하자
    * 노드는 배열로 들고 있어 랜덤 값으로 index접근
    * 노드 까는 것은 인스펙터에서 직접할당하자
    */
    private float locoAniBlend;
    private bool isPush = true;
    private int nodeIndex = 0;
    private Coroutine waitCoCash;
    private Stack<Transform> nodeStack;
    private List<Transform> nodeList;

    public override void Enter()
    {
        base.Enter();

        if(!monster.outMapTimer.isTickin)
        monster.TurnOnNav();
        monster.hpSlider.gameObject.SetActive(false);
        isPush = true;
        navAI.autoBraking = false;
        nodeStack.Clear();
        nodeIndex = 0;
        locoAniBlend = 0.2f;
        navAI.stoppingDistance = monster.PatrolStopDistance;
        navAI.speed = monster.monsterSO.PatrolSpeed;
        animator.CrossFade(monster.aniDataBase.monsterAniClips[EMonsterAni.LocoBlend], 0.25f);
        AssignDestination();
    }

    public override void Update()
    {
        base.Update();
        Patrol();
        AnimationBlend();
    }

    public override void Exit()
    {
        navAI.autoBraking = false; 
        monster.hpSlider.gameObject.SetActive(true);

        if (waitCoCash != null)
            monster.StopCoroutine(waitCoCash);

        StartWalk();
    }


    private void AnimationBlend()
    {
        locoAniBlend = Mathf.Lerp(locoAniBlend, navAI.desiredVelocity.sqrMagnitude, 0.1f);
        animator.SetFloat(aniDB.monsterParams[EMonsterAni.LocoBlend], locoAniBlend);
    }

    private void Patrol()
    {
        if (HasReachedDestination() && monster.nodeList.Count != 1)
        {
            if (isBreakNode())
            {
                if (waitCoCash != null)
                    monster.StopCoroutine(waitCoCash);

                waitCoCash = monster.StartCoroutine(Patroldelay_co());
            }

            AssignDestination();
        }
    }

    private bool HasReachedDestination()
    {
        return !navAI.pathPending &&
            navAI.remainingDistance <= navAI.stoppingDistance;
    }

    private IEnumerator Patroldelay_co()
    {
        StopWalk();
        yield return new WaitForSeconds(3.0f);
        StartWalk();
    }

    private void StopWalk()
    {
        navAI.isStopped = true;
        navAI.speed = 0.0f;
    }

    private void StartWalk()
    {
        monster.navAI.isStopped = false;
        navAI.speed = monsterSO.PatrolSpeed;
    }

    private bool isBreakNode()
    {
        if (nodeStack.Count == 0 ||
            nodeIndex > nodeList.Count - 1)
        {
            nodeIndex = 0;
            isPush = nodeStack.Count == 0;
            return true;
        }

        return false;
    }

    private void AssignDestination()
    {
        if (isPush)
        {
            navAI.SetDestination(monster.nodeList[nodeIndex].position);
            nodeStack.Push(monster.nodeList[nodeIndex++]);
        }

        else
        {
            navAI.SetDestination(nodeStack.Pop().position);
        }
    }
}
