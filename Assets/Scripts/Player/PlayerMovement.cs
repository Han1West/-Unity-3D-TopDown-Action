using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float runSpeed = 15f;
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashTime = .15f;
    [SerializeField] float rotationSpeed = 10f;
    
    PlayerController playerController;
    CharacterController characterController;
    Animator animator;
    float dashTimer;
    bool isDashing = false;
    bool wasRunning = false;
    Vector3 dashDirection;


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    public void StartDash()
    {
        isDashing = true;
        dashTimer = dashTime;
        dashDirection = transform.forward;
        dashDirection.Normalize();
    }

    public void EndDash()
    {
        isDashing = false;
    }

    public void UpdateDash()
    {
        if (!isDashing) return;

        characterController.Move(dashDirection * dashSpeed * Time.deltaTime);
        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {            
            playerController.ChangeState(PlayerState.Idle);
        }
    }

    public void StartMove()
    {
        animator.SetBool("IsMoving", true);        
    }

    public void EndMove()
    {
        animator.SetBool("IsMoving", false);
    }

    public void UpdateMove(Vector2 movement, bool isRunning)
    {
        // 이동 없으면 State 탈출
        if (movement == Vector2.zero)
        {
            playerController.ChangeState(PlayerState.Idle);
            animator.SetBool("IsMoving", false);
        }
            

        if (wasRunning != isRunning)
        {
            animator.SetBool("IsRunning", isRunning);
            wasRunning = isRunning;
        }
        

        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        float speed = isRunning ? runSpeed : moveSpeed;

        // 해당 방향으로 움직인다
        characterController.Move(move * speed * Time.deltaTime);

        // 해당 방향과 같은 회전값이 되도록 회전
        if (move == Vector3.zero) return;
        
        Quaternion targetRotation = Quaternion.LookRotation(move);        
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);        
    }
}
