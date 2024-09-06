using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerGroundData
{

    [field: Header("Speed Change Data")]
    [field: SerializeField]
    [field: Range(0f, 1f)]
    public float rotationSmoothTime { get; private set; } = 0.12f;

    [field: SerializeField]
    [field: Range(0f, 25f)]
    public float speedChangeRate { get; private set; } = 10f;

    [field: Header("Walk Data")]
    [field: SerializeField]
    [field: Range(0f, 25f)]
    public float BaseSpeed { get; private set; } = 5f;

    [field: Header("Run Data")]
    [field: SerializeField]
    [field: Range(0f, 100f)]
    public float sprintSpeed { get; private set; } = 10f;

    [field: Header("Battle Walk Data")]
    [field: SerializeField]
    [field: Range(0f, 100f)]

    public float battleWalkSpeed { get; private set; } = 3f;
}
