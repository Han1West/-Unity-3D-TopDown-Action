using System;
using System.Collections;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.AI;

enum BossState
{ 
    Idle,
    Chase,
    Pattern,
    Attack,
    Stunned,
}


public class BossMonsterBase : MonoBehaviour
{
    [SerializeField] float attackRange = 10f;
    [SerializeField] float chaseRange = 50f;
    [SerializeField] float patternDecisionCooldown = 4.5f;
    [SerializeField] float stunDuration = 2.5f;
    [SerializeField] float attackCooldown = 1.5f;
    [SerializeField] float specialPatternTime = 4f;
    [SerializeField] ParticleSystem stunnedVFX;

    protected AudioSource audioSource;
    protected Animator animator;

    EnemyHealth health;
    PlayerController player;
    NavMeshAgent agent;
 
    protected bool isStunned = false;
    float lastPaternTime = 0f;
    float lastAttackTime = 0f;
    float specialPatternTimer = 0f;

    float basePatternChance = 30f;
    float currentPatternChance;

    float failBonus = 5f;
    float successPenalty = 15f;
    float minChance = 10f;
    float maxChance = 80f;

    BossState currentState = BossState.Idle;
    

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        currentPatternChance = basePatternChance;
    }

    void Update()
    {
        // 스턴 상태라면 모든 로직 막음
        if (isStunned) return;
        
        TryStartPattern();

        switch (currentState)
        {
            case BossState.Pattern:
                ProcessPatternState();
                break;
            default:
                ProcessCurrentState();
                break;
        }  
    }

    private void TryStartPattern()
    {
        if (currentState == BossState.Pattern)
            return;

        if (Time.time - lastPaternTime < patternDecisionCooldown)
            return;

        lastPaternTime = Time.time;

        int rand = UnityEngine.Random.Range(0, 100);

        // 패턴 발동
        if(rand < currentPatternChance)
        {
            ChangeState(BossState.Pattern);
            specialPatternTimer = 0f;

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

    void ProcessPatternState()
    {        
        specialPatternTimer += Time.deltaTime;
        
        DoPattern();

        // 패턴 종료
        if(specialPatternTimer > specialPatternTime)
        {            
            FinishPattern();
            ChangeState(BossState.Idle);            
        }
    }


    private void ProcessCurrentState()
    {        
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState)
        {
            case BossState.Idle:
                // 거리 이내에 적 -> 추적
                if (distToPlayer <= chaseRange)
                    ChangeState(BossState.Chase);
                break;

            case BossState.Chase:
                // 추적                
                agent.updateRotation = true;
                agent.SetDestination(player.transform.position);

                // 공격 사거리 이내 -> 공격
                if (distToPlayer <= attackRange)
                    ChangeState(BossState.Attack);

                // 거리 멀어짐 -> Idle
                else if (distToPlayer > chaseRange)
                    ChangeState(BossState.Idle);
                break;

            case BossState.Attack:
                // 추적 멈춤
                agent.ResetPath();
                // 플레이를 지속적으로 쳐다봄
                LookAtPlayer();
                // 거리 멀어짐 -> 추적
                if (distToPlayer > attackRange)
                    ChangeState(BossState.Chase);
                // 공격 시간 돌아오면 공격 수행
                else if (Time.time - lastAttackTime >= attackCooldown)
                    DoAttack();
                break;

        }
    }

    private void LookAtPlayer()
    {
        agent.updateRotation = false;

        Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
        dirToPlayer.y = -0f;

        if (dirToPlayer != Vector3.zero)
        {
            Quaternion targerRotation = Quaternion.LookRotation(dirToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targerRotation, Time.deltaTime * 10f);
        }
    }

    void DoAttack()
    {
        lastAttackTime = Time.time;
        // 자식 클래스에서 공격 로직 수행
        Attack();
    }

    void DoPattern()
    {
        SpecialPattern();
    }

    void ChangeState(BossState newState)
    {
        currentState = newState;        
        animator.SetBool("IsWalking", newState == BossState.Chase);
    }


    public void GetStunned()
    {
        currentState = BossState.Stunned;
        isStunned = true;
        animator.SetTrigger("Stunned");
        Stunned();
        stunnedVFX.Play();
        StartCoroutine(RecoverFromStun());
    }

    IEnumerator RecoverFromStun()
    {
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
        animator.SetTrigger("StunEnd");
        stunnedVFX.Stop();
        currentState = BossState.Idle;        
    }
    

    protected virtual void Attack() { }
    protected virtual void SpecialPattern() { } 
    protected virtual void FinishPattern() { }
    protected virtual void Stunned() { }

    void OnDisable()
    {
        agent.enabled = false;
    }

}
