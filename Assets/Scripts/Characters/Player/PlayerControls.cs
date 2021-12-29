// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Characters/Player/PlayerControls.inputactions'

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
            ""name"": ""Movement"",
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
                    ""name"": ""Q"",
                    ""type"": ""Button"",
                    ""id"": ""bfd0dbeb-7ca0-4e47-bd96-58cb6786a09d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PauseEditor"",
                    ""type"": ""Button"",
                    ""id"": ""fe54560b-7bd0-45b7-bf9c-f5d72f3ea0eb"",
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
                    ""action"": ""Q"",
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
                    ""action"": ""PauseEditor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""2h"",
            ""id"": ""f3ee7113-de1b-4b57-9ee9-b1237d3838ad"",
            ""actions"": [
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""42c17da4-5f7b-4398-ac38-92a900b2ab19"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ce5ca510-ed6a-41f5-8334-f2523dca5617"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_WASD = m_Movement.FindAction("WASD", throwIfNotFound: true);
        m_Movement_Q = m_Movement.FindAction("Q", throwIfNotFound: true);
        m_Movement_PauseEditor = m_Movement.FindAction("PauseEditor", throwIfNotFound: true);
        // 2h
        m__2h = asset.FindActionMap("2h", throwIfNotFound: true);
        m__2h_Attack = m__2h.FindAction("Attack", throwIfNotFound: true);
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

    // Movement
    private readonly InputActionMap m_Movement;
    private IMovementActions m_MovementActionsCallbackInterface;
    private readonly InputAction m_Movement_WASD;
    private readonly InputAction m_Movement_Q;
    private readonly InputAction m_Movement_PauseEditor;
    public struct MovementActions
    {
        private @PlayerControls m_Wrapper;
        public MovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @WASD => m_Wrapper.m_Movement_WASD;
        public InputAction @Q => m_Wrapper.m_Movement_Q;
        public InputAction @PauseEditor => m_Wrapper.m_Movement_PauseEditor;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                @WASD.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnWASD;
                @WASD.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnWASD;
                @WASD.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnWASD;
                @Q.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnQ;
                @Q.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnQ;
                @Q.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnQ;
                @PauseEditor.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnPauseEditor;
                @PauseEditor.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnPauseEditor;
                @PauseEditor.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnPauseEditor;
            }
            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @WASD.started += instance.OnWASD;
                @WASD.performed += instance.OnWASD;
                @WASD.canceled += instance.OnWASD;
                @Q.started += instance.OnQ;
                @Q.performed += instance.OnQ;
                @Q.canceled += instance.OnQ;
                @PauseEditor.started += instance.OnPauseEditor;
                @PauseEditor.performed += instance.OnPauseEditor;
                @PauseEditor.canceled += instance.OnPauseEditor;
            }
        }
    }
    public MovementActions @Movement => new MovementActions(this);

    // 2h
    private readonly InputActionMap m__2h;
    private I_2hActions m__2hActionsCallbackInterface;
    private readonly InputAction m__2h_Attack;
    public struct _2hActions
    {
        private @PlayerControls m_Wrapper;
        public _2hActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Attack => m_Wrapper.m__2h_Attack;
        public InputActionMap Get() { return m_Wrapper.m__2h; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(_2hActions set) { return set.Get(); }
        public void SetCallbacks(I_2hActions instance)
        {
            if (m_Wrapper.m__2hActionsCallbackInterface != null)
            {
                @Attack.started -= m_Wrapper.m__2hActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m__2hActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m__2hActionsCallbackInterface.OnAttack;
            }
            m_Wrapper.m__2hActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
            }
        }
    }
    public _2hActions @_2h => new _2hActions(this);
    public interface IMovementActions
    {
        void OnWASD(InputAction.CallbackContext context);
        void OnQ(InputAction.CallbackContext context);
        void OnPauseEditor(InputAction.CallbackContext context);
    }
    public interface I_2hActions
    {
        void OnAttack(InputAction.CallbackContext context);
    }
}
