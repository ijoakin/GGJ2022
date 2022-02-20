using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputCotroller : MonoBehaviour
{
    public static PlayerInputCotroller Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        Player.Instance.rawInputMovement = new Vector2(inputMovement.x, inputMovement.y);
    }

    public void OnAttack(InputAction.CallbackContext value)
    {
        if (value.started) {
            Player.Instance.Attack = true;
        }
    }
    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.started) {
            Player.Instance.Jump = true;
        }
    }
}

