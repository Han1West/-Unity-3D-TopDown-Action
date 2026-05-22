using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class CursorUI : MonoBehaviour
{
    [SerializeField] Image cursorImage;

    [SerializeField] Sprite baseSprite;
    [SerializeField] Sprite choiceSprite;    
    

    void Update()
    {
        transform.position = Input.mousePosition;

        CheckButtonHover();
    }

    void CheckButtonHover()
    {
        PointerEventData data = new PointerEventData(EventSystem.current);

        data.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(data, results);

        bool isButtonHover = false;

        foreach (RaycastResult rs in results)
        {
            // RayCast 막는 UI 발견
            if(rs.gameObject.TryGetComponent<Graphic>(out Graphic graphic))
            {
                // 버튼이면 커서 변경
                if (rs.gameObject.GetComponentInParent<Button>() != null)
                {
                    isButtonHover = true;                    
                }

                // 가장 앞 ui만 검사후 종료
                break;
            }
        }

        cursorImage.sprite = isButtonHover ? choiceSprite : baseSprite;
    }
}
