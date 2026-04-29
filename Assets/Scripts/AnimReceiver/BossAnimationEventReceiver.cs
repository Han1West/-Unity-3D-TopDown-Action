using UnityEngine;

public class BossAnimationEventReceiver : MonoBehaviour
{
    BossDragon boss;

    private void Awake()
    {
        boss = GetComponentInParent<BossDragon>();
    }

    public void ActivateDragonNormalAttack()
    {
        boss.ActivateNormalAttack();
    }

    public void DeactivateDragonNormalAttack()
    {
        boss.DeactivateNormalAttack();
    }

    public void ActivateDragonClawAttack()
    {
        boss.ActivateClawAttack();
    }

    public void DeactivateDragonClawAttack()
    {
        boss.DeactivateClawAttack();
    }

    public void ActivateDragonFireBreath()
    {
        boss.ActivateFireBreath();
    }

    public void DeactivateDragonFireBreath()
    {
        boss.DeactivateFireBreath();
    }

    public void StartBlock()
    {
        boss.isBlocking = true;
    }

    public void EndAttack()
    {
        boss.AttackAnimationEnd();
    }

    public void ReflectionEnd()
    {
        boss.CounterAnimationEnd();
    }
}
