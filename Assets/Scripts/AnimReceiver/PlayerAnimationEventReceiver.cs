using UnityEngine;

public class PlayerAnimationEventReceiver : MonoBehaviour
{
    PlayerHealth playerHealth;
    PlayerSkill playerSkill;
    PlayerMovement playerMovement;
    PlayerUnderCC playerUnderCC;
    PlayerController playerController;

    void Awake()
    {
        playerHealth = GetComponentInParent<PlayerHealth>();
        playerSkill = GetComponentInParent<PlayerSkill>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerUnderCC = GetComponentInParent<PlayerUnderCC>();
        playerController = GetComponentInParent<PlayerController>();
    }
    public void EndHit()
    {
        playerHealth.EndHit();
    }
    public void EndUseSkill()
    {
        playerSkill.EndUseSkill();
    }

    public void PlayFootstep()
    {
        playerMovement.PlayFootstep();
    }

    public void EndPlayerFalldown()
    {
        playerUnderCC.EndPlayerFalldown();
    }

    public void EndAppliedCC()
    {
        playerController.EndCC();
    }
}
