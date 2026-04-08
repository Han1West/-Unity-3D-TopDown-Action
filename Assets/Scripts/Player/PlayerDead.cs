using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerDead : MonoBehaviour
{
    [SerializeField] float raiseSpeed = 1f;
    [SerializeField] float raiseTime = 4f;
    [SerializeField] float rasieStartTime = 1f;
    [SerializeField] ParticleSystem sequenceVFX;
    [SerializeField] ParticleSystem disappearVFX;
    

    SkinnedMeshRenderer[] skinnedRenderers;
    MeshRenderer[] meshRenderers;
    Animator animator;
    CapsuleCollider hitCollider;
    CharacterController characterController;

    CinemachineCamera cinemachineCamera;
    CameraController cameraController;

    float raiseTimer = 0f;
    bool isDeadSequencePlaying = false;
    bool isSequenceVFXPlay = false;

    void Awake()
    {
        skinnedRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();

        animator = GetComponentInChildren<Animator>();
        hitCollider = GetComponent<CapsuleCollider>();
        characterController = GetComponent<CharacterController>();

        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        cameraController = cinemachineCamera.GetComponent<CameraController>();
    }

    public void StartDead()
    {
        animator.SetTrigger("Dead");

        // 충돌 처리 모두 끄기
        hitCollider.enabled = false;
        characterController.enabled = false;
        isDeadSequencePlaying = true;


        // 그림자 꺼주기
        foreach (var renderer in skinnedRenderers)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
        foreach (var renderer in meshRenderers)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }

    void Update()
    {
        if (isDeadSequencePlaying)
        {
            ProcessDeadSequence();
        }
    }

    void ProcessDeadSequence()
    {
        raiseTimer += Time.deltaTime;

        if (raiseTimer > rasieStartTime)
        {
            if(!isSequenceVFXPlay)
            {
                sequenceVFX.Play();
                isSequenceVFXPlay = true;
            }

            cameraController.isDeadSequencePlaying = true;
            Vector3 upDir = transform.up;

            // 위로 서서히 떠오른다.
            transform.position += upDir * raiseSpeed * Time.deltaTime;
            sequenceVFX.transform.position -= upDir * raiseSpeed * Time.deltaTime;

            if (raiseTimer > raiseTime)
            {
                EndDeadSequence();
            }
        }

    }

    void EndDeadSequence()
    {
        // 이펙트 실행
        disappearVFX.Play();

        // 모든 렌더러를 꺼준다 (플레이어 숨기기)
        foreach (var renderer in skinnedRenderers)
        {
            renderer.enabled = false;
        }
        foreach (var renderer in meshRenderers)
        {
            renderer.enabled = false;
        }
        isDeadSequencePlaying = false;
        cameraController.isDeadSequencePlaying = false;
    }
}
