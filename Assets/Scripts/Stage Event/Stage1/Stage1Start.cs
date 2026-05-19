using System.Collections;
using UnityEngine;

public class Stage1Start : MonoBehaviour
{
    [SerializeField] GameObject dialogueEvent;

    TypeWriter typeWriter;

    void Start()
    {
        typeWriter = dialogueEvent.GetComponentInChildren<TypeWriter>();

        typeWriter.OnTypingEnd += OnTypingEventEnd;


        StartCoroutine(DialogueEventRoutine());
    }

    void OnDestroy()
    {
        typeWriter.OnTypingEnd -= OnTypingEventEnd;    
    }

    void OnTypingEventEnd()
    {
        // 대화문 종료        
        InputManager.Instance.ResumeGame();
    }

    IEnumerator DialogueEventRoutine()
    {
        yield return null;

        // 대화문 시작
        dialogueEvent.SetActive(true);

        // 게임 정지        
        InputManager.Instance.PauseGame();
    }
}
