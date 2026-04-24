using System.Runtime.CompilerServices;
using UnityEngine;

enum PatternType
{
    None,
    Fly,
    Block
}
    



public class BossDragon : BossMonsterBase
{
    [SerializeField] BoxCollider normalAttackHitbox;
    [SerializeField] BoxCollider clawAttackHitbox;
    [SerializeField] ParticleSystem fireBreathEffect;
    [SerializeField] BoxCollider fireBreathHitbox;

    bool isEntered = true;
    
    int specialPatternRandomInt = -1;
    PatternType currentPattern = PatternType.None;

    protected override void SpecialPattern()
    {
        if(isEntered)
        {
            specialPatternRandomInt = Random.Range(0, 4);
            isEntered = false;
        }        
        
        switch (specialPatternRandomInt)
        {
            case 0:
            case 1:
            case 2:
                FlyFlameAttack();
                break;
            case 3:
                BlockAttack();
                break;
        }
    }


    protected override void Attack()
    {
        int attPattern = Random.Range(0, 10);

        switch (attPattern)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                BasicAttack();
                break;                        
            case 5:
            case 6:
            case 7:
                ClawAttack();
                break;            
            case 8:
            case 9:
                FlameAttack();
                break;            
        }

    }

    protected override void FinishPattern()
    {
        switch (currentPattern)
        {
            case PatternType.Fly:
                animator.SetBool("IsFly", false);
                break;
            case PatternType.Block:
                animator.SetBool("IsBlock", false);
                break;
        }
        
        currentPattern = PatternType.None;
        isEntered = true;
        specialPatternRandomInt = -1;
    }

    protected override void Stunned()
    {
        clawAttackHitbox.enabled = false;
    }

    void BasicAttack()
    {
        animator.SetTrigger("BasicAttack");
    }

    void ClawAttack()
    {
        animator.SetTrigger("ClawAttack");
    }

    void FlameAttack()
    {
        animator.SetTrigger("FlameAttack");
    }

    void FlyFlameAttack()
    {
        currentPattern = PatternType.Fly;
        animator.SetBool("IsFly", true);
    }

    void BlockAttack()
    {
        currentPattern = PatternType.Block;
        animator.SetBool("IsBlock", true);
    }


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
}
