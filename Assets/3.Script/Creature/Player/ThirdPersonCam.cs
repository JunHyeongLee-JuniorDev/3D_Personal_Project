using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("시네머신 카메라 목표")]
    public GameObject m_cinemachineCamTarget;

    [Tooltip("카메라 상 제한")]
    public float m_topClamp = 70.0f;

    [Tooltip("카메라 하 제한")]
    public float m_bottomClamp = -30.0f;

    [Tooltip("뭔지 모르겠다 나중에 한 번 알아보도록 하자")]
    public float m_cameraAngleOverride = 0f;

    private PlayerinputSystem m_input;

    private float m_cinemachineTargetYaw;
    private float m_cinemachineTargetPitch;
    private readonly float m_threshold = 0.01f;

    private void Start()
    {
        m_input = GetComponent<PlayerinputSystem>();

        m_cinemachineTargetYaw = m_cinemachineCamTarget.transform.rotation.eulerAngles.y;
    }



    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        //마우스가 움직였다면
        if (m_input.look.sqrMagnitude >= m_threshold)
        {
            // mouse input을 Time.deltaTime으로 곱하지마라
            // 근데 왜???
            // 아마 값이 너무 작아서 그런게 아닐까?
            // 씨벌 모르겠다

            m_cinemachineTargetYaw += m_input.look.x;
            m_cinemachineTargetPitch += m_input.look.y;
        }

        m_cinemachineTargetYaw = ClampAngle(m_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        m_cinemachineTargetPitch = ClampAngle(m_cinemachineTargetPitch, m_bottomClamp, m_topClamp);

        m_cinemachineCamTarget.transform.rotation = Quaternion.Euler(m_cinemachineTargetPitch,
                                                                   m_cinemachineTargetYaw, 0f);
    }

    private float ClampAngle(float _lfAngle, float _lfMin, float _lfMax)
    {
        if (_lfAngle < -360f) _lfAngle += 360f;
        if (_lfAngle > 360f) _lfAngle -= 360f;
        return Mathf.Clamp(_lfAngle, _lfMin, _lfMax);
    }
}
