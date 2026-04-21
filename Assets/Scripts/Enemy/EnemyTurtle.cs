using UnityEngine;

public class EnemyTurtle : NavMonsterBase
{
    [SerializeField] BoxCollider attackHitbox;
    [SerializeField] AudioClip attackSFX;

    AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Attack()
    {
        
    }

    public void ActivateTurtleAttack()
    {
        attackHitbox.enabled = true;
        audioSource.PlayOneShot(attackSFX, 0.3f);
    }

    public void DeactivateTurtleAttack()
    {
        attackHitbox.enabled = false;
    }
}
