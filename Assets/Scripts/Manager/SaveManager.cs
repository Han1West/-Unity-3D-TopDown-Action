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

    void Start()
    {        
    }

    public void SaveGame()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        playerGuard = FindFirstObjectByType<PlayerGuard>();

        SaveData data = new SaveData();

        Debug.Log(playerHealth.gameObject.name);
        data.playerHp = playerHealth.currentHealth;

        
        Debug.Log(data.playerHp);

        data.playerParryPoint = playerGuard.currentParryPoint;
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
}
