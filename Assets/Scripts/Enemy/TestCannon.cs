using UnityEngine;

public class TestCannon : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPosition;
    [SerializeField] float fireSpeed;
    [SerializeField] float fireDelayTime = 2f;

    float fireTimer = 0f;

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

            fireTimer = fireDelayTime;
        }
    }

}
