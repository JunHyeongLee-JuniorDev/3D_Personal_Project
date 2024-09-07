using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PlayerFOV : MonoBehaviour
{
    /*
     * 플레이어 시야각 구현하자 주변에 적이 있다면 targetting해서 battle State로 넘어가게끔
     */
    [SerializeField]
    private float radius;

    [SerializeField]
    private LayerMask enemyLayer;

    [SerializeField]
    private LayerMask wallLayer;

    [SerializeField]
    private LayerMask groundLayer;

    private Camera mainCam;


    public List<Collider> enemysInView = new List<Collider>();

    private void Start()
    {
        mainCam = Camera.main;
    }


    private void Update()
    {
        AddEnemyList();
    }

    private void AddEnemyList()
    {
        Collider[] enemys = Physics.OverlapSphere(transform.position, radius, enemyLayer, QueryTriggerInteraction.Ignore);
        if (enemys == null) return;
        enemysInView.Clear();

        foreach (Collider enemy in enemys)
        {
            Vector3 _enemyPos = enemy.transform.position;

            if(isInCam(_enemyPos))
            {
                Vector3 _direction = dirBetAB(mainCam.transform.position, _enemyPos);

                if (Physics.Raycast(mainCam.transform.position, _direction, out RaycastHit hitInfo,
                                Vector3.Distance(mainCam.transform.position, _enemyPos), wallLayer | groundLayer))
                    enemysInView.Add(enemy);
                    
                    Debug.DrawRay(mainCam.transform.position, _direction * Vector3.Distance(mainCam.transform.position, _enemyPos), Color.red);
            }
        }

        if(enemysInView.Count <0) return;
    }

    private void minimumDistance(List<Collider> colliders)
    {
        foreach (Collider collider in colliders)
        {
            
        }
    }

    private Vector3 dirBetAB(Vector3 origin, Vector3 target)
    {
        return (target - origin).normalized;
    }

    private bool isInCam(Vector3 position)
    {
        Vector3 viewPos = mainCam.WorldToViewportPoint(position);

        if (viewPos.x >= 0 && viewPos.x <= 1
         && viewPos.y >= 0 && viewPos.y <= 1
         && viewPos.z > 0)
            return true;

        else return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
