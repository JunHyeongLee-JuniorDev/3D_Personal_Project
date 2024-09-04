using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PlayerAirData 
{
    [field: Header("Jump Force")]

    [Tooltip("grounded�� üũ�� Sphere�� ����")]
    [field: SerializeField]
    [field: Range(0f,0.2f)]
    public float groundedOffset { get; private set; } = 0.14f;

    [Tooltip("grounded�� üũ�� Sphere�� ������")]
    [field: SerializeField]
    public float groundedRadius { get; private set; } = 0.28f;

    [field: SerializeField]
    public LayerMask groundLayers { get; private set; }

    [field: SerializeField]
    [Tooltip("�������� �ð�")]
    public float JumpTimeout = 0.50f;

    [field: SerializeField]
    [Tooltip("�������� �ð�")]
    public float FallTimeout = 0.15f;

    [Space(10)]
    [Tooltip("�÷��� ���� ����")]
    [field: SerializeField]
    public float JumpHeight = 1.2f;

    [Tooltip("�÷��̾� �߷� ������ ����Ƽ Default : -9.8f")]
    [field: SerializeField]
    public float Gravity = -9.8f;

    [Tooltip("�÷��̾� �߷� ������ ����Ƽ Default : -9.8f")]
    [field: SerializeField]
    public float GravityOffset = 0.1f;


}
