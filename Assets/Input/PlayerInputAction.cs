//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/PlayerInputAction.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputAction: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputAction"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""8dc2c6bb-9c97-4fdb-bb35-c3585bf1167f"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""ead91f98-41f3-4bf7-bccf-5a43925e2527"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""4c2a0162-28f7-40cf-8914-955c565e8306"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""0bb4b698-1121-4160-85c3-763acde64b7a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""e1618cc0-63fb-4bae-9a59-d003f86b0153"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Battle"",
                    ""type"": ""Button"",
                    ""id"": ""351a75c9-aa2d-46c9-aa6e-771ba145e727"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Block"",
                    ""type"": ""Button"",
                    ""id"": ""e73f58ed-507b-4ca8-be56-61272c8f4e24"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""aff1fbd9-014a-4b7e-9282-404a6305e460"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseWheel"",
                    ""type"": ""Button"",
                    ""id"": ""d3473f85-fa19-4242-8b2e-d99bf6ea2d9b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""T"",
                    ""type"": ""Button"",
                    ""id"": ""751fb83b-8fa5-48e6-bc51-dd43791e04bf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Skill"",
                    ""type"": ""Button"",
                    ""id"": ""c2e516fa-1b39-49ad-9307-b7e53699ae59"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""OnRoll"",
                    ""type"": ""Button"",
                    ""id"": ""d272fa6f-67e2-4c4b-853a-beb928ddcc8c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""91f4ce1e-6a43-4168-a0fb-84712f3d22ef"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""1e71df06-d940-4e85-85e7-5ddb80d29034"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2b6060ae-d73c-4a71-a186-df8f9d468d07"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c9037d4e-90f9-4478-9b66-a3e9d24b96df"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""122ce021-29db-4f23-bbd1-53a3e7173687"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d1006434-0ce4-4acd-8d46-cd01f57a3df9"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false),ScaleVector2(x=0.05,y=0.05)"",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9a06adc3-9756-44fa-9b37-d23cd1704390"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""52a7b76b-b9b7-44aa-9792-ff02f16ec1c8"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""13b67ef1-beed-47d8-8e3e-eaf1d583099e"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Battle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf25c9de-e892-48e9-933a-ba5924601e75"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c7ddcc3-2d06-4c97-888b-b098b3cc443b"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33daf4d2-e2da-4fee-a662-63b6a274977b"",
                    ""path"": ""<Mouse>/scroll/up"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82a5823f-8b31-4af6-a555-e5e45d6f42e3"",
                    ""path"": ""<Mouse>/scroll/down"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d7a9f653-2a33-48e3-94d4-7be1a2a62057"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""T"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""111a985d-7f9e-4853-bb68-e66f7e6284c3"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4a4aad0-063f-4791-861d-3b8792411252"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OnRoll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_Fire = m_Player.FindAction("Fire", throwIfNotFound: true);
        m_Player_Sprint = m_Player.FindAction("Sprint", throwIfNotFound: true);
        m_Player_Battle = m_Player.FindAction("Battle", throwIfNotFound: true);
        m_Player_Block = m_Player.FindAction("Block", throwIfNotFound: true);
        m_Player_Interaction = m_Player.FindAction("Interaction", throwIfNotFound: true);
        m_Player_MouseWheel = m_Player.FindAction("MouseWheel", throwIfNotFound: true);
        m_Player_T = m_Player.FindAction("T", throwIfNotFound: true);
        m_Player_Skill = m_Player.FindAction("Skill", throwIfNotFound: true);
        m_Player_OnRoll = m_Player.FindAction("OnRoll", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_Fire;
    private readonly InputAction m_Player_Sprint;
    private readonly InputAction m_Player_Battle;
    private readonly InputAction m_Player_Block;
    private readonly InputAction m_Player_Interaction;
    private readonly InputAction m_Player_MouseWheel;
    private readonly InputAction m_Player_T;
    private readonly InputAction m_Player_Skill;
    private readonly InputAction m_Player_OnRoll;
    public struct PlayerActions
    {
        private @PlayerInputAction m_Wrapper;
        public PlayerActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @Fire => m_Wrapper.m_Player_Fire;
        public InputAction @Sprint => m_Wrapper.m_Player_Sprint;
        public InputAction @Battle => m_Wrapper.m_Player_Battle;
        public InputAction @Block => m_Wrapper.m_Player_Block;
        public InputAction @Interaction => m_Wrapper.m_Player_Interaction;
        public InputAction @MouseWheel => m_Wrapper.m_Player_MouseWheel;
        public InputAction @T => m_Wrapper.m_Player_T;
        public InputAction @Skill => m_Wrapper.m_Player_Skill;
        public InputAction @OnRoll => m_Wrapper.m_Player_OnRoll;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Fire.started += instance.OnFire;
            @Fire.performed += instance.OnFire;
            @Fire.canceled += instance.OnFire;
            @Sprint.started += instance.OnSprint;
            @Sprint.performed += instance.OnSprint;
            @Sprint.canceled += instance.OnSprint;
            @Battle.started += instance.OnBattle;
            @Battle.performed += instance.OnBattle;
            @Battle.canceled += instance.OnBattle;
            @Block.started += instance.OnBlock;
            @Block.performed += instance.OnBlock;
            @Block.canceled += instance.OnBlock;
            @Interaction.started += instance.OnInteraction;
            @Interaction.performed += instance.OnInteraction;
            @Interaction.canceled += instance.OnInteraction;
            @MouseWheel.started += instance.OnMouseWheel;
            @MouseWheel.performed += instance.OnMouseWheel;
            @MouseWheel.canceled += instance.OnMouseWheel;
            @T.started += instance.OnT;
            @T.performed += instance.OnT;
            @T.canceled += instance.OnT;
            @Skill.started += instance.OnSkill;
            @Skill.performed += instance.OnSkill;
            @Skill.canceled += instance.OnSkill;
            @OnRoll.started += instance.OnOnRoll;
            @OnRoll.performed += instance.OnOnRoll;
            @OnRoll.canceled += instance.OnOnRoll;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Fire.started -= instance.OnFire;
            @Fire.performed -= instance.OnFire;
            @Fire.canceled -= instance.OnFire;
            @Sprint.started -= instance.OnSprint;
            @Sprint.performed -= instance.OnSprint;
            @Sprint.canceled -= instance.OnSprint;
            @Battle.started -= instance.OnBattle;
            @Battle.performed -= instance.OnBattle;
            @Battle.canceled -= instance.OnBattle;
            @Block.started -= instance.OnBlock;
            @Block.performed -= instance.OnBlock;
            @Block.canceled -= instance.OnBlock;
            @Interaction.started -= instance.OnInteraction;
            @Interaction.performed -= instance.OnInteraction;
            @Interaction.canceled -= instance.OnInteraction;
            @MouseWheel.started -= instance.OnMouseWheel;
            @MouseWheel.performed -= instance.OnMouseWheel;
            @MouseWheel.canceled -= instance.OnMouseWheel;
            @T.started -= instance.OnT;
            @T.performed -= instance.OnT;
            @T.canceled -= instance.OnT;
            @Skill.started -= instance.OnSkill;
            @Skill.performed -= instance.OnSkill;
            @Skill.canceled -= instance.OnSkill;
            @OnRoll.started -= instance.OnOnRoll;
            @OnRoll.performed -= instance.OnOnRoll;
            @OnRoll.canceled -= instance.OnOnRoll;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnBattle(InputAction.CallbackContext context);
        void OnBlock(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
        void OnMouseWheel(InputAction.CallbackContext context);
        void OnT(InputAction.CallbackContext context);
        void OnSkill(InputAction.CallbackContext context);
        void OnOnRoll(InputAction.CallbackContext context);
    }
}
