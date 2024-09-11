using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadAndSave : MonoBehaviour
{

}

[Serializable]
public class ItemLoad
{
    public int id;
    public int amount;
    public int slotIndex;

    public ItemLoad(int _id, int _amount, int _slotIndex)
    {
        id = _id;
        amount = _amount;
        slotIndex = _slotIndex;
    }
}