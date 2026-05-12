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
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        SceneManager.LoadScene(nextScene);
    }
}
