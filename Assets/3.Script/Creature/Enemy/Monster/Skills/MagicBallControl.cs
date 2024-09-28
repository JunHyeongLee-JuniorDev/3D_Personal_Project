using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/*
 * 꽃게는 마법 볼을 생성한다.
 */

public class MagicBallControl : MonoBehaviour
{
    [SerializeField] private GameObject magicBallPrefab;
    [SerializeField] private int ballAmount = 5;
    [SerializeField] private float defaultBallPopForce = 150.0f;
    
    private readonly float ballChasedeFaultLerp = 0.1f;
    private float ballChasingLerp;
    private float ballPopForce;
    private readonly float offsetY = 1.0f;

    private Transform target;
    private MonsterWeaponTrigger[] magicBalls;
    private MonsterController monster;

    private Coroutine ballActCash;
    private Coroutine ballFollowCash;

    private void Start()
    {
        monster = GetComponent<MonsterController>();
        if (monster.monsterSO.MonsterType == EMonsterType.Crab)
        {
            monster.OnmagicBall -= StartSkill;
            monster.OnPlayerFound -= SetTarget;

            monster.OnmagicBall += StartSkill;
            monster.OnPlayerFound += SetTarget;
        }

        AssignBalls();
    }

    public void AssignBalls()
    {
        magicBalls = new MonsterWeaponTrigger[ballAmount];

        for (int i = 0; i < ballAmount; i++)
        {
            magicBalls[i] = Instantiate(magicBallPrefab).GetComponent<MonsterWeaponTrigger>();
            magicBalls[i].gameObject.SetActive(false);
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void StartSkill()
    {
        if (ballActCash != null)
        {
            TurnOffBalls();
            StopCoroutine(ballActCash);
        }

        ballChasingLerp = ballChasedeFaultLerp;
        ballPopForce = defaultBallPopForce;

        ballActCash = StartCoroutine(ActBalls());
    }

    private IEnumerator ActBalls()
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

        ballFollowCash = StartCoroutine(ChasingTarget(targetOffset));
    }

    private IEnumerator ChasingTarget(Vector3 lastPos)
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

    private void TurnOffBalls()
    {
        for (int i = 0; i < ballAmount; i++)
        {
            magicBalls[i].gameObject.SetActive(false);
        }
    }
}