using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField] TMP_Text heathText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] EnemyHealth bossHealth;

    void Start()
    {
        if(playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(playerHealth.maxHealth, playerHealth.maxHealth);
        }
        else if (bossHealth != null)
        {
            bossHealth.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(bossHealth.maxHealth, bossHealth.maxHealth);
            UpdateName(bossHealth.enemyUIName);
        }
        
    }

    void OnDestroy()
    {
        if (playerHealth)
            playerHealth.OnHealthChanged -= UpdateHealthBar;
        else if (bossHealth)
            bossHealth.OnHealthChanged -= UpdateHealthBar;

    }

    public void UpdateHealthBar(int current, int max)
    {
        fillImage.fillAmount = (float)current / max;
        string newHealthText = max.ToString() + '/' + current.ToString();
        heathText.text = newHealthText; 
    }

    public void UpdateName(string name)
    {
        nameText.text = name;
    }
}
