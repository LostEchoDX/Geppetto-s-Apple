using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public AudioMixer mixer;
    public GameObject options;
    private Slider sliderMusic;
    private Slider sliderEffect;
    private Slider sliderMaster;

    private void Awake()
    {
        if (mixer == null)
        {
            mixer = Resources.Load<AudioMixer>("AudioMixer");
            Debug.Log("test", mixer);
        }
    }

    private void Start()
    {
        loadingAudioVolume();
    }

    [System.Obsolete]
    private void Update()
    {
        if (options.active)
        {
            SetSliderVolume();
        }
    }

    public void SetVolume(string parameterName, float volume)
    {
        Debug.Log(mixer);
        if (mixer != null)
        {
            mixer.SetFloat(parameterName, Mathf.Log10(volume) * 20);
        }
        PlayerPrefs.SetFloat(parameterName + "Volume", volume);
    }

    public void SetVolumeMaster(float volume)
    {
        SetVolume("Master", volume);
    }

    public void SetVolumeMusic(float volume)
    {
        SetVolume("Music", volume);
    }

    public void SetVolumeEffect(float volume)
    {
        SetVolume("Effect", volume);
    }

    public void loadingAudioVolume()
    {
        float Music;
        float Effect;
        float Master;

        if (mixer == null)
        {
            mixer = Resources.Load<AudioMixer>("AudioMixer");
        }

        if (PlayerPrefs.HasKey("MusicVolume") == true)
        {
            Music = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            Music = 0.5f;
        }

        if (PlayerPrefs.HasKey("EffectVolume") == true)
        {
            Effect = PlayerPrefs.GetFloat("EffectVolume");

        }
        else
        {
            Effect = 0.5f;
        }

        if (PlayerPrefs.HasKey("MasterVolume") == true)
        {
            Master = PlayerPrefs.GetFloat("MasterVolume");
        }
        else
        {
            Master = 0.5f;
        }

        SetVolumeMaster(Master);
        SetVolumeMusic(Music);
        SetVolumeEffect(Effect);

    }
    private void SetSliderVolume()
    {
        sliderMusic = GameObject.Find("SliderMusic").GetComponent<Slider>();
        sliderEffect = GameObject.Find("SliderEffect").GetComponent<Slider>();
        sliderMaster = GameObject.Find("SliderMaster").GetComponent<Slider>();

        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume");
        sliderEffect.value = PlayerPrefs.GetFloat("EffectVolume");
        sliderMaster.value = PlayerPrefs.GetFloat("MasterVolume");
    }
}
