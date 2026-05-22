using UnityEngine;

public class InGameCrosshair : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
