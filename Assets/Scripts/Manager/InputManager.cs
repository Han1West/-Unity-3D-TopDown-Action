using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerDead playerDead;    

    InputActionMap playerMap;
    InputActionMap systemMap;
    InputActionMap uiMap;

    public static InputManager Instance;

    bool isPlaying = true;

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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(InitializeScene());
    }

    void OnDestroy()
    {       
        if(playerDead)
            playerDead.OnPlayerDead -= HandlePlayerDead;

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void HandlePlayerDead()
    {
        playerMap.Disable();
        uiMap.Disable();
        systemMap.Disable();        
    }

    public void PauseGame()
    {
        isPlaying = false;
        playerMap.Disable();
        uiMap.Enable();
    }

    public void ResumeGame()
    {
        isPlaying = true;
        playerMap.Enable();
        uiMap.Disable();
        systemMap.Enable();
    }

    IEnumerator InitializeScene()
    {
        yield return null;

        // 이전 플레이어 이벤트 해제
        if(playerDead)
            playerDead.OnPlayerDead -= HandlePlayerDead;

        PlayerController player = FindFirstObjectByType<PlayerController>();
        if(player)
            playerInput = player.GetComponent<PlayerInput>();
        playerDead = FindFirstObjectByType<PlayerDead>();

        if (playerInput)
        {
            playerMap = playerInput.actions.FindActionMap("Player");
            systemMap = playerInput.actions.FindActionMap("System");
            uiMap = playerInput.actions.FindActionMap("UI");

            if(isPlaying)
            {
                playerMap.Enable();
                systemMap.Enable();
                uiMap.Disable();
            }
            else
            {
                playerMap.Disable();
                uiMap.Disable();
                systemMap.Enable();
            }            
        }

        isPlaying = true;

        if (playerDead)                   
            playerDead.OnPlayerDead += HandlePlayerDead;                    
    }
}
