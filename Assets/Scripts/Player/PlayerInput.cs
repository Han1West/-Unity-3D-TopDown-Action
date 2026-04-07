using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{    
    public Vector2 move;
    public Vector2 mousePosition;
    public bool run = false;        
    public bool dash = false;
    public bool attack = false;
    public bool guard = false;

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnRun(InputValue value)
    {
        run = value.isPressed;
    }

    public void OnDash(InputValue value)
    {
        if (value.isPressed && !dash)
        {            
            dash = true;
            Debug.Log("Pressed Dash");
        }
    }
    
    public void OnLook(InputValue value)
    {
        mousePosition = value.Get<Vector2>();
    }

    public void OnAttack(InputValue value)
    {
        attack = value.isPressed;
    }

    public void OnGuard(InputValue value)
    {
        guard = value.isPressed;        
    }
}
