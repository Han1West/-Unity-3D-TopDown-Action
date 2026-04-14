using System;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float[] attackDuration;
    [SerializeField] float resetTime = 1f;
    [SerializeField] ParticleSystem[] attackPartciles;
    [SerializeField] GameObject attackHitbox;
    [SerializeField] AudioClip[] attackSFX;
    

    PlayerController playerController;
    Animator animator;
    AudioSource audioSource;
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
        audioSource = GetComponent<AudioSource>();
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
        attackHitbox.SetActive(false);
        isAttacking = true;
        attackTimer = attackDuration[attackIndex];
        animator.SetBool("IsAttacking", true);
        animator.SetInteger("AttackIndex", attackIndex);
        ProcessAttack(attackIndex);

        attackIndex = (attackIndex + 1) % maxAttackIndex;
    }

    public void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
        attackHitbox.SetActive(false);        
    }

    public void UpdateAttack()
    {
        if (!isAttacking) return;
                    
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {            
            playerController.OnAttackFinished();
        }
    }

    public void QueueNextAttack()
    {
        if (isAttacking)
            nextAttackQueued = true;
    }

    public bool ConsumeNextAttackQueued()
    {
        if(nextAttackQueued)
        {
            nextAttackQueued = false;
            return true;
        }
        return false;
    }

    void ProcessAttack(int index)
    {
        // 공격의 순서가 초기화 되는 시간
        resetTimer = resetTime;

        // 공격 이펙트 소환
        attackPartciles[index].Play();
        attackHitbox.SetActive(true);
        audioSource.PlayOneShot(attackSFX[index], 0.5f);        
    }

    void ResetAttackOrder()
    {
        attackIndex = 0;
    }
}
