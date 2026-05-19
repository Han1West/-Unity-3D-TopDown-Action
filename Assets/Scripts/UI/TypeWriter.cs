using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
    [SerializeField] GameObject parentUI;
    [SerializeField] GameObject inputFieldUI;
    [SerializeField] TMP_Text textUI;
    [SerializeField] float typingSpeed = 0.05f;

    [TextArea]
    [SerializeField] string[] fullTexts;
    [SerializeField] GameObject skipManualUI;    

    int currentIndex = 0;

    public bool IsFulled { get; private set; } = false;
    public event Action OnTypingEnd;

    void Start()
    {
        StartCoroutine(TypeTextRoutine());
    }

    private void Update()
    {
        if (skipManualUI && 
            IsFulled && !skipManualUI.activeInHierarchy)
            skipManualUI.SetActive(true);
    }

    public void SkipTyping()
    {
        StopAllCoroutines();
        
        textUI.text = fullTexts[currentIndex];
        IsFulled = true;
    }

    IEnumerator TypeTextRoutine()
    {
        textUI.text = "";
            
        foreach(char c in fullTexts[currentIndex])
        {
            textUI.text += c;
            if (textUI.text == fullTexts[currentIndex])
                IsFulled = true;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void SkipDialog()
    {
        // 현재 대화문이 다출력 됐으면 다음으로
        if (IsFulled)
        {
            // 마지막 다이얼로그면
            if (currentIndex == fullTexts.Length - 1)
            {
                SceneInfo sceneInfo = FindFirstObjectByType<SceneInfo>();

                // 현재 씬이 Intro -> 이름 입력창
                if (sceneInfo.sceneType == SceneType.CutScene)
                {
                    gameObject.SetActive(false);
                    skipManualUI.SetActive(false);
                    inputFieldUI.SetActive(true);
                    //EventManager.Instance.LoadNextScene();
                }
                    
                // 현재 씬이 Stage -> UI 끄기
                else if (sceneInfo.sceneType == SceneType.InGamePlay)
                {
                    parentUI.SetActive(false);
                    OnTypingEnd?.Invoke();
                }
                    
            }
            else
            {
                if(skipManualUI)
                    skipManualUI.SetActive(false);                
                currentIndex++;
                IsFulled = false;
                StartCoroutine(TypeTextRoutine());
            }
        }
        // 대화문 다 출력 안됐으면 다 출력
        else
        {
            SkipTyping();
        }
    }
}
