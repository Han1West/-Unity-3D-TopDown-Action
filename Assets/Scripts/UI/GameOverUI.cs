using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TMP_Text tryText;
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text killText;
    [SerializeField] Button retryButton;
    [SerializeField] Button exitButton;
    [SerializeField] PlayerDead playerDead;

    [Header("BGM")]
    [SerializeField] AudioClip failBGM;

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

        tryText.text = GameManager.Instance.GetTryCount().ToString();
        timeText.text = minute.ToString() + ":" + second.ToString();
        killText.text = GameManager.Instance.GetKillCount().ToString();
        
        gameObject.SetActive(true);

        // 커서 활성화
        GameManager.Instance.ActivateBaseCursor();
        GameManager.Instance.DeactivateInGameCrosshair();

        // 효과음
        AudioManager.Instance.PlayOpenPaperUI();
        AudioManager.Instance.PlayBGM(failBGM);
    }

    void OnClickRetry()
    {
        GameManager.Instance.ResumeGame();
        InputManager.Instance.ResumeGame();

        EventManager.Instance.LoadSavedStartScene();        
    }

    void OnClickExit()
    {
        GameManager.Instance.ResumeGame();
        InputManager.Instance.ResumeGame();

        SaveManager.Instance.DeleteSave();
        EventManager.Instance.LoadTitle();
    }
}
