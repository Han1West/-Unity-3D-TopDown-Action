using UnityEngine;

public class EnemySlime : NavMonsterBase
{
    [SerializeField] GameObject slimeProejctile;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] AudioClip attackSFX;

    AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Attack()
    {
        audioSource.PlayOneShot(attackSFX, 0.3f);
    }

    public void ActivateSlimeAttack()
    {
        Instantiate(slimeProejctile, projectileSpawnPoint.position, transform.rotation);
    }
}
