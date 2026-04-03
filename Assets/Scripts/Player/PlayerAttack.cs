using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float[] attackDuration;
    [SerializeField] float resetTime = 1f;

    PlayerController playerController;
    Animator animator;
    int attackIndex = 0;
    int maxAttackIndex = 3;
    float attackTimer = 0f;
    float resetTimer = 0f;
    bool isAttacking = false;
    bool nextAttackQueued = false;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // 공격이 중단되도 일정 시간이 지나야 공격 순서가 초기화 된다.
        if (!isAttacking)
        {
            resetTimer -= Time.deltaTime;

            if (resetTimer < 0f)
            {
                ResetAttackOrder();
            }
        }
    }

    public void StartAttack()
    {
        isAttacking = true;
        attackTimer = attackDuration[attackIndex];
        animator.SetBool("IsAttacking", true);
        animator.SetInteger("AttackIndex", attackIndex);
        ProcessAttack(attackIndex);
    }

    public void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("IsAttacking", false);

        if(nextAttackQueued)
        {
            nextAttackQueued = false;
            // 0, 1, 2, 0 순환
            attackIndex = (attackIndex + 1) % maxAttackIndex;
            StartAttack();            
        }
        else
        {
            playerController.ChangeState(PlayerState.Idle);
        }
    }

    public void UpdateAttack()
    {
        if (!isAttacking) return;
                    
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {   
            EndAttack();
        }
    }

    public void QueueNextAttack()
    {
        if (isAttacking)
        {
            nextAttackQueued = true;
            Debug.Log("Queued!");
        }
        Debug.Log("Cnat Queued!");
    }

    void ProcessAttack(int index)
    {
        // 공격의 순서가 초기화 되는 시간
        resetTimer = resetTime;        
    }

    void ResetAttackOrder()
    {
        attackIndex = 0;
    }
}
