using UnityEngine;
using UnityEngine.XR;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField] GameObject skillObject;
    [SerializeField] float skillDelayTime = 1f;
    [SerializeField] AudioClip chargeSFX;
    [SerializeField] AudioClip fireSFX;
    [SerializeField] AudioClip skillVoiceSFX;

    AudioSource audioSource;
    Animator animator;
    PlayerController playerController;
    public int needSkillPoint = 2;
    float delayTimer = 0f;
    bool playAnimation = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
    }


    public void StartSkill()
    {
        Instantiate(skillObject, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(chargeSFX, 0.5f);
        audioSource.PlayOneShot(skillVoiceSFX);
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
            audioSource.PlayOneShot(fireSFX, 0.5f);            
            playAnimation = true;
        }
    }

    public void EndUseSkill()
    {
        playerController.ChangeState(PlayerState.Idle);
    }
}
