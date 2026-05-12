using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    PlayerDead playerDead;    

    InputActionMap playerMap;
    InputActionMap systemMap;
    InputActionMap uiMap;

    public static InputManager Instance;

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

        playerMap = playerInput.actions.FindActionMap("Player");
        systemMap = playerInput.actions.FindActionMap("System");
        uiMap = playerInput.actions.FindActionMap("UI");
    }    

    void Start()
    {
        playerDead = FindFirstObjectByType<PlayerDead>();

        playerMap.Enable();
        systemMap.Enable();
        uiMap.Disable();

        playerDead.OnPlayerDead += HandlePlayerDead;
    }

    void OnDestroy()
    {        
        playerDead.OnPlayerDead -= HandlePlayerDead;
    }

    void HandlePlayerDead()
    {
        playerMap.Disable();
        uiMap.Disable();
        systemMap.Disable();        
    }

    public void PauseGame()
    {        
        playerMap.Disable();
        uiMap.Enable();
    }

    public void ResumeGame()
    {        
        playerMap.Enable();
        uiMap.Disable();
        systemMap.Enable();
    }
}
