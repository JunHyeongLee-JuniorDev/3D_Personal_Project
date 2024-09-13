using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerSetDisplayer : DynamicInventoryDisplay
{
    protected override void Start()
    {
        viewerTransform = transform.GetChild(1).GetChild(0).GetChild(0);
        base.Start();
    }
}
