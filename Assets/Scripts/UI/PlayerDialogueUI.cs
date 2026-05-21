using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDialogueUI : MonoBehaviour
{
    [SerializeField] TMP_Text speakerName;    

    void Start()
    {
        UpdateName();
        GameManager.Instance.OnPlayerDataLoaded += UpdateName;                
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnPlayerDataLoaded -= UpdateName;
    }

    void UpdateName()
    {
        speakerName.text = GameManager.Instance.PlayerInGameName;
    }

}
