using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
    [SerializeField] TMP_Text textUI;
    [SerializeField] float typingSpeed = 0.05f;

    [TextArea]
    [SerializeField] string fullText;

    public bool IsFulled { get; private set; } = false;

    void Start()
    {
        StartCoroutine(TypeTextRoutine());
    }

    public void SkipTyping()
    {
        StopAllCoroutines();
        
        textUI.text = fullText;
        IsFulled = true;
    }

    IEnumerator TypeTextRoutine()
    {
        textUI.text = "";

        foreach(char c in fullText)
        {
            textUI.text += c;
            if (textUI.text == fullText)
                IsFulled = true;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
