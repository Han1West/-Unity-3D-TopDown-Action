using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;    
    [Range(0f, 1f)]
    [SerializeField] float damageDeclineRate = 0.6f;

    PlayerGuard guard;
    int currentHealth = 0;
    int currentShield = 0;

    void Awake()
    {
        guard = GetComponent<PlayerGuard>();
    }
    void Start()
    {
        currentHealth = maxHealth;
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
        
        Debug.Log("Current Health :" + currentHealth);
    }
}
