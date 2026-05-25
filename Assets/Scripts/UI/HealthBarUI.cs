using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{    
    [Header("Player")]
    [SerializeField] TMP_Text playerHealthText;
    [SerializeField] Image playerFillImage;
    [SerializeField] PlayerHealth playerHealth;

    [Header("Boss")]
    [SerializeField] GameObject bossHealthUI;
    [SerializeField] TMP_Text bossHealthText;
    [SerializeField] Image bossFillImage;
    [SerializeField] TMP_Text bossNameText;
    [SerializeField] EnemyHealth bossHealth;

    void Start()
    {      
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdatePlayerHealthBar;
            UpdatePlayerHealthBar(playerHealth.maxHealth, playerHealth.maxHealth);
        }
        if (bossHealth != null)
        {
            bossHealth.OnHealthChanged += UpdateBossHealthBar;            

            UpdateBossHealthBar(bossHealth.maxHealth, bossHealth.maxHealth);
            UpdateBossName(bossHealth.enemyUIName);
        }
        else
        {
            bossHealthUI.SetActive(false);
        }       
    }

    void OnDestroy()
    {
        if (playerHealth)
            playerHealth.OnHealthChanged -= UpdatePlayerHealthBar;
        else if (bossHealth)
            bossHealth.OnHealthChanged -= UpdateBossHealthBar;

    }

    public void UpdatePlayerHealthBar(int current, int max)
    {
        playerFillImage.fillAmount = (float)current / max;
        string newHealthText = max.ToString() + '/' + current.ToString();
        playerHealthText.text = newHealthText; 
    }

    public void UpdateBossHealthBar(int current, int max)
    {
        bossFillImage.fillAmount = (float)current / max;
        string newHealthText = max.ToString() + '/' + current.ToString();
        bossHealthText.text = newHealthText;

        if (current < 0)
            bossHealthUI.SetActive(false);
    }

    public void UpdateBossName(string name)
    {        
        bossNameText.text = name;
    }
}
