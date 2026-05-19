using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputPlayerNameUI : MonoBehaviour
{
    [SerializeField] GameObject confirmManualUI;
    [SerializeField] GameObject warningUI;
    [SerializeField] TMP_InputField inputName;
    [SerializeField] Button confirmButton;
    [SerializeField] Button cancelButton;
    [SerializeField] TMP_Text displayNameText;

    void Start()
    {
        confirmManualUI.SetActive(true);

        inputName.onSubmit.AddListener(OnSubmitInput);

        confirmButton.onClick.AddListener(OnClickConfirm);
        cancelButton.onClick.AddListener(OnClickCancel);

        // 포커스 바로 가져오기
        inputName.Select();
        inputName.ActivateInputField();
    }


    void OnSubmitInput(string text)
    {
        // 입력한 이름이 비어있으면
        if (string.IsNullOrWhiteSpace(text))
        {
            inputName.Select();
            inputName.ActivateInputField();
            return;
        }
            

        OnEnterInput();        
    }

    void OnEnterInput()
    {        
        warningUI.SetActive(true);
        displayNameText.text = inputName.text;
    }

    void OnClickConfirm()
    {
        GameManager.Instance.SetPlayerInGameName(inputName.text);
        EventManager.Instance.LoadNextScene();
    }

    void OnClickCancel()
    {        
        warningUI.SetActive(false);
        inputName.Select();
        inputName.ActivateInputField();
    }


}
