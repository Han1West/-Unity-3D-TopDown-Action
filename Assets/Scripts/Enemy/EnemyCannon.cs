using UnityEngine;
using UnityEngine.Audio;

public class TestCannon : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPosition;
    [SerializeField] float fireSpeed;
    [SerializeField] float fireDelayTime = 2f;
    [SerializeField] AudioClip fireSFX;

    AudioSource audioSource;
    float fireTimer = 0f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        fireTimer = fireDelayTime;    
    }


    void Update()
    {
        fireTimer -= Time.deltaTime;   

        if( fireTimer < 0 )
        {
            Quaternion spawnRotation = transform.rotation * Quaternion.Euler(0, -90, 0);

            Instantiate(projectilePrefab, projectileSpawnPosition.position, spawnRotation);
            audioSource.PlayOneShot(fireSFX, 0.4f);

            fireTimer = fireDelayTime;
        }
    }

}
