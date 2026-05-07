using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum PatternType
{
    None,
    Block,
    Counter,
    FlameRain
}

public enum EventType
{ 
    None,
    Rage,
    Dead
}


public class BossDragon : BossMonsterBase
{
    [Header("MyHitBox")]
    [SerializeField] BoxCollider hitBox;

    [Header("AttackHitbox")]
    [SerializeField] BoxCollider normalAttackHitbox;
    [SerializeField] BoxCollider clawAttackHitbox;    
    [SerializeField] BoxCollider fireBreathHitbox;

    [Header("Effect")]
    [SerializeField] ParticleSystem fireBreathEffect;
    [SerializeField] ParticleSystem blockShieldEffect;
    [SerializeField] ParticleSystem rageEffect;
    [SerializeField] ParticleSystem rageEventEffect;
    [SerializeField] GameObject rageEventFallingObject;
    [SerializeField] GameObject disaapearEffect;

    [Header("Prefab")]
    [SerializeField] GameObject counterAttackObject;
    [SerializeField] GameObject pillarOfFireObject;

    [Header("Pillar Spawn")]
    [SerializeField] BoxCollider spawnArea;
    [SerializeField] float spawnCooldown = 4f;
    [SerializeField] float minDistance = 2f;
    [SerializeField] int maxTryCount = 30;    


    PatternType currentPattern = PatternType.None;
    EventType currentEvent = EventType.None;

    public bool isBlocking = false;
    public bool isRoaring = false;
    bool isCountering = false;
    bool isSpawningPillar;
    float nextTime = 0f;

    #region Override
    protected override void Attack() 
    {
        int rand = Random.Range(0, 10);

        ResetAttackTriggers();

        if (rand <= 4)
            animator.SetTrigger("BasicAttack");
        else if (rand <= 7)
            animator.SetTrigger("ClawAttack");
        else
            animator.SetTrigger("FlameAttack");

    }

