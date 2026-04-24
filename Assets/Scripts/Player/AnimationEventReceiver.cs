using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    //
    // Player
    //
    public void EndHit()
    {
        GetComponentInParent<PlayerHealth>().EndHit();
    }
    public void EndUseSkill()
    {
        GetComponentInParent<PlayerSkill>().EndUseSkill();
    }

    public void PlayFootstep()
    {
        GetComponentInParent<PlayerMovement>().PlayFootstep();
    }


    //
    // Portal
    //

    public void EndClose()
    {
        GetComponentInParent<EnemySpawnPortal>().EndClose();
    }

    // 
    // Normal Monster
    //
    public void ActivateTurtleAttack()
    {
        GetComponentInParent<EnemyTurtle>().ActivateTurtleAttack();
    }

    public void DeactivateTurtleAttack()
    {
        GetComponentInParent<EnemyTurtle>().DeactivateTurtleAttack();
    }

    public void ActivateSlimeAttack()
    {
        GetComponentInParent<EnemySlime>().ActivateSlimeAttack();
    }


    //
    // Boss Monster
    //
    public void ActivateDragonNormalAttack()
    {
        GetComponentInParent<BossDragon>().ActivateNormalAttack();
    }

    public void DeactivateDragonNormalAttack()
    {
        GetComponentInParent<BossDragon>().DeactivateNormalAttack();
    }

    public void ActivateDragonClawAttack()
    {
        GetComponentInParent<BossDragon>().ActivateClawAttack();
    }

    public void DeactivateDragonClawAttack()
    {
        GetComponentInParent<BossDragon>().DeactivateClawAttack();
    }

    public void ActivateDragonFireBreath()
    {
        GetComponentInParent<BossDragon>().ActivateFireBreath();
    }

    public void DeactivateDragonFireBreath()
    {
        GetComponentInParent<BossDragon>().DeactivateFireBreath();
    }
}
