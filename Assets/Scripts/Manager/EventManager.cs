using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerInfo
{
    public int currentHealth = 0;
    public int currentParryPoint = 0;
}

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public bool IsTransPlayerInfo { get; private set; } = false;

    PlayerHealth playerHealth;
    PlayerGuard playerGuard;


    private void Awake()
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

    public void LoadNextScene()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        playerGuard = FindFirstObjectByType<PlayerGuard>();
     
        // 현재 레벨에 플레이어 존재
        if(playerHealth && playerGuard)
        {
            PlayerInfo curInfo = new PlayerInfo();

            curInfo.currentHealth = playerHealth.currentHealth;
            curInfo.currentParryPoint = playerGuard.currentParryPoint;

            IsTransPlayerInfo = true;

            GameManager.Instance.SaveTempPlayerInfo(curInfo);
        }        
            

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        SceneManager.LoadScene(nextScene);
    }
    
    public void LoadTitle()
    {
        IsTransPlayerInfo = false;
        SceneManager.LoadScene(0);
    }

    public void LoadSavedStartScene()
    {
        IsTransPlayerInfo = false;
        
        SaveData data = SaveManager.Instance.LoadSavedStartGame();

        SaveManager.Instance.IsRetryLoading = true;        
        if (data != null)
            SceneManager.LoadScene(data.sceneName);
    }
}
