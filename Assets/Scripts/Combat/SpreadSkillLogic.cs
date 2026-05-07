using UnityEngine;
using UnityEngine.UIElements;

public class SpreadSkillLogic : MonoBehaviour
{
    [SerializeField] SphereCollider skillHitboxes;
    [SerializeField] float speed = 30f;
    [SerializeField] float lifeTime = 3f;
    [SerializeField] float maxRadius = 13f;
    [SerializeField] AudioClip chargeSFX;
    [SerializeField] AudioClip emissionSFX;

    AudioSource audioSoruce;

    float lifeTimer = 0f;
    bool isEmissioned = false;

    void Awake()
    {
        audioSoruce = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSoruce.PlayOneShot(chargeSFX, 0.5f);
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;

        if(lifeTimer > 1f)
        {
            if(!isEmissioned)
            {
                audioSoruce.PlayOneShot(emissionSFX, 0.5f);
                isEmissioned = true;
            }
            

            if (!skillHitboxes.enabled)
            {
                skillHitboxes.enabled = true;
            }

            skillHitboxes.radius += speed * Time.deltaTime;
            if(skillHitboxes.radius > maxRadius)
                 skillHitboxes.radius = maxRadius;
        }
        

        if (lifeTimer > lifeTime)
            Destroy(gameObject);
    }
}
