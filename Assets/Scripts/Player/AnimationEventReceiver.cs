using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
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

    public void EndClose()
    {
        GetComponentInParent<EnemySpawnPortal>().EndClose();
    }

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
}
