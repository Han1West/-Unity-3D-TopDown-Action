using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class BossAnimationEventReceiver : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] AudioClip fireBreathSFX;
    [SerializeField] AudioClip normalAttackSFX;
    [SerializeField] AudioClip clawAttackSFX;
    [SerializeField] AudioClip reflectionRoarSFX;
    
    [SerializeField] AudioClip rageRoarSFX;
    [SerializeField] AudioClip defendSFX;
    [SerializeField] AudioClip rageFireSFX;

    [SerializeField] AudioClip stunnedSFX;
    [SerializeField] AudioClip deadSFX;

    [SerializeField] AudioClip landSFX;
    [SerializeField] AudioClip[] wingflipSFX;
    [SerializeField] AudioClip[] footstepsSFX;
    

    BossDragon boss;
    AudioSource audioSource;


    private void Awake()
    {
        boss = GetComponentInParent<BossDragon>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    public void ActivateDragonNormalAttack()
    {
        audioSource.PlayOneShot(normalAttackSFX);
        boss.ActivateNormalAttack();
    }

    public void DeactivateDragonNormalAttack()
    {
        boss.DeactivateNormalAttack();
    }

    public void ActivateDragonClawAttack()
    {
        audioSource.PlayOneShot(clawAttackSFX);
        boss.ActivateClawAttack();
    }

    public void DeactivateDragonClawAttack()
    {
        boss.DeactivateClawAttack();
    }

    public void ActivateDragonFireBreath()
    {
        audioSource.PlayOneShot(fireBreathSFX);
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

    public void StartRageRoar()
    {
        audioSource.PlayOneShot(rageRoarSFX);
        boss.isRoaring = true;
    }

    public void EndRageRoar()
    {        
        boss.isRoaring = false;
    }

    public void EndLand()
    {
        audioSource.clip = rageFireSFX;
        audioSource.loop = true;
        audioSource.Play();
        audioSource.PlayOneShot(landSFX);
        boss.EndLand();
    }

    public void StartDie()
    {
        // ∑Á«¡ ¿Áª˝ ∏ÿ√„
        audioSource.Stop();
        audioSource.PlayOneShot(deadSFX);
    }

    public void EndDie()
    {
        boss.EndDie();
    }

    public void StartReflectionRoar()
    {
        audioSource.PlayOneShot(reflectionRoarSFX);
    }

    public void StartStunned()
    {
        audioSource.PlayOneShot(stunnedSFX);
    }

    public void StartSwingDefend()
    {
        audioSource.PlayOneShot(defendSFX);
    }

    public void StartWingFlip()
    {
        int i = Random.Range(0, wingflipSFX.Length);
        audioSource.PlayOneShot(wingflipSFX[i]);
    }

    public void StartFootsteps()
    {
        int i = Random.Range(0, footstepsSFX.Length);
        audioSource.PlayOneShot(footstepsSFX[i], 0.2f);
    }
}
