using UnityEngine;

public class CommonEnemyHealth : EnemyHealth
{
    [SerializeField] GameObject deadParticleVFX;

    protected override void ProcessDead()
    {
        // 죽음 파티클 재생
        Instantiate(deadParticleVFX, transform.position, Quaternion.identity);

        // 죽음 사운드 재생
        GetComponent<EnemyDeathSound>()?.Play();

        Destroy(gameObject);
    }
}
