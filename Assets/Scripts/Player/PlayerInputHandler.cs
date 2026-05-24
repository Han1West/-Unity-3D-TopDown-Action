using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{    
    public Vector2 move;
    public Vector2 mousePosition;
    public bool run = false;        
    public bool dash = false;
    public bool attack = false;
    public bool guard = false;
    public bool skill = false;

    public void OnMove(InputValue value)
    {
        if (!GameManager.Instance.isInPlaying)
            return;

        move = value.Get<Vector2>();
    }

    public void OnRun(InputValue value)
    {
        if (!GameManager.Instance.isInPlaying)
            return;

        run = value.isPressed;
    }

    public void OnDash(InputValue value)
    {
        if (!GameManager.Instance.isInPlaying)
            return;

        if (value.isPressed && !dash)                   
            dash = true;                    
    }
    
    public void OnLook(InputValue value)
    {
        if (!GameManager.Instance.isInPlaying)
            return;

        mousePosition = value.Get<Vector2>();
    }

    public void OnAttack(InputValue value)
    {
        if (!GameManager.Instance.isInPlaying)                   
            return;
        
          
        attack = value.isPressed;
        Debug.Log(value);
    }

    public void OnGuard(InputValue value)
    {
        if (!GameManager.Instance.isInPlaying)
            return;

        guard = value.isPressed;        
    }

    public void OnSkill(InputValue value)
    {
        if (!GameManager.Instance.isInPlaying)
            return;

        skill = value.isPressed;
    }

    public void ResetInput()
    {
        run = false;
        dash = false;
        attack = false;
        guard = false;
        skill = false;
        move = Vector2.zero;
    }

}
