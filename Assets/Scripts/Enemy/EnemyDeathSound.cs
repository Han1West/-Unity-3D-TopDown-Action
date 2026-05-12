using Mono.Cecil.Cil;
using UnityEngine;

public class EnemyDeathSound : MonoBehaviour
{
    [SerializeField] AudioClip deathSFX;    

   
    public void Play()
    {
        //GameObject obj = new GameObject("DeathSFX");
        //AudioSource source = obj.AddComponent<AudioSource>();

        //source.clip = deathSFX;
        //source.Play();
        AudioManager.Instance.PlaySFX(deathSFX);

//        Destroy(obj, deathSFX.length);
    }
}
