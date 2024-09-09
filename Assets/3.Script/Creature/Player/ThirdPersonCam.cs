using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("�ó׸ӽ� ī�޶� ��ǥ")]
    public GameObject m_cinemachineCamTarget;

    [Tooltip("ī�޶� �� ����")]
    public float m_topClamp = 70.0f;

    [Tooltip("ī�޶� �� ����")]
    public float m_bottomClamp = -30.0f;

    [Tooltip("���� �𸣰ڴ� ���߿� �� �� �˾ƺ����� ����")]
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
        //���콺�� �������ٸ�
        if (m_input.look.sqrMagnitude >= m_threshold)
        {
            // mouse input�� Time.deltaTime���� ����������
            // �ٵ� ��???
            // �Ƹ� ���� �ʹ� �۾Ƽ� �׷��� �ƴұ�?
            // ���� �𸣰ڴ�

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
