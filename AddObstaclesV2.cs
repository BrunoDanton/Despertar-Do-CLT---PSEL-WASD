using UnityEngine;
using System.Collections.Generic;

public class AddObstaclesV2 : MonoBehaviour
{
    public void Create(GameObject[] obstaclePrefabs, float[] roads, float roadLength, Vector2 obstacleStartPosition, GameObject[] jumpablePrefabs)
    {
        // Verifica se existem obstáculos puláveis configurados
        bool hasJumpables = jumpablePrefabs != null && jumpablePrefabs.Length > 0;

        // Se tivermos puláveis, podemos encher todas as pistas (Length). 
        // Se não tivermos, temos que deixar uma livre (Length - 1).
        int limitPerLine = hasJumpables ? roads.Length : roads.Length - 1;

        float currentZ = obstacleStartPosition.x;
        int maxObstacleSections = (int)(roadLength / obstacleStartPosition.y);

        for (int k = 0; k < maxObstacleSections; k++)
        {
            List<int> availableRoads = new List<int>();
            for (int i = 0; i < roads.Length; i++) availableRoads.Add(i);

            // Sorteia quantos obstáculos teremos (pode ser 1, 2 ou 3)
            int quantityToSpawn = Random.Range(1, limitPerLine + 1);
            
            // Flag: Se vamos encher todas as pistas, precisamos garantir um pulável
            bool mustHaveJumpable = quantityToSpawn == roads.Length;
            bool hasAtLeastOneJumpable = false;

            for (int spawnedObjects = 0; spawnedObjects < quantityToSpawn; spawnedObjects++)
            {
                if (availableRoads.Count == 0) break;

                int roadIndex = Random.Range(0, availableRoads.Count);
                int selectedRoad = availableRoads[roadIndex];
                availableRoads.RemoveAt(roadIndex);

                Vector3 position = new Vector3(roads[selectedRoad], 3, transform.position.z + currentZ);
                GameObject prefab;

                bool forceJumpableNow = mustHaveJumpable && !hasAtLeastOneJumpable && (spawnedObjects == quantityToSpawn - 1);

                if (hasJumpables && (forceJumpableNow || Random.Range(0, 2) == 1))
                {
                    // Cria Pulável
                    prefab = jumpablePrefabs[Random.Range(0, jumpablePrefabs.Length)];
                    hasAtLeastOneJumpable = true;
                }
                else
                {
                    // Cria Normal 
                    prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                }

                GameObject obstacle = Instantiate(prefab, position, Quaternion.identity);

                obstacle.AddComponent<RoadMovement>();
                obstacle.AddComponent<DestroyObstacle>();
                obstacle.GetComponent<BoxCollider>().isTrigger = true;
            }

            currentZ += obstacleStartPosition.y;
            if (currentZ > roadLength) break;
        }
    }
}