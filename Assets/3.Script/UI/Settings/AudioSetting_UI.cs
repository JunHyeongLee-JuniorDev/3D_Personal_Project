using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSetting_UI : MonoBehaviour
{
    private SettingSaveData saveData;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private void OnEnable()
    {
        saveData = Managers.Instance.Data.settingSaveData;

        masterSlider.value = saveData.masterVolume;
        sfxSlider.value = saveData.SFXVolume;
        musicSlider.value = saveData.musicVolume;
    }

    public void OnMasterChange()
    {
        masterSlider.value = (int)masterSlider.value;

        if (masterSlider.value <= -40.0f)
            Managers.Instance.Data.audioMixer.SetFloat("Master", -80.0f);

        else Managers.Instance.Data.audioMixer.SetFloat("Master", masterSlider.value);
    }

    public void OnMusicChange()
    {
        musicSlider.value = (int)musicSlider.value;

        if (musicSlider.value <= -40.0f)
            Managers.Instance.Data.audioMixer.SetFloat("Music", -80.0f);

        else Managers.Instance.Data.audioMixer.SetFloat("Music", musicSlider.value);
    }

    public void OnSFXChange()
    {
        sfxSlider.value = (int)sfxSlider.value;

        if (sfxSlider.value <= -40.0f)
            Managers.Instance.Data.audioMixer.SetFloat("SFX", -80.0f);

        else Managers.Instance.Data.audioMixer.SetFloat("SFX", sfxSlider.value);
    }

    public void OnClickApply_Btn()
    {
        float _Volume;

        Managers.Instance.Data.audioMixer.GetFloat("Master", out _Volume);
        saveData.masterVolume = _Volume;

        Managers.Instance.Data.audioMixer.GetFloat("SFX", out _Volume);
        saveData.SFXVolume = _Volume;

        Managers.Instance.Data.audioMixer.GetFloat("Music", out _Volume);
        saveData.musicVolume = _Volume;

        Managers.Instance.Data.SaveSetting();
    }

    private void OnDisable()
    {
        Managers.Instance.Data.settingSaveData = Managers.Instance.Data.LoadSetting();
        saveData = Managers.Instance.Data.settingSaveData;
    }
}
