using JetBrains.Annotations;
using UnityEngine;

public class EnemySpawnPortal : MonoBehaviour
{
    [SerializeField] GameObject spawnEnemy;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float spawnCooldown = 5f;
    [SerializeField] float lifeTime = 30f;

    Animator animator;
    float lifeTimer = 0f;
    float spawnTimer = 3f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        lifeTimer += Time.deltaTime;

        if (spawnTimer > spawnCooldown)
        {
            GameObject newEnemy = Instantiate(spawnEnemy, spawnPoint.position, Quaternion.identity);
            GameManager.Instance.AddNewEnemy(newEnemy);
            spawnTimer = 0f;
        }

        if(lifeTimer > lifeTime)
        {
            animator.SetTrigger("Close");
        }        
    }

    public void EndClose()
    {
        GameManager.Instance.HandlePortalDisappear(gameObject);

        Destroy(gameObject);
    }

}
