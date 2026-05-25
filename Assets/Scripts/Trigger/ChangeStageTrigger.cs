using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class ChangeStageTrigger : MonoBehaviour
{
    [SerializeField] GameObject portalVFX;

    bool canChange = false;

    void Start()
    {
        GameManager.Instance.OnStageCleared += ActivateClearPortal;
    }

    void OnDestroy()
    {
        GameManager.Instance.OnStageCleared -= ActivateClearPortal;
    }

    void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {            
            ChangeStage();
        }
    }

    void ChangeStage()
    {
        if (!canChange)
            return;

        EventManager.Instance.LoadNextScene();        
    }

    void ActivateClearPortal()
    {
        canChange = true;
        portalVFX.SetActive(true);
    }
}
