using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class NavMonsterBase : MonoBehaviour
{
    [SerializeField] float attackRange = 10f;
    [SerializeField] float chaseRange = 50f;
    [SerializeField] float attackCooldown = 1.5f;
    [SerializeField] float stunDuration = 0.5f;

    AudioSource audioSource;
    Animator animator;
    PlayerController player;
    NavMeshAgent agent;
    protected bool isStunned = false;
    protected float lastAttackTime = 0f;
    MonsterState currentState = MonsterState.Idle;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();   
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();

        var health = GetComponent<EnemyHealth>();
        health.OnDamaged += HandleDamaged;
    }

    void Update()
    {
        // 스턴 상태라면 모든 로직 막음
        if (isStunned) return;
        
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        

        switch (currentState)
        {
            case MonsterState.Idle:
                // 거리 이내에 적 -> 추적
                if (distToPlayer <= chaseRange)
                    ChangeState(MonsterState.Chase);
                break;

            case MonsterState.Chase:
                // 추적                
                agent.SetDestination(player.transform.position);

                // 공격 사거리 이내 -> 공격
                if (distToPlayer <= attackRange)
                    ChangeState(MonsterState.Attack);

                // 거리 멀어짐 -> Idle
                else if (distToPlayer > chaseRange)
                    ChangeState(MonsterState.Idle);
                break;

            case MonsterState.Attack:
                // 추적 멈춤
                agent.ResetPath();

                // 거리 멀어짐 -> 추적
                if (distToPlayer > attackRange)
                    ChangeState(MonsterState.Chase);
                // 공격 시간 돌아오면 공격 수행
                else if (Time.time - lastAttackTime >= attackCooldown)
                    DoAttack();
                break;

        }        
    }

    void DoAttack()
    {
        UnityEngine.Debug.Log("DoAttack");
        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;
        // 자식 클래스에서 공격 로직 수행
        Attack();
    }

    void ChangeState(MonsterState newState)
    {
        currentState = newState;
        animator.SetBool("IsWalking", newState == MonsterState.Chase);
    }

    void HandleDamaged()
    {
        isStunned = true;
        currentState = MonsterState.Stunned;
        agent.ResetPath();
        animator.SetTrigger("Hit");
        StartCoroutine(RecoverFromStun());
    }

    IEnumerator RecoverFromStun()
    {
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
        currentState = MonsterState.Chase;
    }

    protected virtual void Attack() { }


    void OnDisable()
    {
        agent.enabled = false;    
    }
}
