using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using LitJson;
/*
 * 꽃게는 마법 볼을 생성한다.
 */

public class MonsterSkillControl : MonoBehaviour
{
    [SerializeField] private GameObject magicBallPrefab;
    [SerializeField] private int ballAmount = 5;
    [SerializeField] private float defaultBallPopForce = 150.0f;
    [SerializeField] private float screamRad = 30.0f;
    [SerializeField] private LayerMask enemyLayer;
    
    private readonly float ballChasedeFaultLerp = 0.1f;
    private readonly float offsetY = 1.0f;
    private float ballChasingLerp;
    private float ballPopForce;

    private Transform target;
    private MonsterWeaponTrigger[] magicBalls;
    private MonsterController monster;

    private Coroutine screamCash;
    private Coroutine ballActCash;
    private Coroutine ballFollowCash;

    private void Start()
    {
        monster = GetComponent<MonsterController>();

        switch (monster.monsterSO.MonsterType)
        {
            case EMonsterType.Crab:
                monster.OnSkill += StartFireBall;
                AssignBalls();
                break;

            case EMonsterType.Head:
                monster.OnSkill += StartScream;
                break;
        }




        monster.OnPlayerFound += SetTarget;
    }

    #region book head Skill
    private void StartScream()
    {
        if(screamCash != null) StopCoroutine(screamCash);
        screamCash = StartCoroutine(Scream_co());
    }
    /// <summary>
    /// book head 스킬 코루틴 시간동안 발동
    /// </summary>
    /// <returns></returns>
    private IEnumerator Scream_co()
    {
        int screamTime = 2;
        WaitForSeconds screamWFS = new WaitForSeconds(1.0f);

        while (screamTime > 0)
        {
            screamTime--;

            Collider[] _monsters = Physics.OverlapSphere(transform.position, screamRad, enemyLayer);
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, screamRad);
            if (_monsters.Length > 0)
            {
                for (int i = 0; i < _monsters.Length; i++)
                {
                    MonsterController _calledMonster = _monsters[i].GetComponent<MonsterController>();
                    _calledMonster.isFoundPlayer = true;
                    _calledMonster.player = monster.player;
                }
            }

            yield return screamWFS;
        }
    }
    #endregion


    #region 몬스터 볼
    /// <summary>
    /// 몬스터 볼 초기 생성
    /// </summary>
    public void AssignBalls()
    {
        magicBalls = new MonsterWeaponTrigger[ballAmount];

        for (int i = 0; i < ballAmount; i++)
        {
            magicBalls[i] = Instantiate(magicBallPrefab).GetComponent<MonsterWeaponTrigger>();
            magicBalls[i].UpdateDamage(monster.monsterSO.MonsterSkill.Damage);
            magicBalls[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 타겟 업데이트
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    /// <summary>
    /// 몬스터 볼 발사 코루틴으로 실행
    /// </summary>
    public void StartFireBall()
    {
        if (ballActCash != null)
        {
            TurnOffBalls();
            StopCoroutine(ballActCash);
        }

        ballChasingLerp = ballChasedeFaultLerp;
        ballPopForce = defaultBallPopForce;

        ballActCash = StartCoroutine(ActBalls_co());
    }

    /// <summary>
    /// 코루틴으로 입력한 볼의 개수 많큼 오른쪽부터 왼쪽까지 방향벡터를 더하여 볼을 활성화
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActBalls_co()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.2f);

        for (int i = 0; i < ballAmount; i++)
        {
            magicBalls[i].transform.position = transform.position;
            magicBalls[i].transform.forward = monster.transform.forward;
            magicBalls[i].gameObject.SetActive(true);

            float currentXVector = -1.0f + (2 / (float)ballAmount) * i;
            ballPopForce += defaultBallPopForce / (i + 1);
            magicBalls[i].rb.velocity = Vector3.zero;
            magicBalls[i].rb.AddForce( (new Vector3(currentXVector, 1.0f, 0.0f)) * ballPopForce, ForceMode.Force);
            yield return wfs;
        }

        yield return new WaitForSeconds(1.5f);

        if (ballFollowCash != null)
        {
            StopCoroutine(ballFollowCash);
        }
        Vector3 targetOffset = target.position;
        targetOffset.y += offsetY;
        ballFollowCash = StartCoroutine(ChasingTarget_co(targetOffset));
    }

    /// <summary>
    /// 타겟으로 코루틴에서 Lerp를 이용하여 날아감
    /// </summary>
    /// <param name="lastPos"> 마지막 포지션을 기준으로 날아간다. </param>
    /// <returns></returns>
    private IEnumerator ChasingTarget_co(Vector3 lastPos)
    {
        while (true)
        {
            foreach (var ball in magicBalls)
            {
                if (Vector3.Distance(ball.transform.position, lastPos) <= 0.1f)
                {
                    TurnOffBalls();
                    yield break;
                }
                ballChasingLerp += Time.deltaTime * ballChasedeFaultLerp;
                ball.transform.position = Vector3.Lerp(ball.rb.position, lastPos, ballChasingLerp);
            }
            yield return null;
        }
    }

    /// <summary>
    /// 공격이 모두 끝나면 몬스터 볼 회수
    /// </summary>
    private void TurnOffBalls()
    {
        for (int i = 0; i < ballAmount; i++)
        {
            magicBalls[i].gameObject.SetActive(false);
        }
    }
    #endregion

    private void OnDestroy()
    {
        monster.OnSkill -= AssignBalls;
        monster.OnSkill -= StartScream;
        monster.OnPlayerFound -= SetTarget;
    }
}