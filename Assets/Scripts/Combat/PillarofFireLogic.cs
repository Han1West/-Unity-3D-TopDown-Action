using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Audio;

public class PillarofFireLogic : MonoBehaviour
{
    [SerializeField] GameObject warningBox;
    [SerializeField] BoxCollider attackHitbox;
    [SerializeField] ParticleSystem effect;
    [SerializeField] float alertTime = 2.0f;
    [SerializeField] float lifeTime = 8.0f;
    [SerializeField] AudioClip firePillarSFX;

    AudioSource audioSource;
    float accTime;
    bool attacked = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        warningBox.SetActive(true);
        attackHitbox.enabled = false;
    }

    void Update()
    {
        accTime += Time.deltaTime;

        if(!attacked && accTime > alertTime)
        {
            attacked = true;

            warningBox.SetActive(false);
            audioSource.PlayOneShot(firePillarSFX, 0.5f);
            attackHitbox.enabled = true;
            effect.Play();
        }

        if (accTime > lifeTime)
            StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        effect.Stop();
        attackHitbox.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
