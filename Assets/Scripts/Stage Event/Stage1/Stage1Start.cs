using System.Collections;
using UnityEngine;

[System.Serializable]
public class TextRow
{ 
    public string[] texts;
}


public class Stage1Start : MonoBehaviour
{
    [SerializeField] GameObject dialogueEvent;
    [SerializeField] TextRow[] commonRetryTexts;
    [SerializeField] TextRow[] succeedRetryTexts;

    TypeWriter typeWriter;

    void Start()
    {
        GameManager.Instance.OnPlayerDataLoaded += StartDialogueEvent;        
    }

    void OnDestroy()
    {
        typeWriter.OnTypingEnd -= OnTypingEventEnd;
        GameManager.Instance.OnPlayerDataLoaded -= StartDialogueEvent;
    }

    void StartDialogueEvent()
    {
        ApplyDialogueTexts();

        typeWriter.OnTypingEnd += OnTypingEventEnd;

        StartCoroutine(DialogueEventRoutine());
    }

    void ApplyDialogueTexts()
    {
        TextRow[] appliedTexts = new TextRow[0];

        if (GameManager.Instance.IsSucceed)
            appliedTexts = succeedRetryTexts;
        else
            appliedTexts = commonRetryTexts;

            int tryCount = GameManager.Instance.GetTryCount();

        typeWriter = dialogueEvent.GetComponentInChildren<TypeWriter>();        

        if (tryCount <= 2)
        {
            typeWriter.SetTexts(appliedTexts[tryCount - 1].texts);
        }
        else
            typeWriter.SetTexts(appliedTexts[2].texts);

    }

    void OnTypingEventEnd()
    {
        // 대화문 종료        
        InputManager.Instance.ResumeGame();
        GameManager.Instance.ActivateInGameCrosshair();        
    }

    IEnumerator DialogueEventRoutine()
    {
        yield return null;

        // 대화문 시작
        dialogueEvent.SetActive(true);

        // 게임 정지        
        InputManager.Instance.PauseGame();
        GameManager.Instance.DeactivateInGameCrosshair();
    }
}
