using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VideoSetting_UI : MonoBehaviour
{
    [SerializeField]
    private ToggleSwitch_UI fullScreenBtn;

    [SerializeField]
    private ToggleGroup qualityGroup;

    [SerializeField]
    private TMP_Dropdown resolutionDropdown;

    SettingSaveData saveData;

    private void Awake()
    {
        Resolution[] _resolutionArr = Screen.resolutions;
        Resolution[] _trueR = _resolutionArr.Where(r => r.refreshRateRatio.numerator == 60).ToArray();
        List<string> _resolutionList = new List<string>();

        for (int i = 0; i < _trueR.Length; i++)
        {
            _resolutionList.Add(_trueR[i].width + "X" +
                                _trueR[i].height.ToString());
        }
        fullScreenBtn.onToggle_Event.AddListener(OnClickFullScreen);

        resolutionDropdown.AddOptions(_resolutionList);
    }

    private void OnEnable()
    {
        Managers.Instance.Data.settingSaveData = Managers.Instance.Data.LoadSetting();
        saveData = Managers.Instance.Data.settingSaveData;

        resolutionDropdown.value = saveData.resolutionIndex;
        resolutionDropdown.RefreshShownValue();
        fullScreenBtn.isOn = saveData.isFullScreen;
        AssignShownQuality();
    }

    private void PlayClickSound()
    {
        Managers.Instance.Sound.Play("UI/MenuClick");
    }

    private void AssignShownQuality()
    {
        Toggle[] toggles = qualityGroup.GetComponentsInChildren<Toggle>();
        qualityGroup.SetAllTogglesOff();

        string toggleName;

        switch (saveData.urpAssetsIndex)
        {
            case 0:
                toggleName = "Low";
                break;

            case 1:
                toggleName = "Middle";
                break;

            case 2:
                toggleName = "High";
                break;

            default:
                toggleName = string.Empty;
                break;
        }

        for(int i=0; i < toggles.Length; i++)
        {
            if (toggles[i].name.Equals(toggleName))
            {
                toggles[i].isOn = true;
            }
        }

        qualityGroup.allowSwitchOff = false;
    }

    public void OnClickFullScreen()
    {
        PlayClickSound();
        saveData.isFullScreen = fullScreenBtn.isOn;
    }

    public void OnClickResolution()
    {
        PlayClickSound();
        saveData.resolutionIndex = resolutionDropdown.value;
    }

    public void OnClickQuality_Btn(int qualityIndex)
    {
        PlayClickSound();
        if (saveData != null)
        saveData.urpAssetsIndex = qualityIndex;
    }

    public void OnApply_Btn()
    {
        PlayClickSound();
        Screen.SetResolution(Screen.resolutions[saveData.resolutionIndex].width,
                             Screen.resolutions[saveData.resolutionIndex].height,
                             saveData.isFullScreen);

        QualitySettings.SetQualityLevel(index: saveData.urpAssetsIndex,
                                        applyExpensiveChanges: false);
        //applyExpensiveChanges : 높은 컴퓨터 자원을 소모하는 환경 변화를 할 것인가?
        //URP 에셋을 런 타임에서 바꾸는 매소드

        Managers.Instance.Data.SaveSetting();
    }
}
