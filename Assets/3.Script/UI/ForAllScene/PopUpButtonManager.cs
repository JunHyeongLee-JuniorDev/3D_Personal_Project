using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PopUpButtonManager : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons;
    private Slider slider;

    [SerializeField]
    private List<GameObject> popupMenus = new List<GameObject>();

    [SerializeField] private Navigation.Mode navigationMode;

    private int menuIndex;

    private void Awake()
    {
        for (int i = 0; i < popupMenus.Count; i++)
        {
            popupMenus[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        menuIndex = 0;
        popupMenus[0].SetActive(true);
        SetSelectedDefaultBtn();
    }

    public void OnClickMenu_Btn(int index)
    {
        PlayClickSound();
        popupMenus[menuIndex].SetActive(false);
        menuIndex = index;
        popupMenus[index].SetActive(true);

        foreach (var btn in buttons)
        {
            switch (navigationMode)
            { 
                case Navigation.Mode.Vertical:
                    Navigation newNav = new Navigation();
                    newNav.mode = Navigation.Mode.Explicit;
                    newNav.selectOnRight = popupMenus[index].transform.GetChild(0).Find("FirstBtn").GetComponent<Selectable>();
                    newNav.selectOnUp = btn.navigation.selectOnUp;
                    newNav.selectOnDown = btn.navigation.selectOnDown;
                    btn.navigation = newNav;
                    break;
                case Navigation.Mode.Horizontal:
                    break;
            }
        }
    }

    public void PlayClickSound()
    {
        Managers.Instance.Sound.Play("UI/MenuClick");
    }

    public void SetSelectedDefaultBtn()
    {
        buttons[menuIndex].Select();
    }

    private void OnDisable()
    {
        for (int i = 0; i < popupMenus.Count; i++)
        {
            popupMenus[i].SetActive(false);
        }
    }
}
