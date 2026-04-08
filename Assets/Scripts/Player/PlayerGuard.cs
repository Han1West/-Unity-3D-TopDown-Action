using Unity.VisualScripting;
using UnityEngine;

public class PlayerGuard : MonoBehaviour
{
    [SerializeField] float parryTime = 0.5f;
    [SerializeField] GameObject perfectGuardVFX;
    [SerializeField] GameObject normalGuardVFX;
    [SerializeField] ParticleSystem parryVFX;

    PlayerController playerController;
    Animator animator;

    float parryTimer = 0f;
    public bool canParry { get; private set; } = false;
    public bool isGuarding { get; private set; } = false;    

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
    }

    public void StartGuard()
    {
        canParry = true;
        isGuarding = true;
        parryTimer = parryTime;
        animator.SetBool("IsGuarding", true);
        perfectGuardVFX.SetActive(true);
    }

    public void EndGuard()
    {
        isGuarding = false;
        animator.SetBool("IsGuarding", false);
        normalGuardVFX.SetActive(false);
        perfectGuardVFX.SetActive(false);
    }

    public void UpdateGuard(bool stillGuarding)
    {
        if (!isGuarding) return;

        parryTimer -= Time.deltaTime;

        // 특정 시간이 지나면 패리 기능이 꺼진다.
        if(parryTimer < 0f )
        {
            canParry = false;
            perfectGuardVFX.SetActive(false);
            normalGuardVFX.SetActive(true);
        }

        // 가드 입력이 끝나면 State 변경
        if (!stillGuarding)
            playerController.ChangeState(PlayerState.Idle);
    }

    public void SuccessParry()
    {
        // 상대 공격 차단
        Debug.Log("Success Parry");
        parryVFX.Play();
    }
}
