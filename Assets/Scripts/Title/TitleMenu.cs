using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] TMP_Text titleText1;
    [SerializeField] TMP_Text titleText2;

    [SerializeField] Button newGameButton;
    [SerializeField] Button contiunueButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button exitButton;

    [SerializeField] GameObject optionUI;

    [SerializeField] float speed = 1f;

    void Start()
    {
        newGameButton.onClick.AddListener(OnClickNewGame);
        contiunueButton.onClick.AddListener(OnClickContinue);
        optionButton.onClick.AddListener(OnClickOption);
        exitButton.onClick.AddListener(OnClickExit);
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);

        titleText1.color = Color.Lerp(Color.yellow, Color.red, t);
        titleText2.color = Color.Lerp(Color.white, new Color(0.6f, 0f, 1f), t);
    }

    void OnClickNewGame()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        SceneManager.LoadScene(nextScene);
    }

    void OnClickContinue()
    {

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
}
