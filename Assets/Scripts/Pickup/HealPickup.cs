using UnityEngine;

public class HealPickup : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 0.5f;
    public int heal = 10;
        
    void Update()
    {
        transform.Rotate(0f, rotationSpeed, 0f);
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }


}
