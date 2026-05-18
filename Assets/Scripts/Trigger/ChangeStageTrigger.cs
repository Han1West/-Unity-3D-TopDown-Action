using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class ChangeStageTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {            
            ChangeStage();
        }
    }

    void ChangeStage()
    {

        if (!GameManager.Instance.CanChangeStage())
            return;

        EventManager.Instance.LoadNextScene();        
    }
}
