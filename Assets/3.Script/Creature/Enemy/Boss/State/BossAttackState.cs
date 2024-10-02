using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class BossAttackState : BossBaseState
{

    private Coroutine facingCash;

    public BossAttackState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Boss Attack State");
        base.Enter();
        ActivateSkill();
    }


    private void ActivateSkill()
    {
        int clipID = -1;
        float distance = Vector3.Distance(monster.transform.position, monster.player.position);
        if (facingCash != null) monster.StopCoroutine(facingCash);

        if (distance < 3.0f)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    clipID = aniDB.aniHashs[EPriestAni.InPlaceJumpAttack];
                    monster.TurnOffNav();
                    monster.attackTimer.UpdateMaxTime(3.1f);
                    monster.attackTimer.StartTimer(() =>
                    {
                        monster.TurnOnNav();
                        monster.isAttack = false;
                    });
                    break;

                case 1:
                    clipID = aniDB.aniHashs[EPriestAni.HeavyFootStep];
                    monster.attackTimer.UpdateMaxTime(1.9f);
                    monster.attackTimer.StartTimer(() =>
                    {
                        monster.isAttack = false;
                    });
                    break;
                case 2:
                    clipID = aniDB.aniHashs[EPriestAni.AttackRun];
                    monster.isChargeState = true;
                    monster.isAttack = false;
                    break;
            }
        }

        else
        {
            switch (Random.Range(0, 5))
            {
                case 0:
                    clipID = aniDB.aniHashs[EPriestAni.ForwardJumpAttack];
                    monster.attackTimer.UpdateMaxTime(3.7f);
                    monster.TurnOffNav();
                    monster.attackTimer.StartTimer(() =>
                    {
                        monster.TurnOnNav();
                        monster.isAttack = false;
                    });
                    break;

                case 1:
                    clipID = aniDB.aniHashs[EPriestAni.Combo1];
                    monster.attackTimer.UpdateMaxTime(3.6f);
                    monster.attackTimer.StartTimer(() =>
                    {
                        monster.isAttack = false;
                    });
                    break;

                case 2:
                    clipID = aniDB.aniHashs[EPriestAni.Combo2];
                    monster.attackTimer.UpdateMaxTime(4.2f);
                    monster.attackTimer.StartTimer(() =>
                    {
                        monster.isAttack = false;
                    });
                    break;

                case 3:
                    clipID = aniDB.aniHashs[EPriestAni.SoccerKick];
                    monster.attackTimer.UpdateMaxTime(1.3f);
                    monster.attackTimer.StartTimer(() =>
                    {
                        monster.isAttack = false;
                    });
                    break;

                case 4:
                    clipID = aniDB.aniHashs[EPriestAni.AttackRun];
                    monster.isChargeState = true;
                    monster.isAttack = false;
                    break;
            }

            if (facingCash != null) monster.StopCoroutine(facingCash);
            facingCash = monster.StartCoroutine(facingPlayer());
        }


        animator.CrossFade(clipID, 0.2f);
    }

    private IEnumerator facingPlayer()
    {
        Vector3 _direction = monster.player.position - monster.transform.position;

        while (Vector3.Angle(monster.transform.forward, _direction) > 1.0f)
        {
            _direction = monster.player.position - monster.transform.position;
            monster.transform.rotation = Quaternion.LookRotation(Vector3.Lerp(monster.transform.forward, _direction.normalized, 0.1f));
            yield return null;
        }
    }
}
