using System.Collections;
using UnityEngine;

public class CommonEnemyHealth : EnemyHealth
{
    [SerializeField] GameObject deadParticleVFX;
    [SerializeField] float knockbackForce = 3f;
    [SerializeField] float knockbackDuration = 0.1f;

    Rigidbody rb;
    Coroutine knockbackRoutine;
    PlayerController player;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();

        player = FindFirstObjectByType<PlayerController>();
    }

    protected override void TakeDamage(int amount)
    {
        // 피격당한 방향으로 넉백
        if(player)
        {
            Vector3 knockbackDir = (transform.position - player.gameObject.transform.position).normalized;


            if (knockbackRoutine != null)
                StopCoroutine(knockbackRoutine);

            knockbackRoutine = StartCoroutine(KnockbackRoutine(knockbackDir));
        }

        base.TakeDamage(amount);
    }

    protected override void ProcessDead()
    {
        // 죽음 파티클 재생
        Instantiate(deadParticleVFX, transform.position, Quaternion.identity);

        // 죽음 사운드 재생
        GetComponent<EnemyDeathSound>()?.Play();

        Destroy(gameObject);
    }

    IEnumerator KnockbackRoutine(Vector3 dir)
    {
        float timer = 0f;

        while(timer < knockbackDuration)
        {
            rb.MovePosition(rb.position + dir * knockbackForce * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }
    }
}
