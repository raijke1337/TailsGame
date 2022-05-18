// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Units/InputSystem/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Game"",
            ""id"": ""a2175133-0639-46e1-85f6-af389ba464f6"",
            ""actions"": [
                {
                    ""name"": ""WASD"",
                    ""type"": ""Value"",
                    ""id"": ""e1dc2b4b-9f42-4aef-8911-12736512847b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SkillQ"",
                    ""type"": ""Button"",
                    ""id"": ""bfd0dbeb-7ca0-4e47-bd96-58cb6786a09d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SkillE"",
                    ""type"": ""Button"",
                    ""id"": ""e2917fca-79e3-4182-bf7b-96c2ec7e5061"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SkillR"",
                    ""type"": ""Button"",
                    ""id"": ""f0f3862b-e0cd-4e33-967e-fe7cc6fe6044"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""fe54560b-7bd0-45b7-bf9c-f5d72f3ea0eb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""dc59ae78-b565-45db-9c63-afd7d5a4a4a6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MainAttack"",
                    ""type"": ""Button"",
                    ""id"": ""d45a1719-ff0a-4fa6-8db8-b280e4a19447"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SpecialAttack"",
                    ""type"": ""Button"",
                    ""id"": ""e9c23c4d-a739-4ae0-8b74-c3ffdef73b6d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Items"",
                    ""type"": ""Button"",
                    ""id"": ""c1abd474-226a-4b66-b401-4ba971a244d4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""f1adefd3-de1e-472c-91ef-fa679fc13d88"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""480496d9-20ed-4a3b-aa66-3a330dd86c75"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4f81c151-9c8e-401c-af6d-9ef0e6de87d4"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b4e89c35-f8dd-4f44-a58b-39945439141b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5191f12a-c6e1-4b4a-bed5-c256f95ec034"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3997acb2-927d-40e6-9910-02eb38948d6b"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkillQ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b3fe82c4-6cfe-4cff-b764-8c513fbe9173"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ea33ecec-6802-494f-9532-7a0b46e38fce"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ec6c3874-581d-41f5-b5a9-07b89654ac68"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkillE"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f9336bd-7f91-4611-a546-95a6244ef73c"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkillR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa95f088-fd79-42f1-a62f-715621b94816"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MainAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6d9e2a57-7241-4427-8dc7-f980ecca1ac2"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpecialAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1000d161-135a-4f12-a1d5-4a6abf769f99"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Items"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Game
        m_Game = asset.FindActionMap("Game", throwIfNotFound: true);
        m_Game_WASD = m_Game.FindAction("WASD", throwIfNotFound: true);
        m_Game_SkillQ = m_Game.FindAction("SkillQ", throwIfNotFound: true);
        m_Game_SkillE = m_Game.FindAction("SkillE", throwIfNotFound: true);
        m_Game_SkillR = m_Game.FindAction("SkillR", throwIfNotFound: true);
        m_Game_Pause = m_Game.FindAction("Pause", throwIfNotFound: true);
        m_Game_Dash = m_Game.FindAction("Dash", throwIfNotFound: true);
        m_Game_MainAttack = m_Game.FindAction("MainAttack", throwIfNotFound: true);
        m_Game_SpecialAttack = m_Game.FindAction("SpecialAttack", throwIfNotFound: true);
        m_Game_Items = m_Game.FindAction("Items", throwIfNotFound: true);
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

    // Game
    private readonly InputActionMap m_Game;
    private IGameActions m_GameActionsCallbackInterface;
    private readonly InputAction m_Game_WASD;
    private readonly InputAction m_Game_SkillQ;
    private readonly InputAction m_Game_SkillE;
    private readonly InputAction m_Game_SkillR;
    private readonly InputAction m_Game_Pause;
    private readonly InputAction m_Game_Dash;
    private readonly InputAction m_Game_MainAttack;
    private readonly InputAction m_Game_SpecialAttack;
    private readonly InputAction m_Game_Items;
    public struct GameActions
    {
        private @PlayerControls m_Wrapper;
        public GameActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @WASD => m_Wrapper.m_Game_WASD;
        public InputAction @SkillQ => m_Wrapper.m_Game_SkillQ;
        public InputAction @SkillE => m_Wrapper.m_Game_SkillE;
        public InputAction @SkillR => m_Wrapper.m_Game_SkillR;
        public InputAction @Pause => m_Wrapper.m_Game_Pause;
        public InputAction @Dash => m_Wrapper.m_Game_Dash;
        public InputAction @MainAttack => m_Wrapper.m_Game_MainAttack;
        public InputAction @SpecialAttack => m_Wrapper.m_Game_SpecialAttack;
        public InputAction @Items => m_Wrapper.m_Game_Items;
        public InputActionMap Get() { return m_Wrapper.m_Game; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameActions set) { return set.Get(); }
        public void SetCallbacks(IGameActions instance)
        {
            if (m_Wrapper.m_GameActionsCallbackInterface != null)
            {
                @WASD.started -= m_Wrapper.m_GameActionsCallbackInterface.OnWASD;
                @WASD.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnWASD;
                @WASD.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnWASD;
                @SkillQ.started -= m_Wrapper.m_GameActionsCallbackInterface.OnSkillQ;
                @SkillQ.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnSkillQ;
                @SkillQ.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnSkillQ;
                @SkillE.started -= m_Wrapper.m_GameActionsCallbackInterface.OnSkillE;
                @SkillE.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnSkillE;
                @SkillE.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnSkillE;
                @SkillR.started -= m_Wrapper.m_GameActionsCallbackInterface.OnSkillR;
                @SkillR.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnSkillR;
                @SkillR.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnSkillR;
                @Pause.started -= m_Wrapper.m_GameActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnPause;
                @Dash.started -= m_Wrapper.m_GameActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnDash;
                @MainAttack.started -= m_Wrapper.m_GameActionsCallbackInterface.OnMainAttack;
                @MainAttack.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnMainAttack;
                @MainAttack.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnMainAttack;
                @SpecialAttack.started -= m_Wrapper.m_GameActionsCallbackInterface.OnSpecialAttack;
                @SpecialAttack.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnSpecialAttack;
                @SpecialAttack.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnSpecialAttack;
                @Items.started -= m_Wrapper.m_GameActionsCallbackInterface.OnItems;
                @Items.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnItems;
                @Items.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnItems;
            }
            m_Wrapper.m_GameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @WASD.started += instance.OnWASD;
                @WASD.performed += instance.OnWASD;
                @WASD.canceled += instance.OnWASD;
                @SkillQ.started += instance.OnSkillQ;
                @SkillQ.performed += instance.OnSkillQ;
                @SkillQ.canceled += instance.OnSkillQ;
                @SkillE.started += instance.OnSkillE;
                @SkillE.performed += instance.OnSkillE;
                @SkillE.canceled += instance.OnSkillE;
                @SkillR.started += instance.OnSkillR;
                @SkillR.performed += instance.OnSkillR;
                @SkillR.canceled += instance.OnSkillR;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @MainAttack.started += instance.OnMainAttack;
                @MainAttack.performed += instance.OnMainAttack;
                @MainAttack.canceled += instance.OnMainAttack;
                @SpecialAttack.started += instance.OnSpecialAttack;
                @SpecialAttack.performed += instance.OnSpecialAttack;
                @SpecialAttack.canceled += instance.OnSpecialAttack;
                @Items.started += instance.OnItems;
                @Items.performed += instance.OnItems;
                @Items.canceled += instance.OnItems;
            }
        }
    }
    public GameActions @Game => new GameActions(this);
    public interface IGameActions
    {
        void OnWASD(InputAction.CallbackContext context);
        void OnSkillQ(InputAction.CallbackContext context);
        void OnSkillE(InputAction.CallbackContext context);
        void OnSkillR(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnMainAttack(InputAction.CallbackContext context);
        void OnSpecialAttack(InputAction.CallbackContext context);
        void OnItems(InputAction.CallbackContext context);
    }
}
