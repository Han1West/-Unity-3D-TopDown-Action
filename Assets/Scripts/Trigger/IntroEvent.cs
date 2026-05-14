using TMPro;
using UnityEngine;

public class IntroEvent : MonoBehaviour
{
    [SerializeField] GameObject[] texts;
    [SerializeField] GameObject skipManualUI;

    int currentIndex = 0;
    TypeWriter currentTypeWriter;

    private void Start()
    {
        currentTypeWriter = texts[currentIndex].GetComponentInParent<TypeWriter>();
    }

    private void Update()
    {       
        if (currentTypeWriter.IsFulled && !skipManualUI.activeInHierarchy)
            skipManualUI.SetActive(true);
    }


    public void SkipDialog()
    {
        if (currentIndex == texts.Length - 1)
            return;
        
        // 현재 대화문이 다출력 됐으면 다음으로
        if (currentTypeWriter.IsFulled)
        {
            skipManualUI.SetActive(false);
            texts[currentIndex].gameObject.SetActive(false);
            currentIndex++;
            texts[currentIndex].gameObject.SetActive(true);
            currentTypeWriter = texts[currentIndex].GetComponentInParent<TypeWriter>();
        }
        // 대화문 다 출력 안됐으면 다 출력
        else
        {
            currentTypeWriter.SkipTyping();            
        }               
    }
}
