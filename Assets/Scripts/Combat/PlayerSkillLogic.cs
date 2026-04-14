using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSkillLogic : MonoBehaviour
{
    [SerializeField] SphereCollider skillHitboxes;
    [SerializeField] float speed = 30f;
    [SerializeField] float lifeTime = 3f;
    [SerializeField] float maxRadius = 13f;

    float lifeTimer = 0f;

    void Update()
    {
        lifeTimer += Time.deltaTime;

        if(lifeTimer > 1f)
        {
            skillHitboxes.radius += speed * Time.deltaTime;
            if(skillHitboxes.radius > maxRadius)
                 skillHitboxes.radius = maxRadius;
        }
        

        if (lifeTimer > lifeTime)
            Destroy(gameObject);
    }
}
