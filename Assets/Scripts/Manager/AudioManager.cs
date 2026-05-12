using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource bgmSource;

    public static AudioManager Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);    
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        Debug.Log($"Clip: {clip.name}");
        Debug.Log($"Source Active: {sfxSource.gameObject.activeInHierarchy}");
        Debug.Log($"Source Enabled: {sfxSource.enabled}");
        Debug.Log($"Volume: {volume}");
        
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayBGM(AudioClip clip, float volume = 1f)
    {
        bgmSource.clip = clip;
        bgmSource.volume = volume;
        bgmSource.Play();
    }
}
