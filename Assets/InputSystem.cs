// GENERATED AUTOMATICALLY FROM 'Assets/InputSystem.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputSystem : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputSystem()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputSystem"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""f356814e-46f6-4e76-a113-912de473718d"",
            ""actions"": [
                {
                    ""name"": ""MoveX"",
                    ""type"": ""Button"",
                    ""id"": ""b7b74f64-fc27-4038-950f-8e4656f4e9c7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveZ"",
                    ""type"": ""Button"",
                    ""id"": ""76851e30-f574-45d3-9dce-485dac1ba9e1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PushObject"",
                    ""type"": ""Button"",
                    ""id"": ""a0d5c656-f740-41ab-b22a-7e34407dbb25"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SpawnFirstPortal"",
                    ""type"": ""Button"",
                    ""id"": ""1052a5d4-72a5-49ec-b73b-8869a2ff41d6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SpawnSecondPortal"",
                    ""type"": ""Button"",
                    ""id"": ""79159ce0-8237-4bdf-9be9-b74e43eee03a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TakePortable"",
                    ""type"": ""Button"",
                    ""id"": ""7d063742-8322-4509-84b7-5860b65cc933"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraHorizontal"",
                    ""type"": ""Value"",
                    ""id"": ""c5d91220-6ce3-40a5-bcd0-3d445d0578bc"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraVertical"",
                    ""type"": ""Value"",
                    ""id"": ""3ef32395-503b-4a41-b884-c4d67c1016b7"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""e1430fa0-6de3-4f11-9008-0b583f378e7b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Horizontal"",
                    ""id"": ""b186cd86-4439-4f44-ba9a-0a1f04f2cf20"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveX"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Negative"",
                    ""id"": ""a03263f9-9755-470b-9b9a-bc56191da223"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Positive"",
                    ""id"": ""9ba0d2ee-869b-4c0a-af57-ea03ff6fe349"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Vertical"",
                    ""id"": ""ad5f9e22-030a-4dbf-94b6-84434cc1e0e2"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveZ"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""575569b4-dc28-4520-ace0-a63818035c62"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveZ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""87f42a09-3608-40b6-806e-7a15aafcee5b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveZ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""44ea70d3-0e92-410c-bacf-ef5d99a2998e"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PushObject"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2aaa99e2-4258-4230-ac44-bf67bdf78b7c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpawnFirstPortal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a14d1737-edbf-4b2b-8fdb-35382dbfc07d"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpawnSecondPortal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ec484014-ebbe-4a7f-a737-4cbb93c7caac"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraHorizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8fd0da4c-f368-43bd-be3b-267983e7e5ee"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraVertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c7ac5b1e-11e0-4412-8d72-483554e28008"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TakePortable"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f72bb0c1-be10-4409-a182-c8e403824da4"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_MoveX = m_Player.FindAction("MoveX", throwIfNotFound: true);
        m_Player_MoveZ = m_Player.FindAction("MoveZ", throwIfNotFound: true);
        m_Player_PushObject = m_Player.FindAction("PushObject", throwIfNotFound: true);
        m_Player_SpawnFirstPortal = m_Player.FindAction("SpawnFirstPortal", throwIfNotFound: true);
        m_Player_SpawnSecondPortal = m_Player.FindAction("SpawnSecondPortal", throwIfNotFound: true);
        m_Player_TakePortable = m_Player.FindAction("TakePortable", throwIfNotFound: true);
        m_Player_CameraHorizontal = m_Player.FindAction("CameraHorizontal", throwIfNotFound: true);
        m_Player_CameraVertical = m_Player.FindAction("CameraVertical", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_MoveX;
    private readonly InputAction m_Player_MoveZ;
    private readonly InputAction m_Player_PushObject;
    private readonly InputAction m_Player_SpawnFirstPortal;
    private readonly InputAction m_Player_SpawnSecondPortal;
    private readonly InputAction m_Player_TakePortable;
    private readonly InputAction m_Player_CameraHorizontal;
    private readonly InputAction m_Player_CameraVertical;
    private readonly InputAction m_Player_Jump;
    public struct PlayerActions
    {
        private @InputSystem m_Wrapper;
        public PlayerActions(@InputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveX => m_Wrapper.m_Player_MoveX;
        public InputAction @MoveZ => m_Wrapper.m_Player_MoveZ;
        public InputAction @PushObject => m_Wrapper.m_Player_PushObject;
        public InputAction @SpawnFirstPortal => m_Wrapper.m_Player_SpawnFirstPortal;
        public InputAction @SpawnSecondPortal => m_Wrapper.m_Player_SpawnSecondPortal;
        public InputAction @TakePortable => m_Wrapper.m_Player_TakePortable;
        public InputAction @CameraHorizontal => m_Wrapper.m_Player_CameraHorizontal;
        public InputAction @CameraVertical => m_Wrapper.m_Player_CameraVertical;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @MoveX.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveX;
                @MoveX.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveX;
                @MoveX.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveX;
                @MoveZ.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveZ;
                @MoveZ.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveZ;
                @MoveZ.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveZ;
                @PushObject.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPushObject;
                @PushObject.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPushObject;
                @PushObject.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPushObject;
                @SpawnFirstPortal.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpawnFirstPortal;
                @SpawnFirstPortal.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpawnFirstPortal;
                @SpawnFirstPortal.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpawnFirstPortal;
                @SpawnSecondPortal.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpawnSecondPortal;
                @SpawnSecondPortal.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpawnSecondPortal;
                @SpawnSecondPortal.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpawnSecondPortal;
                @TakePortable.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTakePortable;
                @TakePortable.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTakePortable;
                @TakePortable.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTakePortable;
                @CameraHorizontal.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraHorizontal;
                @CameraHorizontal.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraHorizontal;
                @CameraHorizontal.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraHorizontal;
                @CameraVertical.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraVertical;
                @CameraVertical.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraVertical;
                @CameraVertical.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraVertical;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveX.started += instance.OnMoveX;
                @MoveX.performed += instance.OnMoveX;
                @MoveX.canceled += instance.OnMoveX;
                @MoveZ.started += instance.OnMoveZ;
                @MoveZ.performed += instance.OnMoveZ;
                @MoveZ.canceled += instance.OnMoveZ;
                @PushObject.started += instance.OnPushObject;
                @PushObject.performed += instance.OnPushObject;
                @PushObject.canceled += instance.OnPushObject;
                @SpawnFirstPortal.started += instance.OnSpawnFirstPortal;
                @SpawnFirstPortal.performed += instance.OnSpawnFirstPortal;
                @SpawnFirstPortal.canceled += instance.OnSpawnFirstPortal;
                @SpawnSecondPortal.started += instance.OnSpawnSecondPortal;
                @SpawnSecondPortal.performed += instance.OnSpawnSecondPortal;
                @SpawnSecondPortal.canceled += instance.OnSpawnSecondPortal;
                @TakePortable.started += instance.OnTakePortable;
                @TakePortable.performed += instance.OnTakePortable;
                @TakePortable.canceled += instance.OnTakePortable;
                @CameraHorizontal.started += instance.OnCameraHorizontal;
                @CameraHorizontal.performed += instance.OnCameraHorizontal;
                @CameraHorizontal.canceled += instance.OnCameraHorizontal;
                @CameraVertical.started += instance.OnCameraVertical;
                @CameraVertical.performed += instance.OnCameraVertical;
                @CameraVertical.canceled += instance.OnCameraVertical;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnMoveX(InputAction.CallbackContext context);
        void OnMoveZ(InputAction.CallbackContext context);
        void OnPushObject(InputAction.CallbackContext context);
        void OnSpawnFirstPortal(InputAction.CallbackContext context);
        void OnSpawnSecondPortal(InputAction.CallbackContext context);
        void OnTakePortable(InputAction.CallbackContext context);
        void OnCameraHorizontal(InputAction.CallbackContext context);
        void OnCameraVertical(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
}
