using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [Range(0f, 1f)]
    [SerializeField] float damageDeclineRate = 0.6f;
    
    int currentHealth = 0;    

    void Start()
    {
        currentHealth = maxHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        if (other.CompareTag("PlayerAttack"))
        {
            int damage = other.GetComponent<AttackHitbox>().damage;
            TakeDamage(damage);
        }

    }

    void TakeDamage(int amount)
    {
        currentHealth -= amount;

        Debug.Log("Current Health :" + currentHealth);
    }
}
