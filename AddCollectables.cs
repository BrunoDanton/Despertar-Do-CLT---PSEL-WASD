using UnityEngine;
using System.Collections.Generic;

public class AddCollectables : MonoBehaviour
{
    public void Create(Collectable[] collectables, float[] roads, float roadLength, Vector2 collectableStartPosition)
    {
        int limitPerLine = roads.Length;

        float currentZ = collectableStartPosition.x;
        int maxCollectablesSections = (int)(roadLength / collectableStartPosition.y);

        for (int k = 0; k < maxCollectablesSections; k++)
        {
            List<int> availableRoads = new List<int>();
            for (int i = 0; i < roads.Length; i++) availableRoads.Add(i);

            int quantityToSpawn = Random.Range(1, limitPerLine + 1);
            
            for (int spawnedObjects = 0; spawnedObjects < quantityToSpawn; spawnedObjects++)
            {
                if (availableRoads.Count == 0) break;

                int roadIndex = Random.Range(0, availableRoads.Count);
                int selectedRoad = availableRoads[roadIndex];
                availableRoads.RemoveAt(roadIndex);

                Vector3 position = new Vector3(roads[selectedRoad], 4, transform.position.z + currentZ);

                Collectable prefab;

                prefab = collectables[Random.Range(0, collectables.Length)];   

                if (Random.Range(0f, 1f) < prefab.AppearChance)
                {
                    spawnedObjects--;
                    continue;
                }             

                GameObject collectable = Instantiate(prefab.body, position, Quaternion.identity);

                collectable.AddComponent<RoadMovement>();
                collectable.AddComponent<DestroyObstacle>();
                collectable.AddComponent<DestroyCollectable>();
                collectable.GetComponent<BoxCollider>().isTrigger = true;
            }

            currentZ += collectableStartPosition.y;
            if (currentZ > roadLength) break;
        }
    }
}
