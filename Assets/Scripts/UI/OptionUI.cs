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

    void Start()
    {
        BackButton.onClick.AddListener(OnClickBack);        

        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        effectVolumeSlider.onValueChanged.AddListener(SetEffectVolume);
        BGMVolumeSlider.onValueChanged.AddListener(SetBGMVolume);

        // 임시 기본 값
        SetMasterVolume(1);
        SetEffectVolume(1);
        SetBGMVolume(1);
    }

    public void SetMasterVolume(float value)
    {
        if (value <= 0)
            audioMixer.SetFloat("MasterVolume", -80f);
        else
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetEffectVolume(float value)
    {
        if (value <= 0)
            audioMixer.SetFloat("SFXVolume", -80f);
        else
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }

    public void SetBGMVolume(float value)
    {
        if (value <= 0)
            audioMixer.SetFloat("BGMVolume", -80f);
        else
            audioMixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
    }

    void OnClickBack()
    {
        gameObject.SetActive(false);
        if(pauseUI != null)
            pauseUI.SetActive(true);
    }
}

