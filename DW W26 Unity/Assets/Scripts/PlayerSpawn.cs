using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawn : MonoBehaviour
{
    [field: SerializeField] public Transform[] SpawnPoints { get; private set; }
    [field: SerializeField] public Color[] PlayerColors { get; private set; }
    public int PlayerCount { get; private set; }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int maxPlayerCount = Mathf.Min(SpawnPoints.Length, PlayerColors.Length);
        if (maxPlayerCount < 1)
        {
            Debug.Log("Spawn points or player colors not assigned.");
            return;
        }

        if (PlayerCount >= maxPlayerCount)
        {
            Destroy(playerInput.gameObject);
            return;
        }

        Transform spawnPoint = SpawnPoints[PlayerCount];

        playerInput.transform.position = spawnPoint.position;
        playerInput.transform.rotation = spawnPoint.rotation;

        Color color = PlayerColors[PlayerCount];

        PlayerCount++;

        PlayerController playerController = playerInput.GetComponent<PlayerController>();
        playerController.AssignPlayerInputDevice(playerInput);
        playerController.AssignPlayerNumber(PlayerCount);
        playerController.AssignColor(color);
        playerController.SetSpawnPoint(spawnPoint);
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player left...");
    }
}
