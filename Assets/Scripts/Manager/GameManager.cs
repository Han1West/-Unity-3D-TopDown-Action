using UnityEngine;

public class GameManager : MonoBehaviour
{    
    PlayerDead playerDead;

    public bool IsPlay { get; private set; }

    float playTime = 0;
    int killCount = 0;

    void Start()
    {
        playerDead = FindFirstObjectByType<PlayerDead>();
        IsPlay = true;
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
        if(IsPlay)        
            playTime += Time.deltaTime;       
    }

    void HandleEnemyDead(EnemyHealth enemy)
    {
        Debug.Log(enemy.name);
        killCount++;
    }

    void HandlePlayerDead()
    {
        IsPlay = false;
    }

    public float GetPlayTime()
    {
        return playTime;
    }

    public int GetKillCount()
    {
        return killCount;
    }
}
