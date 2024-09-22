using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class ItemDataBase
{
    public ItemList itemDB {  get; private set; }
    public ResourcesList itemResouces { get; private set; }
    private readonly string itemJsonDBPath = "ItemDataBaseJson";
    private readonly string itemResourcesPath = "ResourcesList";

    public ItemDataBase()
    {
        if (LoadDBFromJson())
            Debug.Log("아이템 DB 불러오기 성공");

        else
            Debug.LogWarning("DB 불러오지 못함");
    }

    public bool LoadDBFromJson()
    {
        TextAsset DBfile = Resources.Load<TextAsset>(itemJsonDBPath);

        if (DBfile != null)
        {
            itemDB = JsonUtility.FromJson<ItemList>(DBfile.text);

            if(itemDB == null)
            return false;
        }

        itemResouces = Resources.Load<ResourcesList>(itemResourcesPath);
        return itemResouces != null;
    }

    public Sprite GetSprite(string displayName)
    {
        for (int i = 0; i < itemResouces._List.Length; i++)
        {
            if (itemResouces._List[i].itemName == displayName)
                return itemResouces._List[i].itemIcon;
        }
        return null;
    }

    public GameObject GetModelPrefab(string displayName)
    {
        for (int i = 0; i < itemResouces._List.Length; i++)
        {
            if (itemResouces._List[i].itemName == displayName)
                return itemResouces._List[i].itemModel;
        }
        return null;
    }

    public ItemData GetItemData(string displayName)
    {
        for (int i = 0; i < itemDB.itemList.Count; i++)
        {
            if (itemDB.itemList[i].displayName == displayName)
                return itemDB.itemList[i];
        }
        return null; 
    }
}

[Serializable]
public class ItemList
{
    public List<ItemData> itemList;
}
