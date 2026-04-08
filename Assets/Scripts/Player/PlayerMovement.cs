using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float runSpeed = 15f;
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashTime = .15f;
    [SerializeField] float rotationSpeed = 10f;    

    [Header("잔상")]
    [SerializeField] float interval = 0.05f;
    [SerializeField] float lifeTime = 0.3f;
    [SerializeField] Color afterImgaeColor = new Color(0.5f, 0.8f, 1f, 0.6f);
    [SerializeField] Material afterImageMaterials;

    SkinnedMeshRenderer[] skinnedRenderers;
    MeshRenderer[] meshRenderers;

    PlayerController playerController;
    CharacterController characterController;
    Animator animator;
    float dashTimer;
    
    bool isDashing = false;
    bool wasRunning = false;
    Vector3 dashDirection;



    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        skinnedRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void StartDash()
    {
        isDashing = true;
        dashTimer = dashTime;
        dashDirection = transform.forward;
        dashDirection.Normalize();
        StartCoroutine(SpawnAfterImages());
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

    IEnumerator SpawnAfterImages()
    {
        while(isDashing)
        {
            SpawnAfterImage();
            yield return new WaitForSeconds(interval);
        }
    }

    void SpawnAfterImage()
    {
        foreach(var renderer in skinnedRenderers)
        {
            Mesh bakedMesh = new Mesh();
            renderer.BakeMesh(bakedMesh);
            CreateGhost(bakedMesh, renderer.transform);
        }

        foreach (var renderer in meshRenderers)
        {
            MeshFilter mf = renderer.GetComponent<MeshFilter>();
            if (mf == null || mf.sharedMesh == null)
                continue;

            CreateGhost(mf.sharedMesh, renderer.transform);
        }
    }

    private void CreateGhost(Mesh bakedMesh, Transform originTransform)
    {
        GameObject ghost = new GameObject("AfterImage");
        ghost.transform.position = originTransform.position;
        ghost.transform.rotation = originTransform.rotation;
        ghost.transform.localScale = originTransform.localScale;

        MeshFilter mf = ghost.AddComponent<MeshFilter>();
        MeshRenderer mr = ghost.AddComponent<MeshRenderer>();

        mf.mesh = bakedMesh;
        mr.material = afterImageMaterials;
        mr.material.color = afterImgaeColor;

        StartCoroutine(FadeAndDestroy(ghost, lifeTime));
    }

    IEnumerator FadeAndDestroy(GameObject ghost, float duration)
    {
        MeshRenderer mr = ghost.GetComponent<MeshRenderer>();
        Color startColor = mr.material.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 0f, elapsed / duration);
            Color c = startColor;
            c.a = alpha;
            mr.material.color = c;
            yield return null;                
        }

        Mesh mesh = ghost.GetComponent<MeshFilter>().mesh;
        if(mesh != ghost.GetComponent<MeshFilter>().sharedMesh)
            Destroy(ghost.GetComponent<MeshFilter>().mesh);

        Destroy(ghost);
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
