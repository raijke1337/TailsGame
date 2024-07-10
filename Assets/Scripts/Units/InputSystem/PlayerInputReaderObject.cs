using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerControls;
[CreateAssetMenu(fileName = "InputReader", menuName = "Inputs/InputReader")]
public class PlayerInputReaderObject : ScriptableObject, IGameActions
{
    private PlayerControls _controls;

    public event UnityAction<Vector2> Movement = delegate { };
    public event UnityAction<Vector2> Aim = delegate { };

    public event UnityAction Jump = delegate { };
    public event UnityAction Melee = delegate { };
    public event UnityAction Ranged = delegate { };

    public event UnityAction DodgeSpec = delegate { };
    public event UnityAction MeleeSpec = delegate { };
    public event UnityAction RangedSpec = delegate { };
    public event UnityAction ShieldSpec = delegate { };


    public event UnityAction MountAction = delegate { };
    public event UnityAction PausePressed = delegate { };


    public Vector3 InputDirection
    {get
        {
            Vector2 read = _controls.Game.WASD.ReadValue<Vector2>();
            return new Vector3(read.x,0f,read.y);
        }
    }
    public Vector2 AimPoint => _controls.Game.Aim.ReadValue<Vector2>();



    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new PlayerControls();
            _controls.Game.SetCallbacks(this);
        }
    }

    public void EnablePlayerInputs()
    {
        _controls.Enable();
    }


    #region calls

    public void OnWASD(InputAction.CallbackContext context)
    {
        Movement.Invoke(context.ReadValue<Vector2>());
    }
    public void OnAim(InputAction.CallbackContext context)
    {
       Aim.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Jump.Invoke();
    }
    public void OnMainAttack(InputAction.CallbackContext context)
    {
        Melee.Invoke();
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        Ranged.Invoke();
    }


    public void OnPause(InputAction.CallbackContext context)
    {
        PausePressed.Invoke();
    }

    public void OnUseMeleeSkill(InputAction.CallbackContext context)
    {
        MeleeSpec.Invoke();
    }

    public void OnUseRangedSkill(InputAction.CallbackContext context)
    {
        RangedSpec.Invoke();
    }

    public void OnUseShieldSkill(InputAction.CallbackContext context)
    {
        ShieldSpec.Invoke();
    }

    public void OnUseDodgeSkill(InputAction.CallbackContext context)
    {
        DodgeSpec.Invoke();
    }
    public void OnMount(InputAction.CallbackContext context)
    {
        MountAction.Invoke();
    }
    #endregion
}
