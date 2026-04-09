using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGuard : MonoBehaviour
{
    [SerializeField] float parryTime = 0.5f;
    [SerializeField] GameObject perfectGuardVFX;
    [SerializeField] GameObject normalGuardVFX;
    [SerializeField] ParticleSystem parryVFX;
    [SerializeField] float slowTime = 5f;

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
        canParry = false;
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

    public void SuccessParry(Vector3 hitDirection)
    {
        // 상대 공격 차단        
        parryVFX.Play();

        // 플레이어의 방향을 공격받은 방향으로 회전
        hitDirection.y = 0;
        if (hitDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(hitDirection);
            transform.rotation = targetRotation;
        }

        // 슬로우 모션
        StartCoroutine(SlowMotion());
    }

    IEnumerator SlowMotion()
    {
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(slowTime);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
