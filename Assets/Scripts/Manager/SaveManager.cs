using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public int playerHp;
    public int playerParryPoint;
    public float playTime;
    public int totalKill;
    public string sceneName;
}

public class SaveManager : MonoBehaviour
{    
    public static SaveManager Instance;
    public bool IsContinueLoading { get; set; }

    PlayerHealth playerHealth;
    PlayerGuard playerGuard;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }        
    }

    public void SaveGame()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        playerGuard = FindFirstObjectByType<PlayerGuard>();

        SaveData data = new SaveData();
        
        if(playerHealth && playerGuard)
        {
            data.playerHp = playerHealth.currentHealth;
            data.playerParryPoint = playerGuard.currentParryPoint;
        }
        data.playTime = GameManager.Instance.GetPlayTime();
        data.totalKill = GameManager.Instance.GetKillCount();        
        data.sceneName = SceneManager.GetActiveScene().name;

        string json = JsonUtility.ToJson(data);

        PlayerPrefs.SetString("SaveData", json);
        PlayerPrefs.Save();
    }

    public SaveData LoadGame()
    {
        if (!PlayerPrefs.HasKey("SaveData"))
            return null;

        string json = PlayerPrefs.GetString("SaveData");
        return JsonUtility.FromJson<SaveData>(json);
    }

    public void DeleteSave()
    {
        if(PlayerPrefs.HasKey("SaveData"))
        {
            PlayerPrefs.DeleteKey("SaveData");
            PlayerPrefs.Save();
        }
    }
}
