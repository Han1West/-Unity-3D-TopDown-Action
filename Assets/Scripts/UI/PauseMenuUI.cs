using UnityEngine;
using UnityEngine.UI;
using System.Collections;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button toTitleButton;
    [SerializeField] Button exitButton;

    [SerializeField] GameObject optionUI;
    
    SystemInputHandler systemInputHandler;

    void Start()
    {        
        systemInputHandler = FindFirstObjectByType<SystemInputHandler>();

        resumeButton.onClick.AddListener(OnClickResume);
        optionButton.onClick.AddListener(OnClickOption);
        toTitleButton.onClick.AddListener(OnClickToTitle);
        exitButton.onClick.AddListener(OnClickExit);        
    }

    void OnClickResume()
    {
        InputManager.Instance.ResumeGame();
        GameManager.Instance.ResumeGame();
        systemInputHandler.ResumeGame();

        gameObject.SetActive(false);        
    }

    void OnClickOption()
    {
        gameObject.SetActive(false);
        optionUI.SetActive(true);
    }

    void OnClickToTitle()
    {

    }

    void OnClickExit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
