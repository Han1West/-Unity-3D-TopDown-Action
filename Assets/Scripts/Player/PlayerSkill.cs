using UnityEngine;
using UnityEngine.XR;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField] GameObject skillObject;
    [SerializeField] float skillDelayTime = 1f;

    Animator animator;
    PlayerController playerController;
    public int needSkillPoint = 2;
    float delayTimer = 0f;
    bool playAnimation = false;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
    }


    public void StartSkill()
    {
       Instantiate(skillObject, transform.position, Quaternion.identity);
    }

    public void EndSkill()
    {
        delayTimer = 0f;
        playAnimation = false;
    }

    public void UpdateSkill()
    {
        delayTimer += Time.deltaTime;
        if (delayTimer > skillDelayTime && !playAnimation)
        {
            animator.SetTrigger("UseSKill");
            playAnimation = true;
        }
    }

    public void EndUseSkill()
    {
        playerController.ChangeState(PlayerState.Idle);
    }
}
