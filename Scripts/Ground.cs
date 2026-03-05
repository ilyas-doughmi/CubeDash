using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [Header("References")]
    public GameObject Scene; 
    public GameObject player; 

    [Header("Spawning Settings")]
    [Tooltip("How many pieces of the scene to keep ahead of the player.")]
    public int initialScenesToSpawn = 5;
    
    [Tooltip("The offset direction and distance to spawn the next scene piece. E.g. (30, 0, 0) for X-axis movement.")]
    public Vector3 spawnOffset = new Vector3(30f, 0, 0); 
    
    [Tooltip("How far behind the player an old scene piece must be before it is removed.")]
    public float despawnDistanceBehindPlayer = 15f; 

    private List<GameObject> activeScenes = new List<GameObject>();
    private Vector3 nextSpawnPosition;
    private Vector3 moveDirection;

    void Start()
    {
        nextSpawnPosition = transform.position;
        moveDirection = spawnOffset.normalized;

        if (spawnOffset.magnitude == 0)
        {
            Debug.LogWarning("Spawn Offset is zero! Please set a valid spawn offset in the Inspector.");
            return;
        }

        for (int i = 0; i < initialScenesToSpawn; i++)
        {
            SpawnScene();
        }
    }

    void Update()
    {
        if (player == null || Scene == null) return;

        Vector3 playerToNextSpawn = nextSpawnPosition - player.transform.position;
        float distanceToNextSpawn = Vector3.Dot(playerToNextSpawn, moveDirection);

        if (distanceToNextSpawn < spawnOffset.magnitude * (initialScenesToSpawn - 1))
        {
            SpawnScene();
        }

        if (activeScenes.Count > 0)
        {
            GameObject oldestScene = activeScenes[0];
            Vector3 oldestSceneToPlayer = player.transform.position - oldestScene.transform.position;
            float distanceBehind = Vector3.Dot(oldestSceneToPlayer, moveDirection);

            if (distanceBehind > despawnDistanceBehindPlayer)
            {
                activeScenes.RemoveAt(0);
                Destroy(oldestScene);
            }
        }
    }

    private void SpawnScene()
    {
        GameObject newScene = Instantiate(Scene, nextSpawnPosition, transform.rotation);
        newScene.transform.parent = this.transform; 
        
        activeScenes.Add(newScene);
        nextSpawnPosition += spawnOffset;
    }
}
