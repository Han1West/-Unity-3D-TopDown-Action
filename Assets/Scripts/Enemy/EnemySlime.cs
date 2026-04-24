using UnityEngine;

public class EnemySlime : NavMonsterBase
{
    [SerializeField] GameObject slimeProejctile;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] AudioClip attackSFX;

    protected override void Attack()
    {
        animator.SetTrigger("Attack");
        audioSource.PlayOneShot(attackSFX, 0.3f);
    }

    public void ActivateSlimeAttack()
    {
        Instantiate(slimeProejctile, projectileSpawnPoint.position, transform.rotation);
    }
}
