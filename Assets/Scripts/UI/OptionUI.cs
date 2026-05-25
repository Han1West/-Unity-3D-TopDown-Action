using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider effectVolumeSlider;
    [SerializeField] Slider BGMVolumeSlider;
    [SerializeField] Button BackButton;    

    [SerializeField] GameObject pauseUI;

    const string MASTER_KEY = "MasterVolume";
    const string SFX_KEY = "SFXVolume";
    const string BGM_KEY = "BGMVolume";

    void Start()
    {
        BackButton.onClick.AddListener(OnClickBack);        

        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        effectVolumeSlider.onValueChanged.AddListener(SetEffectVolume);
        BGMVolumeSlider.onValueChanged.AddListener(SetBGMVolume);

        LoadVolume();
    }

    public void SetMasterVolume(float value)
    {
        if (value <= 0)
            audioMixer.SetFloat("MasterVolume", -80f);
        else
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat(MASTER_KEY, value);
    }

    public void SetEffectVolume(float value)
    {
        if (value <= 0)
            audioMixer.SetFloat("SFXVolume", -80f);
        else
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat(SFX_KEY, value);
    }

    public void SetBGMVolume(float value)
    {
        if (value <= 0)
            audioMixer.SetFloat("BGMVolume", -80f);
        else
            audioMixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat(BGM_KEY, value);
    }

    void OnClickBack()
    {
        gameObject.SetActive(false);
        if(pauseUI != null)
            pauseUI.SetActive(true);
    }

    void LoadVolume()
    {
        float master = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float sfx = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        float bgm = PlayerPrefs.GetFloat(BGM_KEY, 1f);

        masterVolumeSlider.value = master;
        effectVolumeSlider.value = sfx;
        BGMVolumeSlider.value = bgm;

        SetMasterVolume(master);
        SetEffectVolume(sfx);
        SetBGMVolume(bgm);        
    }
}

