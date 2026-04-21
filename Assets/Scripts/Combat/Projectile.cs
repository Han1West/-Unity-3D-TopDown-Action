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

    void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }

}
