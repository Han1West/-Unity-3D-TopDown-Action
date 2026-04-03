using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput input;
    PlayerMovement movement;    
    PlayerAttack attack;
    public PlayerState currentState { get; private set; } = PlayerState.Idle;

    bool stateChangedThisFrame = false;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovement>();        
        attack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        stateChangedThisFrame = false;

        HandleState();

        if (stateChangedThisFrame)
            return;

        StateUpdate();        
    }

    void StateUpdate()
    {        
        // 상태에 따라 동작
        switch (currentState)
        {
            case PlayerState.Idle:
                break;

            case PlayerState.Move:
                movement.UpdateMove(input.move, input.run);                
                break;

            case PlayerState.Dash:
                movement.UpdateDash();                
                break;

            case PlayerState.Attack:
                attack.UpdateAttack();
                break;

            case PlayerState.Guard:
                break;

            default:
                break;
        }
    }

    void HandleState()
    {
        if (currentState == PlayerState.Dash) return;

        if(input.dash)
        {
            ChangeState(PlayerState.Dash);        
            // 입력 소비
            input.dash = false;
        }
        else if (input.attack)
        {
            if(currentState == PlayerState.Attack)
                attack.QueueNextAttack();
            else
                ChangeState(PlayerState.Attack);

            input.attack = false;
        }
        // 공격
        else if (input.move != Vector2.zero)
        {
            ChangeState(PlayerState.Move);
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }
    }

    public void ChangeState(PlayerState newState)
    {        
        if (newState == PlayerState.None) return;
        if (currentState == newState) return;
        if (!CanChangeState(newState)) return;


        // 현재 상태 탈출
        OnStateExit(currentState);

        // 상태 갱신
        currentState = newState;        

        // 새로운 상태 진입
        OnStateEnter(currentState);

        stateChangedThisFrame = true;
    }

    bool CanChangeState(PlayerState newState)
    {
        // 현재 상태가 대쉬면 상태 전이를 막는다 (대쉬중에는 다른 동작 불가능)
        if(currentState == PlayerState.Dash)
        {
            if (newState == PlayerState.Idle)
                return true;
            return false;
        }
            

        return true;
    }

    void OnStateEnter(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.None:
                break;
            case PlayerState.Idle:
                break;
            case PlayerState.Move:
                movement.StartMove();
                break;
            case PlayerState.Dash:
                movement.StartDash();
                break;
            case PlayerState.Attack:
                attack.StartAttack();
                break;
            case PlayerState.Guard:
                break;
            default:
                break;
        }
    }
    void OnStateExit(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.None:
                break;
            case PlayerState.Idle:
                break;
            case PlayerState.Move:
                movement.EndMove();
                break;
            case PlayerState.Dash:
                movement.EndDash();
                break;
            case PlayerState.Attack:
                attack.EndAttack();
                break;
            case PlayerState.Guard:
                break;
            default:
                break;
        }
    }
}