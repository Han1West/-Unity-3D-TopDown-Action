using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField] TMP_Text heathText;
    [SerializeField] PlayerHealth playerHealth;

    void Start()
    {
        playerHealth.OnHealthChanged += UpdateHealthBar;
        UpdateHealthBar(100, 100);
    }

    void OnDestroy()
    {
        playerHealth.OnHealthChanged -= UpdateHealthBar;
    }

    public void UpdateHealthBar(int current, int max)
    {
        fillImage.fillAmount = (float)current / max;
        string newHealthText = max.ToString() + '/' + current.ToString();
        heathText.text = newHealthText; 
    }
}