    protected override void StartPattern()
    {
        int rand = Random.Range(0, 4);

        switch (rand)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                StartBlockPattern();
                break;
        }
    }

    protected override void PatternUpdate()
    {

    }

    protected override void EndPattern()
    {
        if(isBlocking)
        {            
            animator.SetBool("IsBlock", false);
            isBlocking = false;        
            blockShieldEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        currentPattern = PatternType.None;
    }

    protected override bool CanEndPattern()
    {
        return !isCountering;
    }

    protected override void StartEvent()
    {
        switch (currentEvent)
        {
            case EventType.None:                
                break;
            case EventType.Rage:
                StartRageEvent();
                break;
            case EventType.Dead:
                StartDeadEvent();
                break;
            default:
                break;
        }
    }

    protected override void EventUpdate()
    {
        switch (currentEvent)
        {
            case EventType.None:
                break;
            case EventType.Rage:
                break;
            case EventType.Dead:
                UpdateDeadEvent();
                break;
            default:
                break;
        }
    }

    protected override void EndEvent()
    {
        switch (currentEvent)
        {
            case EventType.None:
                break;
            case EventType.Rage:
                EndRageEvent();
                break;
            case EventType.Dead:
                EndDeadEvent(); 
                break;
            default:
                break;
        }        
    }

    protected override void OnStunned()
    {
        base.OnStunned();
        
        isSpawningPillar = false;
        isBlocking = false;
        isCountering = false;

        animator.SetBool("IsBlock", false);

        DisableAllHitbox();
        ResetAttackTriggers();        
    }

    protected override void EnterRageMode()
    {
        currentEvent = EventType.Rage;
        ChangeState(BossState.Event);               
    }

    protected override void UpdateRageLogic()
    {
        if (!isSpawningPillar)
            StartCoroutine(SpawnPillarOfFireCoroutine());
    }

    protected override void EnterDeadEvent()
    {
        currentEvent = EventType.Dead;
        ChangeState(BossState.Event);
    }

    #endregion

    #region Normal Attack

    public void AttackAnimationEnd()
    {
        DisableAllHitbox();
        EndAttack();
    }

    void ResetAttackTriggers()
    {
        animator.ResetTrigger("BasicAttack");
        animator.ResetTrigger("ClawAttack");
        animator.ResetTrigger("FlameAttack");
        animator.ResetTrigger("Counter");
    }
    #endregion

    #region Block Pattern

    void StartBlockPattern()
    {
        currentPattern = PatternType.Block;
        blockShieldEffect.Play();
        animator.SetBool("IsBlock", true);
    }
    
    #endregion

    #region Counter

    public void CounterAttack()
    {
        if (!isBlocking || isCountering)
            return;        

        StopAllCoroutines();

        isSpawningPillar = false;
        isBlocking = false;
        isCountering = true;

        currentPattern = PatternType.Counter;
        blockShieldEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        ResetAttackTriggers();
        
        animator.SetTrigger("Counter");
        animator.SetBool("IsBlock", false);        

        StartCoroutine(CounterDamageRoutine());
    }

    IEnumerator CounterDamageRoutine()
    {
        yield return new WaitForSeconds(2f);

        Instantiate(counterAttackObject, transform.position, Quaternion.identity);
    }

    public void CounterAnimationEnd()
    {
        isCountering = false;
        currentPattern = PatternType.None;

        EndAttack();
    }
    #endregion

    #region Event
    void StartRageEvent()
    {
        // 스탯 변화
        attackCooldown -= 1f;
        patternCheckCooldown -= 1f;
        agent.speed = 10f;
        agent.angularSpeed = 180f;
        agent.acceleration = 15f;


        // 이벤트 동안 히트 박스 비활성화
        if (hitBox.enabled)
            hitBox.enabled = false;

        rageEventEffect.Play();

        animator.SetTrigger("EnterRageMode");
        StartCoroutine(SpawnFallingRockCoroutine());
    }

    void EndRageEvent()
    {        
        currentEvent = EventType.None;
        rageEventEffect.Stop();
        rageEffect.Play();

        isRage = true;

        if (!hitBox.enabled)
            hitBox.enabled = true;
    }

    public void EndLand()
    {
        if (currentState == BossState.Event && currentEvent == EventType.Rage)
        {            
            isBusy = false;            
            ChangeState(BossState.Idle);
        }
    }

    void StartDeadEvent()
    {
        if (hitBox.enabled)
            hitBox.enabled = false;

        StopAllCoroutines();
        DisableAllHitbox();
        DeactviatedAllEffect();
         
        animator.SetTrigger("Dead");
    }

    void UpdateDeadEvent()
    {
        if(Time.time >= nextTime)
        {
            rageImpulseSource.GenerateImpulse(0.2f);
            nextTime = Time.time + 2f;
        }
        
    }

    void EndDeadEvent()
    {
        currentEvent = EventType.None;
        StartCoroutine(DestroyBossCoroutine());
    }

    public void EndDie()
    {
        if (currentState == BossState.Event && currentEvent == EventType.Dead)
        {
            isBusy = false;
            ChangeState(BossState.Dead);
        }
    }

    #endregion

    #region Hitbox Event

    public void ActivateNormalAttack()
    {        
        normalAttackHitbox.enabled = true;
    }

    public void DeactivateNormalAttack()
    {
        normalAttackHitbox.enabled = false;
    }

    public void ActivateClawAttack()
    {        
        clawAttackHitbox.enabled = true;
    }

    public void DeactivateClawAttack()
    {
        clawAttackHitbox.enabled = false;
    }

    public void ActivateFireBreath()
    {        
        fireBreathEffect.Play();
        fireBreathHitbox.enabled = true;
    }

    public void DeactivateFireBreath()
    {
        fireBreathEffect.Stop();
        fireBreathHitbox.enabled = false;
    }

    void DisableAllHitbox()
    {
        normalAttackHitbox.enabled = false;
        clawAttackHitbox.enabled = false;
        fireBreathHitbox.enabled = false;
    }

    void DeactviatedAllEffect()
    {
        fireBreathEffect.Stop();        
        blockShieldEffect.Stop();
        rageEffect.Stop();
        stunnedVFX.Stop();
    }
    #endregion


    IEnumerator SpawnFallingRockCoroutine()
    {
        float waitTime = 10f;
        float timer = 0f;

        while (!isRoaring && timer < waitTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        while (isRoaring)
        {
            rageImpulseSource.GenerateImpulse(0.2f);

            int spawnCount = Random.Range(3, 7);
            List<Vector3> spawnedPositions = new List<Vector3>();            

            for (int i = 0; i < spawnCount; ++i)
            {
                bool found = false;

                for (int tryCount = 0; tryCount < maxTryCount; tryCount++)
                {
                    Vector3 newSpawnPoint = GetRandomPointInArea();
                    newSpawnPoint.y = 20f;

                    if (IsValidSpawnPoint(newSpawnPoint, spawnedPositions))
                    {
                        
                        Instantiate(rageEventFallingObject, newSpawnPoint, Quaternion.identity);
                        spawnedPositions.Add(newSpawnPoint);
                        found = true;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator SpawnPillarOfFireCoroutine()
    {
        isSpawningPillar = true;
        
        while(isRage)
        {
            Debug.Log("Spawn Pillar");

            // 플레이어 위치에 일정 시간 마다 불기둥 소환
            Vector3 spawnPoint = player.transform.position;            

            Instantiate(pillarOfFireObject, spawnPoint, Quaternion.identity);
    
            yield return new WaitForSeconds(spawnCooldown);
        }        
    }

    IEnumerator DestroyBossCoroutine()
    {
        yield return new WaitForSeconds(3f);

        Instantiate(disaapearEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    Vector3 GetRandomPointInArea()
    {
        Bounds bounds = spawnArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = 0;

        return new Vector3(x, y, z);
    }

    bool IsValidSpawnPoint(Vector3 newPoint, List<Vector3> existing)
    {
        foreach (Vector3 point in existing)
        {
            if (Vector3.Distance(point, newPoint) < minDistance)
                return false;
        }
        return true;
    }
}
