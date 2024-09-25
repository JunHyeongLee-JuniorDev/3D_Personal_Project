using System.Collections;
using UnityEngine;

public class MonsterLocoState : MonsterBaseState
{
    public MonsterLocoState(MonsterStateMachine stateMachine) : base(stateMachine) {}

    /*
    * 노드를 계속 따라가도록하자
    * 노드는 배열로 들고 있어 랜덤 값으로 index접근
    * 노드 까는 것은 인스펙터에서 직접할당하자
    */
    private float locoAniBlend;
    private bool isPush = true;
    private int nodeIndex = 0;
    private Coroutine waitCoCash;


    public override void Enter()
    {
        base.Enter();
        locoAniBlend = 0.0f;
        monster.navAI.speed = monster.monsterSO.PatrolSpeed;
        monster.navAI.stoppingDistance = 0.0f;
        monster.animator.CrossFade(monster.aniDataBase.monsterAniClips[EMonsterAni.LocoBlend], 0.25f);
    }

    public override void Update()
    {
        base.Update();
        if (!monster.isCanPatrol) return;
        Patrol();
        AnimationBlend();
    }

    public override void Exit()
    {
        if(waitCoCash != null)
            monster.StopCoroutine(waitCoCash);

        StartWalk();
    }


    private void AnimationBlend()
    {
        locoAniBlend = Mathf.Lerp(locoAniBlend, monster.navAI.speed, 0.01f);
        monster.animator.SetFloat(monster.aniDataBase.monsterParams[EMonsterAni.LocoBlend], locoAniBlend);
    }

    private void Patrol()
    {
        if (monster.navAI.remainingDistance < monster.PatrolStopDistance)
        {

            if (isBreakNode())
            {
                waitCoCash = monster.StartCoroutine(Patroldelay_co());
            }

            AssignDestination();
        }
    }

    private IEnumerator Patroldelay_co()
    {
        StopWalk();
        yield return new WaitForSeconds(3.0f);
        StartWalk();
    }

    private void StopWalk()
    {
        monster.navAI.isStopped = true;
        monster.navAI.speed = 0.0f;
    }

    private void StartWalk()
    {
        monster.navAI.isStopped = false;
        monster.navAI.speed = monster.monsterSO.PatrolSpeed;
    }

    private bool isBreakNode()
    {
        if (monster.nodeStack.Count == 0 ||
            nodeIndex > monster.nodeList.Count - 1)
        {
            nodeIndex = 0;
            isPush = monster.nodeStack.Count == 0;
            return true;
        }

        return false;
    }

    private void AssignDestination()
    {
        if (isPush)
        {
            monster.navAI.SetDestination(monster.nodeList[nodeIndex].position);
            monster.nodeStack.Push(monster.nodeList[nodeIndex++]);
        }

        else
        {
            monster.navAI.SetDestination(monster.nodeStack.Pop().position);
        }
    }
}
