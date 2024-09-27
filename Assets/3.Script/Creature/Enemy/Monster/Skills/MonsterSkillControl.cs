using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/*
 * 꽃게는 마법 볼을 생성한다.
 */

public class MagicBallControl : MonoBehaviour
{
    [SerializeField] private GameObject magicBallPrefab;
    private Transform target;
    private GameObject[] magicBalls;

    public void AssignBalls(int ballAmount)
    {
        magicBalls = new GameObject[ballAmount];

        for (int i = 0; i < ballAmount; i++)
        {
            magicBalls[i] = Instantiate(magicBallPrefab);
            magicBalls[i].transform.SetParent(transform);
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void StartSkill()
    {

    }


    private IEnumerator ChasingTarget(Vector3 lastPos)
    {
        while (true)
        {
            foreach (var ball in magicBalls)
            {
                if(ball.transform.position == lastPos) yield break;
            }

            yield return null;
        }
    }
}