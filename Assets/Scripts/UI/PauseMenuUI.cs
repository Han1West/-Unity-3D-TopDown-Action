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
        Debug.Log("On Click Resume");

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
        //isToTitle = true;
        //warningUI.SetActive(true);
        StartCoroutine(OpenWarningRoutine(true));
    }

    void OnClickExit()
    {
        StartCoroutine(OpenWarningRoutine(false));
        //warningUI.SetActive(true);
    }

    IEnumerator OpenWarningRoutine(bool toTitle)
    {
        yield return null;

        isToTitle = toTitle;
        warningUI.SetActive(true);
    }

    void OnClickWarningConfirm()
    {
        Debug.Log("On Click Warning Confirm");

        InputManager.Instance.ResumeGame();
        GameManager.Instance.ResumeGame();

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
        StartCoroutine(WarningCancelRoutine());
        //isToTitle = false;
        //warningUI.SetActive(false);
    }

    IEnumerator WarningCancelRoutine()
    {
        yield return null;

        isToTitle = false;
        warningUI.SetActive(false);
    }
}
