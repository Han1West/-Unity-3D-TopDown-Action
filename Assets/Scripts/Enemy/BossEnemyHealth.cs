using UnityEngine;

public class BossEnemyHelath : EnemyHealth
{

    BossDragon boss;

    protected override void Awake()
    {
        base.Awake();
        boss = GetComponent<BossDragon>();
    }


    protected override void TakeDamage(int amount)
    {
        // block 중에 공격 -> 반격
        if(boss.isBlocking)
        {
            base.TakeDamage(0);
            boss.CounterAttack();
        }            
        else
            base.TakeDamage(amount);
    }

    protected override void ProcessDead()
    {
        
    }
}
