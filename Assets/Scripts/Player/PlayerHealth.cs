using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;    
    [Range(0f, 1f)]
    [SerializeField] float damageDeclineRate = 0.6f;
    [SerializeField] float hitDuration = 0.1f;
    [SerializeField] Material hitMaterial;

    SkinnedMeshRenderer[] renderers;
    MeshRenderer[] renderers2;
    Material[][] originMaterials;
    Material[][] originMaterials2;

    PlayerController playerController;
    PlayerGuard guard;
    Animator animator;
    int currentHealth = 0;
    int upperLayerIndex;    

    void Awake()
    {
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
        if (other.CompareTag("EnemyAttack"))
        {
            int damage = other.GetComponent<AttackHitbox>().damage;

            // 패리 성공
            if (guard.canParry)
                guard.SuccessParry();
            // 실패
            else
                TakeDamage(damage);
        }
        
    }

    void TakeDamage(int amount)
    {
        // 가드 중 
        if(guard.isGuarding)
            currentHealth -= Mathf.FloorToInt(amount * (1 - damageDeclineRate));        
        else
            currentHealth -= amount;
   
        StartCoroutine(HitFlash());
        
        Debug.Log("Current Health :" + currentHealth);

        if(currentHealth <= 0)
        {
            playerController.ChangeState(PlayerState.Dead);
            return;
        }


        animator.SetTrigger("Hit");
        animator.SetLayerWeight(upperLayerIndex, 1f);
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
}
