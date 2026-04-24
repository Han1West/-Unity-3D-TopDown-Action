using UnityEngine;

public class EnemyTurtle : NavMonsterBase
{
    [SerializeField] BoxCollider attackHitbox;
    [SerializeField] AudioClip attackSFX;


    protected override void Attack()
    {
        animator.SetTrigger("Attack");
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
