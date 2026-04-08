using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [Range(0f, 1f)]
    [SerializeField] float maxOffsetAmount = 0.2f;
    [SerializeField] float smoothSpeed = 5f;
    [SerializeField] float deadCameraDistanceSpeed = 3f;
    [SerializeField] float deadCameraRotationSpeed = 2f;
    
    CinemachinePositionComposer positionComposer;
    Camera cam;
    bool isRecovering = false;
    public bool isDeadSequencePlaying { get; set; } = false;

    void Awake()
    {        
        positionComposer = GetComponent<CinemachinePositionComposer>();
        cam = Camera.main;
    }

    void Update()
    {
        // Ctrl key -> 둘러보기
        if(Input.GetKey(KeyCode.LeftControl))
        {
            UpdateCameraToSearch();
        }
        // 기존으로 회복
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isRecovering = true;                 
        }

        if(isRecovering)
        {
            UpdateCameraToNormal();
        }

        if(isDeadSequencePlaying)
        {
            ProcessDeadSequence();
        }
    }

    void UpdateCameraToSearch()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        // 플레이어 → 마우스 화면 방향
        Vector3 viewDir = mousePosition - player.position;

        float horizontalDist = new Vector2(viewDir.x, viewDir.z).magnitude;

        Vector2 targetOffset;
        if (horizontalDist > 0.01f)
        {
            // 방향 추출
            Vector2 normalizedDir = new Vector2(-viewDir.x, viewDir.z);
            normalizedDir.Normalize();

            // 거리에 따라 부드럽게 증가하다 최대값에서 클램프
            float influence = Mathf.Clamp01(horizontalDist / 10f); // 10유닛 거리에서 최대
            targetOffset = normalizedDir * (maxOffsetAmount * influence);
        }
        else
        {
            targetOffset = Vector2.zero;
        }

        Vector2 current = positionComposer.Composition.ScreenPosition;

        positionComposer.Composition.ScreenPosition = Vector2.Lerp(
            current,
            targetOffset,
            smoothSpeed * Time.deltaTime);
    }

    void UpdateCameraToNormal()
    {
        Vector2 current = positionComposer.Composition.ScreenPosition;

        positionComposer.Composition.ScreenPosition = Vector2.Lerp(
            current,
            Vector2.zero,
            smoothSpeed * Time.deltaTime);

        if((current - Vector2.zero).magnitude < 0.001f)
        {
            positionComposer.Composition.ScreenPosition = Vector2.zero;
            isRecovering = false;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, player.position);

        if (plane.Raycast(ray, out float distance))
            return ray.GetPoint(distance);

        return player.position;
    }

    void ProcessDeadSequence()
    {
        // 카메라 확대
        positionComposer.CameraDistance -= deadCameraDistanceSpeed * Time.deltaTime;

        // 카메라 기울기 내리기
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.x -= deadCameraRotationSpeed * Time.deltaTime; 
        transform.eulerAngles = currentRotation;         
    }

}
