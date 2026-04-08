using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    public void EndHit()
    {
        GetComponentInParent<PlayerHealth>().EndHit();
    }
}
