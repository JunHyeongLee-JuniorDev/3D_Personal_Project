using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLockOn : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private void OnEnable()
    {
        if (target == null) target = Camera.main.transform;
        StartCoroutine(lookCam());
    }

    private IEnumerator lookCam()
    {
        while (gameObject.activeInHierarchy)
        {
            transform.rotation = Quaternion.LookRotation(target.position - transform.position);
            yield return null;
        }
    }

}
