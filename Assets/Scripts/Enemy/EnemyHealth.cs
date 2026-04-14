using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [Range(0f, 1f)]
    [SerializeField] float damageDeclineRate = 0.6f;
    [SerializeField] float hitDuration = 0.1f;
    [SerializeField] Material hitMaterial;
    [SerializeField] GameObject damageTextPrefab;
    [SerializeField] GameObject deadParticleVFX;
    [SerializeField] AudioClip hitSFX;

    SkinnedMeshRenderer[] renderers;
    MeshRenderer[] renderers2;
    Material[][] originMaterials;
    Material[][] originMaterials2;

    AudioSource audioSource;
    int currentHealth = 0;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        renderers2 = GetComponentsInChildren<MeshRenderer>();

        originMaterials = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; ++i)
        {
            originMaterials[i] = renderers[i].materials;
        }

        originMaterials2 = new Material[renderers2.Length][];
        for (int i = 0; i < renderers2.Length; ++i)
        {
            originMaterials2[i] = renderers2[i].materials;
        }
    }
    void Start()
    {
        currentHealth = maxHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("PlayerAttack"))
        {
            int damage = other.GetComponent<AttackHitbox>().damage;
            TakeDamage(damage);
        }
    }
    void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // 피격 이펙트
        StartCoroutine(HitFlash());        
        audioSource.PlayOneShot(hitSFX);

        // 피격 데미지 출력
        GameObject damageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        damageText.GetComponent<DamageText>().damageText.color = Color.black;
        damageText.GetComponent<DamageText>().damageText.text = amount.ToString();
        Debug.Log("Current Health :" + currentHealth);

        // 사망 처리 
        if(currentHealth <= 0)
        {
            ProcessDead(); 
        }
    }

    void ProcessDead()
    {
        Instantiate(deadParticleVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator HitFlash()
    {
        // Skin mesh renderers
        foreach (var renderer in renderers)
        {
            var hitMats = new Material[renderer.materials.Length];
            for (int i = 0; i < hitMats.Length; ++i)
                hitMats[i] = hitMaterial;

            renderer.materials = hitMats;
        }

        // Mesh renderers
        foreach (var renderer in renderers2)
        {
            var hitMats = new Material[renderer.materials.Length];
            for (int i = 0; i < hitMats.Length; ++i)
                hitMats[i] = hitMaterial;

            renderer.materials = hitMats;
        }


        yield return new WaitForSeconds(hitDuration);

        for (int i = 0; i < renderers.Length; ++i)
            renderers[i].materials = originMaterials[i];

        for (int i = 0; i < renderers2.Length; ++i)
            renderers2[i].materials = originMaterials2[i];
    }
}
