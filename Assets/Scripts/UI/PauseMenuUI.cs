using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenuUI : MonoBehaviour
{
    [Header("menu")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button toTitleButton;
    [SerializeField] Button exitButton;

    [Header("Popup")]
    [SerializeField] GameObject optionUI;
    [SerializeField] GameObject warningUI;

    [Header("WarningButton")]
    [SerializeField] Button warningConfirmButton;
    [SerializeField] Button warningCancelButton;

    SystemInputHandler systemInputHandler;

    bool isToTitle = false;

    void Start()
    {        
        systemInputHandler = FindFirstObjectByType<SystemInputHandler>();

        resumeButton.onClick.AddListener(OnClickResume);
        optionButton.onClick.AddListener(OnClickOption);
        toTitleButton.onClick.AddListener(OnClickToTitle);
        exitButton.onClick.AddListener(OnClickExit);

        warningConfirmButton.onClick.AddListener(OnClickWarningConfirm);
        warningCancelButton.onClick.AddListener(OnClickWarningCancel);
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
        isToTitle = true;
        warningUI.SetActive(true);        
    }

    void OnClickExit()
    {
        warningUI.SetActive(true);
    }

    void OnClickWarningConfirm()
    {
        if (isToTitle)
        {
            isToTitle = false;
            EventManager.Instance.LoadTitle();            
        }
        else
        {
            isToTitle = false;
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }
    }

    void OnClickWarningCancel()
    {
        isToTitle = false;
        warningUI.SetActive(false);
    }
}
