using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] public TextMeshPro damageText;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float lifeTime = 1f;

    void Start()
    {
        StartCoroutine(FloatAndFade());
    }

    void LateUpdate()
    {
        // 카메라 향하게 
        transform.forward = Camera.main.transform.forward;  
    }

    IEnumerator FloatAndFade()
    {
        float elapsed = 0f;
        Color startColor = damageText.color;

        while (elapsed < lifeTime)
        {
            elapsed += Time.deltaTime;
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

            damageText.color = new Color(startColor.r, startColor.g, startColor.b,
                Mathf.Lerp(1f, 0f, elapsed / lifeTime));

            yield return null;
        }
        Destroy(gameObject);
    }
}
