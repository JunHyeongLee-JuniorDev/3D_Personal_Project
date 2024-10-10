using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ResourcesList", menuName = "Inventory/ResourcesList")]
public class ResourcesList : ScriptableObject
{
    [SerializeField]
    private ItemResouce[] _list;
    public ItemResouce[] _List => _list;
}