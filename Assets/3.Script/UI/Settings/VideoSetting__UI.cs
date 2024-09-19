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
        List<string> _resolutionList = new List<string>();

        for (int i = 0; i < _resolutionArr.Length; i++)
        {
            _resolutionList.Add(_resolutionArr[i].width + "X" + _resolutionArr[i].height.ToString());
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

        Debug.Log("ȭ�� �ػ� �ε��� : " + saveData.resolutionIndex);
        AssignShownQuality();
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
                Debug.LogWarning("�Ҵ���� ���� �׷��� ����Ƽ index");
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
        saveData.isFullScreen = fullScreenBtn.isOn;
    }

    public void OnClickResolution()
    {

        saveData.resolutionIndex = resolutionDropdown.value;
    }

    public void OnClickQuality_Btn(int qualityIndex)
    {
        if(saveData != null)
        saveData.urpAssetsIndex = qualityIndex;
    }

    public void OnApply_Btn()
    {
        Screen.SetResolution(Screen.resolutions[saveData.resolutionIndex].width,
                             Screen.resolutions[saveData.resolutionIndex].height,
                             saveData.isFullScreen);

        QualitySettings.SetQualityLevel(index: saveData.urpAssetsIndex,
                                        applyExpensiveChanges: false);
        //applyExpensiveChanges : ���� ��ǻ�� �ڿ��� �Ҹ��ϴ� ȯ�� ��ȭ�� �� ���ΰ�?
        //URP ������ �� Ÿ�ӿ��� �ٲٴ� �żҵ�

        Managers.Instance.Data.SaveSetting();
    }
}
