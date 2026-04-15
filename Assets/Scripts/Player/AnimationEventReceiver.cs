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
}
