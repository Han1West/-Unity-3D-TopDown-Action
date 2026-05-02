using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [SerializeField] float speed = 30f;
    [SerializeField] GameObject hitEffect;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {        
        rb.linearVelocity = Vector3.down * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
