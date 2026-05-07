using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.AI;

public enum BossState
{ 
    Idle,
    Chase,
    Pattern,
    Attack,
    Event,
    Stunned,
    Dead
}


public class BossMonsterBase : MonoBehaviour
{
    [Header("Range")]
    [SerializeField] float attackRange = 10f;
    [SerializeField] float chaseRange = 50f;

    [Header("Cooldown")]
    [SerializeField] protected float attackCooldown = 1.5f;
    [SerializeField] protected float patternCheckCooldown = 4.5f;

    [Header("Pattern")]
    [SerializeField] float patternDuration = 4f;
    [SerializeField] float basePatternChance = 30f;
    [SerializeField] float failBonus = 5f;
    [SerializeField] float successPenalty = 15f;
    [SerializeField] float minChance = 10f;
    [SerializeField] float maxChance = 80f;

    [Header("Stun")]
    [SerializeField] float stunDuration = 2.5f;
    [SerializeField] protected ParticleSystem stunnedVFX;

    protected CinemachineImpulseSource rageImpulseSource;
    protected AudioSource audioSource;
    protected Animator animator;
    protected NavMeshAgent agent;
    protected PlayerController player;
    protected BossState currentState = BossState.Idle;

    EnemyHealth health;
    GameManager gameManager;

    public bool isBusy = false;
    protected bool isRage = false;
    protected bool pendingRage = false;
    protected bool isStunned = false;

    float lastAttackTime = 0f;
    float lastPaternTime = 0f;
    float patternTimer = 0f;
    float currentPatternChance;

