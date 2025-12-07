using UnityEngine;
using System.Collections.Generic;

public class AddCollectables : MonoBehaviour
{
    // Cache de lista para evitar alocação por frame/chamada
    private List<int> availableRoads = new List<int>();

    public void Create(Collectable[] collectables, float[] roads, float roadLength, Vector2 collectableStartPosition)
    {
        int limitPerLine = roads.Length;

        float currentZ = collectableStartPosition.x;
        int maxCollectablesSections = (int)(roadLength / collectableStartPosition.y);

        for (int k = 0; k < maxCollectablesSections; k++)
        {
            // Otimização: Reutiliza a lista e limpa/preenche
            availableRoads.Clear(); 
            for (int i = 0; i < roads.Length; i++) availableRoads.Add(i);

            int quantityToSpawn = Random.Range(1, limitPerLine + 1);
            
            for (int spawnedObjects = 0; spawnedObjects < quantityToSpawn; spawnedObjects++)
            {
                if (availableRoads.Count == 0) break;

                // Escolhe e remove a pista
                int roadIndex = Random.Range(0, availableRoads.Count);
                int selectedRoad = availableRoads[roadIndex];
                availableRoads.RemoveAt(roadIndex);

                Vector3 position = new Vector3(roads[selectedRoad], 4, transform.position.z + currentZ);

                Collectable selectedCollectable = collectables[Random.Range(0, collectables.Length)];   

                // Lógica de chance otimizada: Aumenta o contador (spawnedObjects++) se o item for criado
                if (Random.Range(0f, 1f) > selectedCollectable.AppearChance)
                {
                    // Se a chance de nãp aparecer é maior, continua para a próxima iteração
                    continue;
                }             

                // O item será criado
                GameObject collectable = Instantiate(selectedCollectable.body, position, Quaternion.identity);

                // Adiciona e configura os componentes
                collectable.AddComponent<RoadMovement>();
                collectable.AddComponent<RotateObject>();
                collectable.AddComponent<DestroyObstacle>();
                collectable.AddComponent<DestroyCollectable>();
                
                // Acessa o Collider diretamente em vez de GetComponent<>
                if (collectable.TryGetComponent<BoxCollider>(out BoxCollider boxCollider))
                {
                    boxCollider.isTrigger = true;
                }
                else
                {
                    // Se não tiver, adiciona um e o torna trigger (mais robusto)
                    collectable.AddComponent<BoxCollider>().isTrigger = true;
                }
            }

            currentZ += collectableStartPosition.y;
            if (currentZ > roadLength) break;
        }
    }
}
