using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Delay")]
    [SerializeField] float[] attackDuration;
    [SerializeField] float resetTime = 1f;

    [Header("Attack Effect")]
    [SerializeField] ParticleSystem[] attackPartciles;
    [SerializeField] BoxCollider attackHitbox;
    [SerializeField] AudioClip[] attackSFX;
    [SerializeField] AudioClip[] attackVoiceSFX;

    [Header("Attack Move")]
    [SerializeField] float[] attackMoveDistance;
    [SerializeField] float attackMoveSpeed = 15f;

    PlayerController playerController;
    CharacterController characterController;
    Animator animator;
    AudioSource audioSource;
    Camera mainCamera;

    int attackIndex = 0;
    int maxAttackIndex = 3;
    float attackTimer = 0f;
    float resetTimer = 0f;
    bool isAttacking = false;
    bool nextAttackQueued = false;
    Vector3 attackMoveDirection;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();

        mainCamera = Camera.main;
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
        RotateToMouseDirection();

        StartCoroutine(AttackMoveRoutine(attackMoveDistance[attackIndex]));

        attackHitbox.enabled = false;        
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
        attackHitbox.enabled = false;        
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
        attackHitbox.enabled = true;

        // 소리 재생
        audioSource.PlayOneShot(attackVoiceSFX[index], 0.5f);
        audioSource.PlayOneShot(attackSFX[index], 0.5f);        
    }

    void ResetAttackOrder()
    {
        attackIndex = 0;
    }

    void RotateToMouseDirection()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Plane plane = new Plane(Vector3.up, transform.position);

        if(plane.Raycast(ray, out float distance))
        {
            Vector3 mouseWorldPos = ray.GetPoint(distance);

            Vector3 direction = mouseWorldPos - transform.position;

            direction.y = 0f;

            if(direction.sqrMagnitude > 0.01f)
            {
                direction.Normalize();

                attackMoveDirection = direction;

                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

    IEnumerator AttackMoveRoutine(float movedistance)
    {
        float moveDistance = 0f;

        while(moveDistance < movedistance)
        {
            float move = attackMoveSpeed * Time.deltaTime;

            Vector3 moveVector = attackMoveDirection * move;

            characterController.Move(moveVector);

            moveDistance += move;

            yield return null;
        }
    }
}