    #region Unity
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
        rageImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        player = FindFirstObjectByType<PlayerController>();
        currentPatternChance = basePatternChance;
        ChangeState(BossState.Idle);
    }

    void Update()
    {
        // 게임 중지 or 종료
        if (!gameManager.IsPlay) return;

        // 스턴 상태라면 모든 로직 막음
        if (player == null || isStunned || currentState == BossState.Dead) return;
        
        TryStartPattern();

        switch (currentState)
        {
            case BossState.Idle:
                UpdateIdle();
                break;

            case BossState.Chase:
                UpdateChase();
                break;

            case BossState.Attack:
                UpdateAttack();
                break;

            case BossState.Pattern:
                UpdatePattern();
                break;

            case BossState.Event:
                UpdateEvent();
                break;
        }

        TryStartpendingRage();

        if (isRage)
            UpdateRageLogic();
    }


    void OnDisable()
    {
        if (agent != null && agent.enabled)
            agent.enabled = false;
    }

    #endregion

    #region State

    protected void ChangeState(BossState newState)
    {
        ExitState(currentState);
        currentState = newState;
        EnterState(newState);        
    }

    void EnterState(BossState state)
    {
        switch (state)
        {
            case BossState.Idle:
                animator.SetBool("IsWalking", false);
                StopMove();
                break;

            case BossState.Chase:
                animator.SetBool("IsWalking", true);
                agent.updateRotation = true;
                ResumeMove();
                break;

            case BossState.Attack:
                animator.SetBool("IsWalking", false);
                agent.updateRotation = false;
                StopMove();
                break;

            case BossState.Pattern:
                animator.SetBool("IsWalking", false);
                StopMove();
                patternTimer = 0f;
                isBusy = true;
                StartPattern();
                break;

            case BossState.Event:
                animator.SetBool("IsWalking", false);
                StopMove();
                isBusy = true;
                StartEvent();
                break;

            case BossState.Stunned:
                animator.SetBool("IsWalking", false);
                StopMove();
                break;

            case BossState.Dead:
                animator.SetBool("IsWalking", false);
                StopMove();
                break;
        }
    }

    void ExitState(BossState State)
    {
        if (State == BossState.Pattern)
            EndPattern();
        if (State == BossState.Event)
            EndEvent();
    }
    #endregion

    #region Idle

    void UpdateIdle()
    {
        if (DistanceToPlayer() <= chaseRange)
            ChangeState(BossState.Chase);
    }
    #endregion

    #region Chase

    void UpdateChase()
    {        
        agent.SetDestination(player.transform.position);

        if (DistanceToPlayer() <= attackRange)
            ChangeState(BossState.Attack);
    }
    #endregion

    #region Attack

    void UpdateAttack()
    {
        // 항상 플레이어 바라봄
        LookAtPlayer();

        if (isBusy)
            return;        

        if(DistanceToPlayer() > attackRange)
        {
            ChangeState(BossState.Chase);
            return;
        }

        if(Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            isBusy = true;
            Attack();
        }
    }

    protected void EndAttack()
    {
        isBusy = false;
        ChangeState(BossState.Idle);
    }
    #endregion

    #region Pattern

    void TryStartPattern()
    {
        if (currentState == BossState.Pattern ||
            currentState == BossState.Stunned ||
            isBusy)
            return;

        if (Time.time - lastPaternTime < patternCheckCooldown)
            return;

        lastPaternTime = Time.time;

        int rand = UnityEngine.Random.Range(0, 100);

        // 패턴 발동
        if(rand < currentPatternChance)
        {
            ChangeState(BossState.Pattern);
            patternTimer = 0f;

            // 성공하면 기본 확률 - 페널티 확률 로 돌아감
            if(currentPatternChance > basePatternChance)
                currentPatternChance = basePatternChance - successPenalty;
            else
                currentPatternChance -= successPenalty;
        }
        // 실패
        else
        {
            // 현재 확률이 기본확률 보다 낮았다면 -> 기본 확률로
            if (currentPatternChance < basePatternChance)
                currentPatternChance = basePatternChance;
            // 기본확률 보다 크거나 같으면 보너스 확률 추가
            else
                currentPatternChance += failBonus;
        }
        
        currentPatternChance = Mathf.Clamp(currentPatternChance, minChance, maxChance);
    }

    void UpdatePattern()
    {
        patternTimer += Time.deltaTime;

        PatternUpdate();

        if (patternTimer > patternDuration && CanEndPattern())
        {
            isBusy = false;
            ChangeState(BossState.Idle);
        }
    }

    protected virtual bool CanEndPattern()
    {
        return true;
    }

    #endregion

    #region Event

    void UpdateEvent()
    {
        EventUpdate();
    }

    void TryStartpendingRage()
    {
        if (!pendingRage || isRage)
            return;

        if (isBusy) 
            return;

        if (currentState == BossState.Pattern || currentState == BossState.Attack
            || currentState == BossState.Event)
            return;

        pendingRage = false;
        EnterRageMode();
    }

    public void RequsetRage()
    {        
        pendingRage = true;
    }

    public void RequestDead()
    {           
        EnterDeadEvent();
    }

    #endregion
    #region Stun
    public void GetStunned()
    {
        if (isStunned)
            return;        
        
        isStunned = true;
        isBusy = true;
        ChangeState(BossState.Stunned);       
        animator.SetTrigger("Stunned");

        if(stunnedVFX != null)
            stunnedVFX.Play();

        // StopAllCoroutine 아래 함수 내부에 실행
        OnStunned();
        
        StartCoroutine(RecoverFromStun());
    }

    IEnumerator RecoverFromStun()
    {
        yield return new WaitForSeconds(stunDuration);

        if(stunnedVFX != null)
            stunnedVFX.Stop();        
        animator.SetTrigger("StunEnd");

        isStunned = false;
        isBusy = false;

        ChangeState(BossState.Idle);
    }
    #endregion

    #region Utility

    protected float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }

    void LookAtPlayer()
    {        
        Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
        dirToPlayer.y = 0f;

        if (dirToPlayer != Vector3.zero)
        {
            Quaternion targerRotation = Quaternion.LookRotation(dirToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targerRotation, Time.deltaTime * 10f);
        }
    }

    void StopMove()
    {
        if (!agent.enabled) return;

        agent.isStopped = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;
    }

    void ResumeMove()
    {
        if(!agent.enabled) return;

        agent.isStopped = false;
    }    
    #endregion

    #region Virtual    
    protected virtual void Attack() { }
    protected virtual void StartPattern() { } 
    protected virtual void PatternUpdate() { }
    protected virtual void EndPattern() { }
    protected virtual void StartEvent() { }
    protected virtual void EventUpdate() { }
    protected virtual void EndEvent() { }
    protected virtual void OnStunned() 
    {
        StopAllCoroutines();
    }
    protected virtual void UpdateRageLogic() { }
    

    protected virtual void EnterRageMode() { }

    protected virtual void EnterDeadEvent() { }
    
    #endregion
}