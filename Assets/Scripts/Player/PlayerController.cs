using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float dashCoolDown = 1f;

    PlayerInputHandler input;
    PlayerMovement movement;    
    PlayerAttack attack;
    PlayerGuard guard;
    PlayerDead dead;
    PlayerSkill skill;
    PlayerUnderCC underCC;


    public PlayerState CurrentState { get; private set; } = PlayerState.Idle;
    Animator animator;

    float dashTimer = 0;
    bool canDash = true;
    bool canControl = true;


    void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        movement = GetComponent<PlayerMovement>();        
        attack = GetComponent<PlayerAttack>();
        guard = GetComponent<PlayerGuard>();
        dead = GetComponent<PlayerDead>();
        skill = GetComponent<PlayerSkill>();
        underCC = GetComponent<PlayerUnderCC>();

        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        dashTimer = dashCoolDown;
    }

    void Update()
    {

        // 현재 Dead State 라면 모든 동작 수행 중지
        if (CurrentState == PlayerState.Dead)        
            return;
        

        if (!canControl)                   
            return;
        
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
        switch (CurrentState)
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

            case PlayerState.Skill:
                skill.UpdateSkill();
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
        else if (input.skill)
        {
            ChangeState(PlayerState.Skill);
            input.skill = false;
        }
        else if (input.attack)
        {
            if (CurrentState == PlayerState.Attack)
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
        if (CurrentState == newState) return;
        if (!CanChangeState(newState)) return;
        
        

        // 현재 상태 탈출
        OnStateExit(CurrentState);

        // 상태 갱신
        CurrentState = newState;        

        // 새로운 상태 진입
        OnStateEnter(CurrentState);        
    }

    bool CanChangeState(PlayerState newState)
    {
        if (CurrentState == PlayerState.Dead)
            return false;

        // 죽음 상태는 무조건 전이 가능
        if (newState == PlayerState.Dead)
            return true;


        // 현재 상태가 대쉬면 상태 전이를 막는다 (대쉬중에는 다른 동작 불가능)
        if(CurrentState == PlayerState.Dash)
        {
            if (newState == PlayerState.Idle)
                return true;
            return false;
        }
        if (CurrentState == PlayerState.Attack)
        {
            if (newState == PlayerState.Idle)
                return true;
            return false;
        }
        if(CurrentState == PlayerState.Skill)
        {
            if (newState == PlayerState.Idle)
                return true;
            return false; 
        }


        // 진입 요청 스테이트가 스킬이라면 조건을 확인한다.
        if(newState == PlayerState.Skill)
        {
            return CanUseSkill();
        }

        return true;
    }

    bool CanUseSkill()
    {
        if (guard.currentParryPoint >= skill.needSkillPoint)
            return true;

        return false;
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
            case PlayerState.Skill:
                skill.StartSkill();
                guard.UseParryPoint(skill.needSkillPoint);
                break;
            case PlayerState.Guard:
                guard.StartGuard();
                break;
            case PlayerState.Dead:
                input.attack = false;
                input.skill = false;
                input.guard = false;
                input.dash = false;

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
            case PlayerState.Skill:
                skill.EndSkill();
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
        if (CurrentState == PlayerState.Dead)
            return;

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

    public void ApplyCCToPlayer(CCType type)
    {
        switch (type)
        {
            case CCType.None:
                break;
            case CCType.Falldown:
                canControl = false;                
                underCC.PlayCCSequence(type);
                break;
            default:
                break;
        }
    }

    public void EndCC()
    {
        canControl = true;
        animator.SetBool("CanControl", true);
    }
}