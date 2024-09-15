using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_SaveLoad_Ui : MonoBehaviour
{
    private Button save_Btn;
    private Button load_Btn;

    private void Start()
    {
        save_Btn = transform.GetChild(0).GetComponent<Button>();
        load_Btn = transform.GetChild(1).GetComponent<Button>();

        save_Btn.onClick.AddListener(OnClickSave);
        load_Btn.onClick.AddListener(OnClickLoad);
    }

    public void OnClickSave()
    {
        if(DataManager.instance.SaveGame())
        Debug.Log("���� �Ϸ�");

        else
        Debug.Log("���� ����");
    }
    public void OnClickLoad()
    {
        DataManager.instance.LoadGame();
    }
}
