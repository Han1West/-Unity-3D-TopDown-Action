using UnityEngine;

public class ClearGameTrigger : MonoBehaviour
{
    [SerializeField] GameObject clearGameUI;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ClearGame();
        }
    }

    void ClearGame()
    {
        Debug.Log("Clear");
        clearGameUI.SetActive(true);

        GameManager.Instance.PauseGame();
        InputManager.Instance.PauseGame();
    }
}
