using System.Collections;
using UnityEngine;

public class PlayerUnderCC : MonoBehaviour
{
    [SerializeField] float knockbackSpeed = 2.0f;

    Animator animator;
    CharacterController characterController;    
    bool isFalldown = false;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    public void PlayCCSequence(CCType type)
    {        
        switch (type)
        {
            case CCType.None:
                break;
            case CCType.Falldown:
                if(!isFalldown)
                {
                    animator.SetBool("CanControl", false);
                    animator.SetTrigger("Falldown");
                    isFalldown = true;
                    StartCoroutine(FalldownCoroutine());
                }                
                break;
            default:
                break;
        }
    }

    IEnumerator FalldownCoroutine()
    {
        while(isFalldown)
        {
            Vector3 dir = -transform.forward;

            characterController.Move(dir * knockbackSpeed * Time.deltaTime);

            yield return null;
        }

        animator.ResetTrigger("Falldown");
        StartCoroutine(FalldownStayCoroutine());
    }

    IEnumerator FalldownStayCoroutine()
    {
        yield return new WaitForSeconds(1f);

        animator.SetTrigger("GetUp");
    }

    public void EndPlayerFalldown()
    {
        isFalldown = false;
    }    
}
