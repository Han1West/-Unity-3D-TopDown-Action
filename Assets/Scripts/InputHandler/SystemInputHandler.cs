using UnityEngine;
using UnityEngine.InputSystem;

public class SystemInputHandler : MonoBehaviour
{
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject optionUI;
    [SerializeField] IntroEvent intro;

    bool isPaused = false;


    public void OnPause(InputValue value)
    {
        // 옵션 켜져있으면 옵션 끄기
        if (optionUI.activeInHierarchy)
        {
            DeactivateOptionMenu();
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
        if (value.isPressed)
            intro.SkipDialog();
    }

    void ActivatePauseMenu()
    {
        // 플레이어 조작, UI 조작 활성화 상태 변경
        InputManager.Instance.PauseGame();
        // 게임 상태 변경
        GameManager.Instance.PauseGame();

        // UI 활성화
        pauseUI.SetActive(true);
    }

    void DeactivatePauseMenu()
    {
        InputManager.Instance.ResumeGame();
        GameManager.Instance.ResumeGame();

        pauseUI.SetActive(false);
    }

    void DeactivateOptionMenu()
    {
        pauseUI.SetActive(true);
        optionUI.SetActive(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
    }
}
