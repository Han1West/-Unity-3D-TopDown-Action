using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearUI : MonoBehaviour
{
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text killText;
    [SerializeField] Button toTitleButton;    

    void Start()
    {
        toTitleButton.onClick.AddListener(OnClickToTitle);                
    }

    private void OnEnable()
    {
        float totalTime = GameManager.Instance.GetPlayTime();
        int minute = (int)totalTime / 60;
        int second = (int)totalTime % 60;

        timeText.text = minute.ToString() + ":" + second.ToString();
        killText.text = GameManager.Instance.GetKillCount().ToString();
    }

    void OnClickToTitle()
    {
        GameManager.Instance.ResumeGame();
        InputManager.Instance.ResumeGame();

        // 게임오버 -> 세이브파일 제거 (초기화)
        SaveManager.Instance.DeleteSave();
        EventManager.Instance.LoadTitle();   
    }
}
