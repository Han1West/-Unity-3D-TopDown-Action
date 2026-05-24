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
    InGameCrosshair inGameCrosshair;
    CursorUI baseCursor;

    public bool isInPlaying = false;    

    int tempPlayerHealth = 0;
    int tempPlayerParryPoint = 0;

    float playTime = 0;
    int killCount = 0;
    int tryCount = 1;
    public bool IsSucceed { get; private set; } = false;
    List<GameObject> enemies = new List<GameObject>();

    public static GameManager Instance;
    public event Action OnPlayerDataLoaded;
    public event Action OnStageCleared;

    public string PlayerInGameName { get; private set; }

    bool isAlertClearInfo = false;

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

        Cursor.visible = false;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EnemyHealth.OnEnemyDead -= HandleEnemyDead;
    }

    // 새로운 씬 로드
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(InitializeScene());
    }

    void Update()
    {
        if (isInPlaying)
            playTime += Time.deltaTime;

        if (enemies.Count <= 0 && !isAlertClearInfo)
        {
            Debug.Log("NO ENEMY");
            OnStageCleared?.Invoke();
            isAlertClearInfo = true;
        }            
    }

    void HandleEnemyDead(EnemyHealth enemy)
    {        
        killCount++;        

        enemies.Remove(enemy.gameObject);
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

    public int GetTryCount()
    {
        return tryCount;
    }

    public void PauseGame()
    {
        isInPlaying = false;

        // 마우스 커서 변경
        DeactivateInGameCrosshair();

        // 게임 중지
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isInPlaying = true;        

        // 마우스 커서 변경
        ActivateInGameCrosshair();

        // 게임 재개
        Time.timeScale = 1f;
    }

    void ApplySaveData(SaveData data)
    {        
        PlayerInGameName = data.playerName;

        playTime = data.playTime;
        killCount = data.totalKill;
        tryCount = data.tryCount;
        IsSucceed = data.isSucceed;

        playerHealth.SetCurrentHealth(data.playerHp);
        playerGuard.SetParryPoint(data.playerParryPoint);        
    }

    void ApplyPlayerInfo()
    {        
        if (playerHealth &&  playerGuard)
        {            
            playerHealth.SetCurrentHealth(tempPlayerHealth);
            playerGuard.SetParryPoint(tempPlayerParryPoint);

            tempPlayerHealth = 0;
            tempPlayerParryPoint = 0;
        }        
    }

    //public bool CanChangeStage()
    //{
    //    if (enemies.Count <= 0)                    
    //        return true;
        
            
    //    return false;
    //}

    public void AddNewEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    IEnumerator InitializeScene()
    {
        yield return null;

        // 이전 플레이어 Dead 해제
        if (playerDead)
            playerDead.OnPlayerDead -= HandlePlayerDead;

        EnemyHealth.OnEnemyDead -= HandleEnemyDead;

        playerDead = FindFirstObjectByType<PlayerDead>();
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        playerGuard = FindFirstObjectByType<PlayerGuard>();
        inGameCrosshair = FindFirstObjectByType<InGameCrosshair>();
        baseCursor = FindFirstObjectByType<CursorUI>();

        
        SceneInfo sceneInfo = FindFirstObjectByType<SceneInfo>();

        if (sceneInfo.sceneType == SceneType.InGamePlay)
        {
            isInPlaying = true;
            DeactivateBaseCursor();
        }            
        else
        {
            isInPlaying = false;
            ActivateBaseCursor();
            yield break;
        }


        EnemyHealth.OnEnemyDead += HandleEnemyDead;
        if (playerDead)
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
        // 리트라이 게임 시작
        else if (SaveManager.Instance.IsRetryLoading)
        {
            SaveData data = SaveManager.Instance.LoadSavedStartGame();

            if (data != null)
                ApplySaveData(data);

            tryCount++;

            SaveManager.Instance.IsRetryLoading = false;
            SaveManager.Instance.SaveGame();
        }
        // 레벨 진행
        else
        {
            // 저장되있는 플레이어 정보 적용
            if (EventManager.Instance.IsTransPlayerInfo)
            {
                ApplyPlayerInfo();
            }

            // 현재 레벨 저장
            SaveManager.Instance.SaveGame();
        }

        isAlertClearInfo = false;
        OnPlayerDataLoaded?.Invoke();
    }

    public void SaveTempPlayerInfo(PlayerInfo playerInfo)
    {
        tempPlayerHealth = playerInfo.currentHealth;
        tempPlayerParryPoint = playerInfo.currentParryPoint;
    }

    public void SetPlayerInGameName(string name)
    {
        PlayerInGameName = name;
    }

    public void ActivateInGameCrosshair()
    {
        if(inGameCrosshair)
            inGameCrosshair.gameObject.SetActive(true);       
    }

    public void ActivateBaseCursor()
    {
        if (baseCursor)
            baseCursor.gameObject.SetActive(true);
    }

    public void DeactivateInGameCrosshair()
    {
        if (inGameCrosshair)
            inGameCrosshair.gameObject.SetActive(false);        
    }

    public void DeactivateBaseCursor()
    {        
        if(baseCursor)
            baseCursor.gameObject.SetActive(false);        
    }
}
