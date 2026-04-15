using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;    
    [Range(0f, 1f)]
    [SerializeField] float damageDeclineRate = 0.6f;
    [SerializeField] float hitDuration = 0.1f;
    [SerializeField] Material hitMaterial;
    [SerializeField] ParticleSystem healVFX;
    [SerializeField] GameObject damageTextPrefab;

    [SerializeField] AudioClip healingSFX;

    SkinnedMeshRenderer[] renderers;
    MeshRenderer[] renderers2;
    Material[][] originMaterials;
    Material[][] originMaterials2;

    AudioSource audioSource;
    PlayerController playerController;
    PlayerGuard guard;
    Animator animator;
    int currentHealth = 0;
    int upperLayerIndex;

    public event Action<int, int> OnHealthChanged;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
        guard = GetComponent<PlayerGuard>();
        animator = GetComponentInChildren<Animator>();

        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        renderers2 = GetComponentsInChildren<MeshRenderer>();

        originMaterials = new Material[renderers.Length][];
        for(int i =0; i < renderers.Length; ++i)
        {
            originMaterials[i] = renderers[i].materials;
        }

        originMaterials2= new Material[renderers2.Length][];
        for(int i =0; i < renderers2.Length; ++i)
        {
            originMaterials2[i] = renderers2[i].materials;
        }
    }
    void Start()
    {
        currentHealth = maxHealth;
        upperLayerIndex = animator.GetLayerIndex("Upper Layer");
    }

    void OnTriggerEnter(Collider other)
    {
        // 데미지
        if (other.CompareTag("EnemyAttack"))
        {
            Vector3 hitDirection = other.transform.position - transform.position;
            hitDirection.Normalize();

            int damage = other.GetComponent<AttackHitbox>().damage;

            // 패리 성공
            if (guard.canParry)
                guard.SuccessParry(hitDirection);
            // 실패
            else
                TakeDamage(damage, hitDirection);
        }

        // 힐
        if (other.CompareTag("Heal"))
        {            
            int heal = other.GetComponent<HealPickup>().heal;

            // 현재 체력이 최대체력이 아닐때만 상호작용
            if(currentHealth < maxHealth)
            {
                TakeHeal(heal);
                other.GetComponent<HealPickup>().DestroyThis();
            }                
        }        
    }

    void TakeDamage(int amount, Vector3 hitDirection)
    {
        int takenDamage = amount;

        // 가드 중 
        if(guard.isGuarding)
        {
            // 체력 감소 비율 감쇄
            takenDamage = Mathf.FloorToInt(amount * (1 - damageDeclineRate));
            currentHealth -= takenDamage;


            // 플레이어 공격 받은 방향으로 회전
            hitDirection.y = 0;
            if (hitDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(hitDirection);
                transform.rotation = targetRotation; 
            }
        }            
        else
            currentHealth -= takenDamage;

        GameObject damageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        damageText.GetComponent<DamageText>().damageText.text = takenDamage.ToString();
   
        // Hit 판정 피격 표시 
        StartCoroutine(HitFlash());
        
        Debug.Log("Current Health :" + currentHealth);

        // 피가 0 이하면 사망처리
        if(currentHealth <= 0)
        {
            playerController.ChangeState(PlayerState.Dead);
            return;
        }

        // 가드중이 아닐 때만 Hit 애니메이션 트리거 발동
        if(!guard.isGuarding)
        {
            animator.SetTrigger("Hit");
            animator.SetLayerWeight(upperLayerIndex, 1f);
        }

        // 이벤트 발생
        OnHealthChanged(currentHealth, maxHealth);
    }

    public void EndHit()
    {
        animator.SetLayerWeight(upperLayerIndex, 0f);
    }

    private IEnumerator HitFlash()
    {
        // Skin mesh renderers
        foreach(var renderer in renderers)
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

    void TakeHeal(int amount)
    {
        healVFX.Play();
        audioSource.PlayOneShot(healingSFX);

        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        GameObject damageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        damageText.GetComponent<DamageText>().damageText.color = Color.green;
        damageText.GetComponent<DamageText>().damageText.text = amount.ToString();

        Debug.Log("Current Health :" + currentHealth);

        // 이벤트 발생
        OnHealthChanged(currentHealth, maxHealth);
    }
}
