using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [SerializeField] float speed = 30f;
    [SerializeField] GameObject hitEffect;
    [SerializeField] AudioClip[] hitSFX;
    
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
        int i = Random.Range(0, hitSFX.Length);
        
        AudioSource.PlayClipAtPoint(hitSFX[i], transform.position);
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
