using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDiplayer : DynamicInventoryDisplay
{
    /*
     * ���� ���� �� ��������� ������!
     */
    protected override void Start()
    {
        viewerTransform = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0);
        base.Start();
    }
}
