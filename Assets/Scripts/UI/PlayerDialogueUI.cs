using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDialogueUI : MonoBehaviour
{
    [SerializeField] TMP_Text speakerName;    

    void Start()
    {
        speakerName.text = GameManager.Instance.PlayerInGameName;
    }

}
