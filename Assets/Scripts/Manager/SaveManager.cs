using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public string playerName;
    public int playerHp;
    public int playerParryPoint;
    public float playTime;
    public int totalKill;
    public int tryCount;
    public string sceneName;
    public bool isSucceed = false;
}

public class SaveManager : MonoBehaviour
{    
    public static SaveManager Instance;
    public bool IsContinueLoading { get; set; }
    public bool IsRetryLoading { get; set; }

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

        data.playerName = GameManager.Instance.PlayerInGameName;

        if (playerHealth && playerGuard)
        {
            data.playerHp = playerHealth.currentHealth;
            data.playerParryPoint = playerGuard.currentParryPoint;
        }
        
        data.playTime = GameManager.Instance.GetPlayTime();
        data.totalKill = GameManager.Instance.GetKillCount();
        data.tryCount = GameManager.Instance.GetTryCount();
        data.isSucceed = GameManager.Instance.IsSucceed;
        data.sceneName = SceneManager.GetActiveScene().name;        
        

        string json = JsonUtility.ToJson(data);

        if(SceneManager.GetActiveScene().name == "Stage 1")
        {
            // 다른 모든 세이브 삭제
            DeleteSave();
            PlayerPrefs.SetString("Start SaveData", json);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetString("SaveData", json);
            PlayerPrefs.Save();
        }
            
    }

    public void SaveSucceedData()
    {
        string json = PlayerPrefs.GetString("Start SaveData");
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        data.isSucceed = true;
        data.tryCount = 0;

        json = JsonUtility.ToJson(data);

        PlayerPrefs.SetString("Start SaveData", json);
        PlayerPrefs.Save();
    }

    public SaveData LoadGame()
    {
        if (!PlayerPrefs.HasKey("SaveData"))
            return null;

        string json = PlayerPrefs.GetString("SaveData");
        return JsonUtility.FromJson<SaveData>(json);
    }

    public SaveData LoadSavedStartGame()
    {
        if (!PlayerPrefs.HasKey("Start SaveData"))
            return null;

        string json = PlayerPrefs.GetString("Start SaveData");
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
