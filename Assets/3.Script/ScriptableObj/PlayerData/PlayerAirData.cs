using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PlayerAirData 
{
    [field: Header("Jump Force")]

    [Tooltip("grounded를 체크할 Sphere의 높이")]
    [field: SerializeField]
    [field: Range(0f,0.2f)]
    public float groundedOffset { get; private set; } = 0.14f;

    [Tooltip("grounded를 체크할 Sphere의 반지름")]
    [field: SerializeField]
    public float groundedRadius { get; private set; } = 0.28f;

    [field: SerializeField]
    public LayerMask groundLayers { get; private set; }

    [field: SerializeField]
    [Tooltip("떨어지는 시간")]
    public float JumpTimeout = 0.50f;

    [field: SerializeField]
    [Tooltip("떨어지는 시간")]
    public float FallTimeout = 0.15f;

    [Space(10)]
    [Tooltip("플레이 점프 높이")]
    [field: SerializeField]
    public float JumpHeight = 1.2f;

    [Tooltip("플레이어 중력 데이터 유니티 Default : -9.8f")]
    [field: SerializeField]
    public float Gravity = -9.8f;

    [Tooltip("플레이어 중력 데이터 유니티 Default : -9.8f")]
    [field: SerializeField]
    public float GravityOffset = 0.1f;


}
