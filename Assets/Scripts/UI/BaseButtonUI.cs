using UnityEngine;
using UnityEngine.EventSystems;

public enum UIButtonType
{ 
    Default,
    Confirm,
    Cancel,
    Startgame,
    Complete,
    PaperConfirm,
    PaperCancel,
}


public class BaseButtonUI : MonoBehaviour,
    IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] UIButtonType buttonType;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayUIHover(buttonType);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlayUIClick(buttonType);
    }
}
