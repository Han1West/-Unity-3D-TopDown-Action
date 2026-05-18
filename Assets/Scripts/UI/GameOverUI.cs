using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text killText;
    [SerializeField] Button retryButton;
    [SerializeField] Button exitButton;
    [SerializeField] PlayerDead playerDead;

    void Start()
    {        
        if(playerDead != null)
        {
            playerDead.OnPlayerDead += UpdateDeadInfomation;            
        }
        
        retryButton.onClick.AddListener(OnClickRetry);
        exitButton.onClick.AddListener(OnClickExit);

        gameObject.SetActive(false);
    }


    void OnDestroy()
    {
        if(playerDead != null)        
            playerDead.OnPlayerDead -= UpdateDeadInfomation;        
    }

    public void UpdateDeadInfomation()
    {        
        float totalTime = GameManager.Instance.GetPlayTime();
        int minute = (int)totalTime / 60;
        int second = (int)totalTime % 60;

        timeText.text = minute.ToString() + ":" + second.ToString();
        killText.text = GameManager.Instance.GetKillCount().ToString();

        Debug.Log("Player Dead Overay");
        gameObject.SetActive(true);
    }

    void OnClickRetry()
    {
        GameManager.Instance.ResumeGame();
        InputManager.Instance.ResumeGame();
        SceneManager.LoadScene(2);
    }

    void OnClickExit()
    {
        GameManager.Instance.ResumeGame();
        InputManager.Instance.ResumeGame();

        EventManager.Instance.LoadTitle();
    }
}
