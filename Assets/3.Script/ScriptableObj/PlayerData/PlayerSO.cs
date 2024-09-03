using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Characters/PlayerData")]
public class PlayerSO : ScriptableObject
{
    [field: SerializeField]
    public PlayerGroundData GroundedData {get ; private set;}
}
