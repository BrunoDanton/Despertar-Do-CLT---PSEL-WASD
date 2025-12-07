using UnityEngine;
using System.Collections.Generic;

public class AddObstaclesV2 : MonoBehaviour
{
    // Cache de lista para evitar alocação por frame/chamada
    private List<int> availableRoads = new List<int>();

    public void Create(GameObject[] obstaclePrefabs, float[] roads, float roadLength, Vector2 obstacleStartPosition, GameObject[] jumpablePrefabs)
    {
        // Sonda uma única vez se a lista de puláveis é válida
        bool hasJumpables = jumpablePrefabs != null && jumpablePrefabs.Length > 0;

        // Otimização: A quantidade máxima que pode ter obstáculos. 
        // Se houver puláveis, todas as pistas (roads.Length) podem ser preenchidas.
        // Se não houver, deve sobrar uma pista livre (roads.Length - 1).
        int limitPerLine = hasJumpables ? roads.Length : roads.Length - 1;

        float currentZ = obstacleStartPosition.x;
        int maxObstacleSections = (int)(roadLength / obstacleStartPosition.y);

        for (int k = 0; k < maxObstacleSections; k++)
        {
            // Otimização: Reutiliza a lista e limpa/preenche
            availableRoads.Clear(); 
            for (int i = 0; i < roads.Length; i++) availableRoads.Add(i);

            // Sorteia quantos obstáculos teremos
            int quantityToSpawn = Random.Range(1, limitPerLine + 1);
            
            // Se vamos encher todas as pistas e temos puláveis, precisamos garantir um pulável
            bool mustHaveJumpable = quantityToSpawn == roads.Length && hasJumpables;
            bool hasAtLeastOneJumpable = false;

            for (int spawnedObjects = 0; spawnedObjects < quantityToSpawn; spawnedObjects++)
            {
                if (availableRoads.Count == 0) break;

                // Escolhe e remove a pista
                int roadIndex = Random.Range(0, availableRoads.Count);
                int selectedRoad = availableRoads[roadIndex];
                availableRoads.RemoveAt(roadIndex);

                Vector3 position = new Vector3(roads[selectedRoad], 3, transform.position.z + currentZ);
                GameObject prefab;

                // Garante um pulável se for o último slot e for obrigatório
                bool forceJumpableNow = mustHaveJumpable && !hasAtLeastOneJumpable && (spawnedObjects == quantityToSpawn - 1);

                // Lógica de spawn: Força pulável OU sorteia 50% de chance
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
                
                // Adiciona e configura os componentes
                obstacle.AddComponent<RoadMovement>();
                obstacle.AddComponent<DestroyObstacle>();
                obstacle.AddComponent<DetectObjectRoad>();
                obstacle.GetComponent<DetectObjectRoad>().actualLane = selectedRoad - 1;
                // Acessa o Collider diretamente em vez de GetComponent<>
                if (obstacle.TryGetComponent<BoxCollider>(out BoxCollider boxCollider))
                {
                    boxCollider.isTrigger = true;
                }
                else
                {
                    // Se não tiver, adiciona um e o torna trigger (mais robusto)
                    obstacle.AddComponent<BoxCollider>().isTrigger = true;
                }
            }

            currentZ += obstacleStartPosition.y;
            if (currentZ > roadLength) break;
        }
    }
}
