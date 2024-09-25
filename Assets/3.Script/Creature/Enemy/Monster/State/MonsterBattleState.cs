using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MonsterBattleState : MonsterBaseState
{
    public MonsterBattleState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
        switch (monsterSO.MonsterType)
        {
            case EMonsterType.None:
                Debug.LogWarning("�Ҵ� �ȵ� �ִϸ��̼�");
                break;
            case EMonsterType.Minotaur:
                aniClips = aniDB.minoAttackClips;
                break;
            case EMonsterType.Wizard:
                Debug.LogWarning(monsterSO.MonsterType + "�� �ִϸ��̼� �߰� ���!");
                break;
            case EMonsterType.Oak:
                Debug.LogWarning(monsterSO.MonsterType + "�� �ִϸ��̼� �߰� ���!");
                break;
            case EMonsterType.Skelleton:
                Debug.LogWarning(monsterSO.MonsterType + "�� �ִϸ��̼� �߰� ���!");
                break;
        }

        OnBehave = new UnityEvent();
    }

    /*
     * �÷��̾ ����޽� ������ �����ٸ� ���� ����
     * �������� ������ ���ϱ� ���� �� ������ ���� ���� ���ٰ� ������.
     * 
     * ���� ���� ���� ���Ѱ�
     */

    private enum Behaves
    {
        Right,
        Left,
        Attack,
        Length
    };

    private bool isBehave;
    private int attackIndex;
    private int[] aniClips;
    private UnityEvent OnBehave;
    private Coroutine behaveCoCash;

    public override void Enter()
    {
        base.Enter();
        RandomAttackState();
        isBehave = false;
        monster.navAI.ResetPath();
        OnBehave?.RemoveAllListeners();
        monster.animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.Run], 0.2f);
        monster.navAI.SetDestination(monster.player.position);
        monster.navAI.speed = monster.monsterSO.RunSpeed;
    }

    public override void Update()
    {
        CancelCondition();
        if (!monster.isFoundPlayer) return;
        base.Update();
        ChaseTarget();
    }

    public override void Exit()
    {
        base.Exit();
        if (behaveCoCash != null)
            monster.StopCoroutine(behaveCoCash);
        isBehave = false;
        monster.navAI.isStopped = true;
        monster.navAI.ResetPath();
    }
    
    private void ChaseTarget()
    {
        if (monster.navAI.remainingDistance <= monster.navAI.stoppingDistance)
        {
            if (!isBehave)
            {
                if (behaveCoCash != null) 
                    monster.StopCoroutine(behaveCoCash);
                
                behaveCoCash = monster.StartCoroutine(RandomBehave_co());
            }
            
            OnBehave?.Invoke();
            monster.navAI.SetDestination(monster.player.position);
        }

        else if(!monster.attackTimer.isTickin &&
            monster.navAI.remainingDistance > monster.navAI.stoppingDistance)
        {
            if (isBehave)
            {
                if (behaveCoCash != null)
                    monster.StopCoroutine(behaveCoCash);
                monster.animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.Run], 0.2f);
                isBehave = false;
            }
            monster.navAI.SetDestination(monster.player.position);
        }
    }

    private IEnumerator RandomBehave_co()
    {
        WaitForSeconds wfs = new WaitForSeconds(5f);
        isBehave = true;

        while (true)
        {
            OnBehave?.RemoveAllListeners();
            if (!monster.isFoundPlayer) yield break;

            switch (Random.Range((int)Behaves.Right, (int)Behaves.Length))
            {
                case (int)Behaves.Right:
                    Debug.Log("������");
                    monster.animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.WalkRight], 0.2f);
                    OnBehave.AddListener(() =>
                    {
                        Debug.DrawRay(monster.transform.position, monster.transform.right);
                        monster.transform.RotateAround(monster.player.position,
                                               Vector3.down, monsterSO.RotateAroundSpeed * Time.deltaTime);
                        monster.transform.LookAt(new Vector3(monster.player.position.x,
                                                             monster.transform.position.y,
                                                             monster.player.position.z));
                    });
                    break;

                    case (int)Behaves.Left:
                    Debug.Log("����");
                    monster.animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.WalkLeft], 0.2f);
                    OnBehave.AddListener(() =>
                    {
                        Debug.DrawRay(monster.transform.position, -monster.transform.right);
                        monster.transform.RotateAround(monster.player.position,
                                               Vector3.up, monsterSO.RotateAroundSpeed * Time.deltaTime);
                        monster.transform.LookAt(new Vector3(monster.player.position.x,
                                                             monster.transform.position.y,
                                                             monster.player.position.z));
                    });
                    break;

                case (int)Behaves.Attack:
                    monster.attackTimer.UpdateMaxTime(monsterSO.MonsterSkills[attackIndex].AttackAniTime);
                    monster.animator.CrossFade(aniClips[attackIndex], 0.1f);
                    wfs = new WaitForSeconds(monsterSO.MonsterSkills[attackIndex].AttackAniTime);
                    monster.navAI.isStopped = true;
                    monster.attackTimer.StartTimer(() =>
                    {
                        monster.navAI.isStopped = false;
                        monster.animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.Idle], 0.1f);
                        RandomAttackState();
                    });
                    break;
            }

            yield return wfs;
        }
    }

    private void RandomAttackState()
    {
        attackIndex = Random.Range(0, monster.monsterSO.MonsterSkills.Length);
        monster.navAI.stoppingDistance = 
            monsterSO.MonsterSkills[attackIndex].StoppingDistance;

        Debug.Log("���� ���� �ε��� :" + attackIndex);
    }

    private void CancelCondition()
    {
        if (monster.navAI.pathStatus == NavMeshPathStatus.PathPartial)
        {
            monster.isFoundPlayer = false;
        }
    }
}
