using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using LitJson;
/*
 * �ɰԴ� ���� ���� �����Ѵ�.
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
    /// book head ��ų �ڷ�ƾ �ð����� �ߵ�
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


    #region ���� ��
    /// <summary>
    /// ���� �� �ʱ� ����
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
    /// Ÿ�� ������Ʈ
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    /// <summary>
    /// ���� �� �߻� �ڷ�ƾ���� ����
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
    /// �ڷ�ƾ���� �Է��� ���� ���� ��ŭ �����ʺ��� ���ʱ��� ���⺤�͸� ���Ͽ� ���� Ȱ��ȭ
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
    /// Ÿ������ �ڷ�ƾ���� Lerp�� �̿��Ͽ� ���ư�
    /// </summary>
    /// <param name="lastPos"> ������ �������� �������� ���ư���. </param>
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
    /// ������ ��� ������ ���� �� ȸ��
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