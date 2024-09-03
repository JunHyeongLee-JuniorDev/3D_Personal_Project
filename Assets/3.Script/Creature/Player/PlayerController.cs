using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field: Header("캐릭터 데이터")]
    [field: SerializeField]
    public PlayerSO m_PhysicsData { get; private set; }

    [field : Header("캐릭터 애니메이션 데이터")]
    [field : SerializeField]
    public PlayerAnimationData m_aniData { get; private set; }

    //Unity Components
    [field : HideInInspector]
    public CharacterController m_Controller;
    public PlayerStateMachine m_StateMachine { get; private set; }
    public PlayerinputSystem m_input { get; private set; }
    public Animator m_animator { get; private set; }
    public GameObject m_mainCam { get; private set; }

    private void Awake()
    {
        if (m_mainCam == null)
        {
            m_mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        }

        m_StateMachine = new PlayerStateMachine(this);
    }

    private void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_input = GetComponent<PlayerinputSystem>();
        m_input.player = this;
        m_animator = GetComponent<Animator>();
        m_aniData = new PlayerAnimationData();
        m_aniData.Initialize();

        m_StateMachine.ChangeState(m_StateMachine.idleState);
    }

    private void Update()
    {
        m_StateMachine.Update();
    }

    private void FixedUpdate()
    {
        m_StateMachine.PhysicsUpdate();
    }
}
