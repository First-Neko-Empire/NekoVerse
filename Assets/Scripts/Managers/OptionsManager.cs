using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : Singleton<OptionsManager>
{
    [SerializeField]
    Toggle tgl_fullScreen;
    [SerializeField]
    Slider globalVolumeSlider;
    [SerializeField]
    Slider soundEffectsVolumeSlider;
    [SerializeField]
    TMP_InputField if_endpoint;
    [SerializeField]
    AudioSource theme;

    private float globalVolume;
    private float soundEffectsVolume;


    private void Start()
    {
        if_endpoint.text = Dotenv.endpoint;
    }

    public float SoundEffectsVolume
    {
        get { return  soundEffectsVolumeSlider.value; }
        private set {  soundEffectsVolume = value; }
    }

    public float GlobalVolume
    {
        get { return globalVolumeSlider.value; }
        private set { globalVolume = value; }
    }

    public float EffectiveSoundEffectsVolume
    {
        get { return globalVolume * soundEffectsVolume; }
    }

    private new void Awake()
    {
        base.Awake();
        LoadPlayerData();
    }

    public void OnToggleValueChanged()
    {
        Screen.fullScreen = tgl_fullScreen.isOn;
        SaveFullScreenToPrefs();
    }

    public void OnGlobalVolumeSliderValueChanged()
    {
        globalVolume = globalVolumeSlider.value;
        theme.volume = GlobalVolume;
        SaveGlobalVolumeToPrefs();
    }

    public void OnSoundEffectsVolumeSliderValueChanged()
    {
        soundEffectsVolume = soundEffectsVolumeSlider.value;
        SaveSoundEffectsVolumeToPrefs();
    }


    void LoadPlayerData()
    {
        LoadGlobalVolumeFromPrefs();
        LoadSoundEffectsVolumeFromPrefs();
        LoadFullScreenFromPrefs();
    }


    void LoadGlobalVolumeFromPrefs()
    {
        if (PlayerPrefs.HasKey("GLOBAL_VOLUME"))
        {
            globalVolume = PlayerPrefs.GetFloat("GLOBAL_VOLUME");
            globalVolumeSlider.value = globalVolume;
        }
    }
    void LoadSoundEffectsVolumeFromPrefs()
    {
        if (PlayerPrefs.HasKey("SOUND_EFFECTS_VOLUME"))
        {
            soundEffectsVolume = PlayerPrefs.GetFloat("SOUND_EFFECTS_VOLUME");
            soundEffectsVolumeSlider.value = soundEffectsVolume;
        }
    }
    void LoadFullScreenFromPrefs()
    {
        if (PlayerPrefs.HasKey("FULLSCREEN"))
        {
            tgl_fullScreen.isOn = PlayerPrefs.GetInt("FULLSCREEN") == 1 ? true : false;
        }
    }



    void SaveGlobalVolumeToPrefs()
    {
        PlayerPrefs.SetFloat("GLOBAL_VOLUME", globalVolume);
    }
    void SaveSoundEffectsVolumeToPrefs()
    {
        PlayerPrefs.SetFloat("SOUND_EFFECTS_VOLUME", soundEffectsVolume);
    }
    void SaveFullScreenToPrefs()
    {
        PlayerPrefs.SetInt("FULLSCREEN", tgl_fullScreen.isOn==true?1:0);
    }


    public void OnEndpointEndEdit()
    {
        NethereumManager.Instance.TryChangeEndPoint(if_endpoint.text);
    }
}
