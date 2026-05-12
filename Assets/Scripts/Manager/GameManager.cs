using UnityEngine;

public class GameManager : MonoBehaviour
{    
    PlayerDead playerDead;

    bool isPlaying = false;

    float playTime = 0;
    int killCount = 0;

    public static GameManager Instance;

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
        playerDead = FindFirstObjectByType<PlayerDead>();
        isPlaying = true;
        EnemyHealth.OnEnemyDead += HandleEnemyDead;
        playerDead.OnPlayerDead += HandlePlayerDead;
    }

    void OnDestroy()
    {
        EnemyHealth.OnEnemyDead -= HandleEnemyDead;
        playerDead.OnPlayerDead -= HandlePlayerDead;
    }

    void Update()
    {
        if(isPlaying)        
            playTime += Time.deltaTime;       
    }

    void HandleEnemyDead(EnemyHealth enemy)
    {
        Debug.Log(enemy.name);
        killCount++;
    }

    void HandlePlayerDead()
    {
        PauseGame();
    }

    public float GetPlayTime()
    {
        return playTime;
    }

    public int GetKillCount()
    {
        return killCount;
    }

    public void PauseGame()
    {
        isPlaying = false;

        // 마우스 커서 변경

        // 게임 중지
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPlaying = true;

        // 마우스 커서 변경

        // 게임 재개
        Time.timeScale = 1f;
    }
}
