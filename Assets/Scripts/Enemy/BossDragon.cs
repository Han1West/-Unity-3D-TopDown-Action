using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public enum PatternType
{
    None,
    Block,
    Counter,
    FlameRain
}

public class BossDragon : BossMonsterBase
{
    [Header("Hitbox")]
    [SerializeField] BoxCollider normalAttackHitbox;
    [SerializeField] BoxCollider clawAttackHitbox;    
    [SerializeField] BoxCollider fireBreathHitbox;

    [Header("Effect")]
    [SerializeField] ParticleSystem fireBreathEffect;

    [Header("Prefab")]
    [SerializeField] GameObject counterAttackObject;
    [SerializeField] GameObject pillarOfFireObject;

    [Header("Pillar Spawn")]
    [SerializeField] BoxCollider spawnArea;
    [SerializeField] float spawnCooldown = 4f;
    [SerializeField] float minDistance = 2f;
    [SerializeField] int maxTryCount = 30;    

    
    PatternType currentPattern = PatternType.None;

    public bool isBlocking = false;
    bool isCountering = false;

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
            Debug.Log("End Pattern");
            animator.SetBool("IsBlock", false);
            isBlocking = false;
        }

        currentPattern = PatternType.None;
    }

    protected override bool CanEndPattern()
    {
        return !isCountering;
    }

    protected override void OnStunned()
    {
        StopAllCoroutines();

        isBlocking = false;
        isCountering = false;

        animator.SetBool("IsBlock", false);

        DisableAllHitbox();
        ResetAttackTriggers();        
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

        animator.SetBool("IsBlock", true);
    }
    
    #endregion

    #region Counter

    public void CounterAttack()
    {
        if (!isBlocking || isCountering)
            return;

        Debug.Log("Do Counter Attack");

        StopAllCoroutines();

        isBlocking = false;
        isCountering = true;

        currentPattern = PatternType.Counter;

        
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
    #endregion


    IEnumerator SpawnPillarOfFireCoroutine()
    {
        while(false)
        {
            int spawnCount = Random.Range(4, 7);            
            List<Vector3> spawnedPositions = new List<Vector3>();

            for(int i = 0; i < spawnCount; ++i)
            {
                bool found = false;

                for(int tryCount = 0; tryCount < maxTryCount; tryCount++)
                {
                    Vector3 newSpawnPoint = GetRandomPointInArea();

                    if (IsValidSpawnPoint(newSpawnPoint, spawnedPositions))
                    {
                        Instantiate(pillarOfFireObject, newSpawnPoint, Quaternion.identity);
                        spawnedPositions.Add(newSpawnPoint);
                        found = true;
                        break;
                    }
                }
            }            
            yield return new WaitForSeconds(spawnCooldown);
        }        
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
