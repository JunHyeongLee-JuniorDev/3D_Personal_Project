using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class VideoSetting_UI : MonoBehaviour
{
    [SerializeField]
    private ToggleSwitch_UI fullScreenBtn;


    [SerializeField]
    private TMP_Dropdown resolutionDropdown;

    private void Start()
    {
        Resolution[] _resolutionArr = Screen.resolutions;
        List<string> _resolutionList = new List<string>();

        for (int i = 0; i < _resolutionArr.Length; i++)
        {
            _resolutionList.Add(_resolutionArr[i].width + "X" + _resolutionArr[i].height.ToString());
        }


        resolutionDropdown.AddOptions(_resolutionList);
    }

    public void OnClickResolution()
    {
                Screen.SetResolution(Screen.resolutions[resolutionDropdown.value].width,
                                     Screen.resolutions[resolutionDropdown.value].height,
                                     fullScreenBtn.isOn);
    }
}
