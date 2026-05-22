using UnityEngine;
using UnityEngine.InputSystem;

public class SystemInputHandler : MonoBehaviour
{
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject optionUI;
    [SerializeField] GameObject warningUI;    
    [SerializeField] TypingTrigger typing;
    [SerializeField] SkippableEvent skippableEvent;    
 
    bool isPaused = false;


    public void OnPause(InputValue value)
    {
        // 옵션 켜져있으면 옵션 끄기
        if (optionUI && optionUI.activeInHierarchy)
        {
            DeactivateOptionMenu();
            return;
        }

        if(warningUI && warningUI.activeInHierarchy)
        {
            DeactivateWarningUI();
            return;
        }

        if (pauseUI != null)
        {
            if (value.isPressed)
                isPaused = !isPaused;

            if (isPaused)
                ActivatePauseMenu();
            else
                DeactivatePauseMenu();
        }
    }

    public void OnSkip(InputValue value)
    {
        if (value.isPressed && skippableEvent)
            skippableEvent.SkipEvent();

        if (value.isPressed && typing)
            typing.SkipDialog();
    }

    void ActivatePauseMenu()
    {
        // 플레이어 조작, UI 조작 활성화 상태 변경
        InputManager.Instance.PauseGame();
        // 게임 상태 변경
        GameManager.Instance.PauseGame();
        GameManager.Instance.ActivateBaseCursor();
        
        // UI 활성화
        pauseUI.SetActive(true);

        // 효과음
        AudioManager.Instance.PlayPause();
        AudioManager.Instance.TurnDownVolumeBGM();
    }

    void DeactivatePauseMenu()
    {
        InputManager.Instance.ResumeGame();
        GameManager.Instance.ResumeGame();
        GameManager.Instance.DeactivateBaseCursor();

        pauseUI.SetActive(false);

        // 효과음
        AudioManager.Instance.PlayResume();
        AudioManager.Instance.ResetVolumeBGM();
    }

    void DeactivateOptionMenu()
    {
        pauseUI.SetActive(true);
        optionUI.SetActive(false);
    }

    void DeactivateWarningUI()
    {
        warningUI.SetActive(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        GameManager.Instance.DeactivateBaseCursor();

        AudioManager.Instance.PlayResume();
        AudioManager.Instance.ResetVolumeBGM();
    }
}
