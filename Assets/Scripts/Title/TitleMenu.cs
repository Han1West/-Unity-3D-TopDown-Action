using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    [Header("Title UI")]
    [SerializeField] TMP_Text titleText1;
    [SerializeField] TMP_Text titleText2;

    [SerializeField] Button newGameButton;
    [SerializeField] Button continueButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button exitButton;

    [Header("Option UI")]
    [SerializeField] GameObject optionUI;

    [Header("Warning UI")]
    [SerializeField] GameObject warningUI;

    [SerializeField] Button warningConfirmButton;
    [SerializeField] Button warningCancelButton;

    [Header("ETC")]
    [SerializeField] float speed = 1f;

    void Start()
    {
        newGameButton.onClick.AddListener(OnClickNewGame);
        continueButton.onClick.AddListener(OnClickContinue);
        optionButton.onClick.AddListener(OnClickOption);
        exitButton.onClick.AddListener(OnClickExit);

        warningConfirmButton.onClick.AddListener(OnClickWarningConfirm);
        warningCancelButton.onClick.AddListener(OnClickWarningCancel);

        if (PlayerPrefs.HasKey("SaveData") || PlayerPrefs.HasKey("Start SaveData"))
            continueButton.interactable = true;
        else
            continueButton.interactable = false;
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);

        titleText1.color = Color.Lerp(Color.yellow, Color.red, t);
        titleText2.color = Color.Lerp(Color.white, new Color(0.6f, 0f, 1f), t);
    }

    void OnClickNewGame()
    {
        SaveManager.Instance.IsContinueLoading = false;

        SaveData continuedData = SaveManager.Instance.LoadGame();
        SaveData startData = SaveManager.Instance.LoadSavedStartGame();

        // 저장된 세이브파일이 있다
        if (continuedData != null || startData != null )
        {
            warningUI.SetActive(true);
        }
        else
            EventManager.Instance.LoadNextScene();            
    }

    void OnClickContinue()
    {        
        SaveData continuedData = SaveManager.Instance.LoadGame();
        SaveData startData = SaveManager.Instance.LoadSavedStartGame();
        
        if (continuedData != null)
        {
            SaveManager.Instance.IsContinueLoading = true;
            SceneManager.LoadScene(continuedData.sceneName);
        }
        else if (startData != null)
        {
            SaveManager.Instance.IsContinueLoading = true;
            SceneManager.LoadScene(startData.sceneName);
        }            
    }

    void OnClickOption()
    {
        optionUI.SetActive(true);
    }

    void OnClickExit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    void OnClickWarningConfirm()
    {
        EventManager.Instance.LoadNextScene();
    }

    void OnClickWarningCancel()
    {
        warningUI.SetActive(false);
    }
}
