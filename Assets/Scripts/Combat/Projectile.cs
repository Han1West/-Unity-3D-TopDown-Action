using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 30f;

    Rigidbody rb;    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.linearVelocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
