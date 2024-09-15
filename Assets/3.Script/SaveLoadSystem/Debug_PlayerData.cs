using UnityEngine;

public class Debug_PlayerData : MonoBehaviour
{
    float currentHealth = 100.0f;
    PlayerData playerSaveData = new PlayerData();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            playerSaveData.playerPosition = transform.position;
            playerSaveData.playerRotation = transform.rotation;
            playerSaveData.currentHealth = currentHealth;

            DataManager.instance.currentSaveData.savePlayerData = playerSaveData;
            DataManager.instance.SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            DataManager.instance.LoadGame();

            playerSaveData = DataManager.instance.currentSaveData.savePlayerData;
            transform.position = playerSaveData.playerPosition;
            transform.rotation = playerSaveData.playerRotation;
            currentHealth = playerSaveData.currentHealth;
        }
    }
}
