using Mono.Cecil.Cil;
using UnityEngine;

public class EnemyDeathSound : MonoBehaviour
{
    [SerializeField] AudioClip deathSFX;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        GameObject obj = new GameObject("DeathSFX");
        AudioSource source = obj.AddComponent<AudioSource>();

        source.clip = deathSFX;
        source.Play();

        Destroy(obj, deathSFX.length);
    }
}
