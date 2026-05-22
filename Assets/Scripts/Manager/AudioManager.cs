using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource bgmSource;

    [Header("UI Sound")]
    [SerializeField] AudioClip uiHoverSFX;
    [SerializeField] AudioClip paperuiHoverSFX;
    [SerializeField] AudioClip uiConfirmSFX;
    [SerializeField] AudioClip uiCancelSFX;
    [SerializeField] AudioClip paperuiConfirmSFX;
    [SerializeField] AudioClip paperuiCancelSFX;
    [SerializeField] AudioClip uiStartGameSFX;
    [SerializeField] AudioClip uiCompleteSFX;
    [SerializeField] AudioClip openPaperUISFX;
    [SerializeField] AudioClip pauseSFX;
    [SerializeField] AudioClip resumeSFX;
    [SerializeField] AudioClip textBlipSFX;
    [SerializeField] AudioClip textSlamSFX;

    public static AudioManager Instance;

    float tempVolume = 0f;

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
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayBGM(AudioClip clip, float volume = 0.15f)
    {
        bgmSource.clip = clip;
        bgmSource.volume = volume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void TurnDownVolumeBGM()
    {
        tempVolume = bgmSource.volume;
        bgmSource.volume = 0.05f;
    }

    public void ResetVolumeBGM()
    {
        bgmSource.volume = tempVolume;
    }

    public void PlayUIHover(UIButtonType type)
    {
        switch (type)
        {
            case UIButtonType.Default:
            case UIButtonType.Confirm:
            case UIButtonType.Cancel:
            case UIButtonType.Startgame:
            case UIButtonType.Complete:            
                sfxSource.PlayOneShot(uiHoverSFX);
                break;
            case UIButtonType.PaperConfirm:
            case UIButtonType.PaperCancel:
                sfxSource.PlayOneShot(paperuiHoverSFX);
                break;
            default:
                break;
        }        
    }

    public void PlayUIClick(UIButtonType type)
    {
        switch (type)
        {
            case UIButtonType.Default:
            case UIButtonType.Confirm:
                sfxSource.PlayOneShot(uiConfirmSFX);
                break;
            case UIButtonType.Cancel:
                sfxSource.PlayOneShot(uiCancelSFX);
                break;
            case UIButtonType.Startgame:
                sfxSource.PlayOneShot(uiStartGameSFX);
                break;
            case UIButtonType.Complete:
                sfxSource.PlayOneShot(uiCompleteSFX);                
                break;
            case UIButtonType.PaperConfirm:
                sfxSource.PlayOneShot(paperuiConfirmSFX);
                break;
            case UIButtonType.PaperCancel:
                sfxSource.PlayOneShot(paperuiCancelSFX);
                break;
            default:
                break;
        }        
    }

    public void PlayOpenPaperUI()
    {
        sfxSource.PlayOneShot(openPaperUISFX);
    }

    public void PlayPause()
    {
        sfxSource.PlayOneShot(pauseSFX);
    }

    public void PlayResume()
    {
        sfxSource.PlayOneShot(resumeSFX);
    }

    public void PlayTextBlip()
    {
        sfxSource.PlayOneShot(textBlipSFX, 0.3f);
    }

    public void PlayTextSlam()
    {
        sfxSource.PlayOneShot(textSlamSFX, 0.4f);
    }
}
