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

        Debug.Log(clearGameUI.transform.root.gameObject.activeInHierarchy);
        Debug.Log(clearGameUI.activeSelf);      // 자기 자신
        Debug.Log(clearGameUI.activeInHierarchy); // 실제 씬에서 활성 여부

        //GameManager.Instance.PauseGame();
        //InputManager.Instance.PauseGame();          
    }
}
