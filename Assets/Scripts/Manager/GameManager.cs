using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    PlayerHealth playerHealth;
    PlayerDead playerDead;
    PlayerGuard playerGuard;

    public bool isInPlaying { get; private set; } = false;

    int tempPlayerHealth = 0;
    int tempPlayerParryPoint = 0;

    float playTime = 0;
    int killCount = 0;
    List<GameObject> enemies = new List<GameObject>();

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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnEnable()
    {
        
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EnemyHealth.OnEnemyDead -= HandleEnemyDead;
        playerDead.OnPlayerDead -= HandlePlayerDead;
    }

    // 새로운 씬 로드
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {        
        StartCoroutine(InitializeScene());
    }

    void Update()
    {
        if(isInPlaying)        
            playTime += Time.deltaTime;       
    }

    void HandleEnemyDead(EnemyHealth enemy)
    {        
        killCount++;

        enemies.Remove(enemy.gameObject);

        //// 씬 내에 모든 적 제거 -> Save
        //if(enemies.Count == 0 )
        //    SaveManager.Instance.SaveGame();
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
        isInPlaying = false;

        // 마우스 커서 변경

        // 게임 중지
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isInPlaying = true;

        // 마우스 커서 변경

        // 게임 재개
        Time.timeScale = 1f;
    }

    void ApplySaveData(SaveData data)
    {
        playTime = data.playTime;
        killCount = data.totalKill;

        playerHealth.SetCurrentHealth(data.playerHp); 
        playerGuard.SetParryPoint(data.playerParryPoint);        
    }

    void ApplyPlayerInfo()
    {        
        playerHealth.SetCurrentHealth(tempPlayerHealth);
        playerGuard.SetParryPoint(tempPlayerParryPoint);

        tempPlayerHealth = 0;
        tempPlayerParryPoint = 0;
    }

    public bool CanChangeStage()
    {                
        if (enemies.Count <= 0)
            return true;

        return false;
    }

    public void AddNewEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }    

    IEnumerator InitializeScene()
    {        
        yield return null;

        playerDead = FindFirstObjectByType<PlayerDead>();
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        playerGuard = FindFirstObjectByType<PlayerGuard>();

        SceneInfo sceneInfo = FindFirstObjectByType<SceneInfo>();

        if (sceneInfo.sceneType == SceneType.InGamePlay)
            isInPlaying = true;
        else
        {
            isInPlaying = false;
            yield break;
        }
            

        EnemyHealth.OnEnemyDead += HandleEnemyDead;
        if(playerDead)
            playerDead.OnPlayerDead += HandlePlayerDead;

        enemies.Clear();

        // 스테이지 내 몬스터들을 전부 넣는다.
        GameObject[] allObject = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in allObject)
        {
            if (obj.layer == LayerMask.NameToLayer("Enemy") && obj.transform.parent == null)
                enemies.Add(obj);
        }

        // 저장된 게임 시작
        if (SaveManager.Instance.IsContinueLoading)
        {
            SaveData data = SaveManager.Instance.LoadGame();

            if (data != null)
                ApplySaveData(data);

            SaveManager.Instance.IsContinueLoading = false;
        }
        // 현재 상태 세이브
        else
        {
            // 저장되있는 플레이어 정보 적용
            if(EventManager.Instance.IsTransPlayerInfo)
            {
                Debug.Log($"Before Apply: {tempPlayerHealth}");
                ApplyPlayerInfo();
            }
                
            // 현재 레벨 저장
            SaveManager.Instance.SaveGame();            
        }        
    }

    public void SaveTempPlayerInfo(PlayerInfo playerInfo)
    {        
        tempPlayerHealth = playerInfo.currentHealth;
        tempPlayerParryPoint = playerInfo.currentParryPoint;        
    }
}
