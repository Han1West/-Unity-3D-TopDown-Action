using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float dashCoolDown = 1f;

    PlayerInput input;
    PlayerMovement movement;    
    PlayerAttack attack;
    PlayerGuard guard;
    PlayerDead dead;
    public PlayerState currentState { get; private set; } = PlayerState.Idle;

    float dashTimer = 0;
    bool canDash = true;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovement>();        
        attack = GetComponent<PlayerAttack>();
        guard = GetComponent<PlayerGuard>();
        dead = GetComponent<PlayerDead>();
    }

    void Update()
    {
        // 현재 Dead State 라면 모든 동작 수행 중지
        if ((currentState == PlayerState.Dead))
        {
            return;
        }
        HandleState();
        StateUpdate();        

        // 대쉬 사용함
        if(!canDash)
        {
            dashTimer += Time.deltaTime;
            if(dashTimer > dashCoolDown )
            {
                canDash = true;
                dashTimer = 0f;
            }
        }
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
                guard.UpdateGuard(input.guard);
                break;            

            default:
                break;
        }
    }

    void HandleState()
    {        
        if(input.dash)
        {
            if(canDash)
            {
                ChangeState(PlayerState.Dash);
                canDash = false;
            }            
            // 입력 소비
            input.dash = false;
        }
        else if (input.guard)
        {
            ChangeState(PlayerState.Guard);
        }
        else if (input.attack)
        {
            if (currentState == PlayerState.Attack)
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
        if (currentState == PlayerState.Attack)
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
                guard.StartGuard();
                break;
            case PlayerState.Dead:
                dead.StartDead();
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
                guard.EndGuard();
                break;
            default:
                break;
        }
    }


    public void OnAttackFinished()
    {
        // 공격 중에 공격 요청이 한번 더 들어온경우 다음 공격 바로 실행
        if (attack.ConsumeNextAttackQueued())
        {
            attack.StartAttack();            
        }
            
        else
        {
            ChangeState(PlayerState.Idle);            
        }
            
    }
}