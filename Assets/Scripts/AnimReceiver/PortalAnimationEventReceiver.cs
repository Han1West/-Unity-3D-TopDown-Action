using UnityEngine;

public class PortalAnimationEventReceiver : MonoBehaviour
{
    EnemySpawnPortal portal;

    void Awake()
    {
        portal = GetComponentInParent<EnemySpawnPortal>();
    }

    public void EndClose()
    {
        portal.EndClose();
    }
}
