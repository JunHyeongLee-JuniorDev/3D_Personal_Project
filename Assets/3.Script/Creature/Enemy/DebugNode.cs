using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugNode : MonoBehaviour
{
    [SerializeField] private Color nodeColor;

    private void OnDrawGizmos()
    {
        Gizmos.color = nodeColor;

        Gizmos.DrawSphere(transform.position, transform.localScale.x);
    }
}
