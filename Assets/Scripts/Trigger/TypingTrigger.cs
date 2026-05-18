using TMPro;
using UnityEngine;

public class TypingTrigger : MonoBehaviour
{
    [SerializeField] GameObject text;    


    TypeWriter currentTypeWriter;

    void Start()
    {
        currentTypeWriter = text.GetComponentInParent<TypeWriter>();
    }


    public void SkipDialog()
    {        
        currentTypeWriter.SkipDialog();
    }
}
