// TriggerCollision.cs
using UnityEngine;

public class TriggerCollision : MonoBehaviour
{
    public GameObject roadPrefab; 
    public GameObject[] obstaclePrefabs;
    public GameObject[] jumpablePrefabs;
    public Collectable[] collectables;

    public Vector3 startPosition = new Vector3 (0, 0, 0);
    public float[] roads;
    public Vector2 obstacleStartPosition;
    public Vector2 collectableStartPosition;
    public bool buildOrDestroy;
    private bool hasTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        
        if (other.gameObject.CompareTag("Player"))
        {
            hasTriggered = true;
            if (buildOrDestroy)
            {
                Vector3 novaPosicao = startPosition; 
                GameObject road = Instantiate(roadPrefab, novaPosicao, Quaternion.identity);
                float roadLength = 100f;

                AddObstaclesV2 addObstacles = road.AddComponent<AddObstaclesV2>();

                AddCollectables addCollectables = road.AddComponent<AddCollectables>();

                if (addCollectables == null)
                {
                    addCollectables = road.AddComponent<AddCollectables>();
                }

                if (addObstacles == null) 
                {
                    addObstacles = road.AddComponent<AddObstaclesV2>();
                }

                addCollectables.Create(collectables, roads, roadLength, collectableStartPosition);
                addObstacles.Create(obstaclePrefabs, roads, roadLength, obstacleStartPosition, jumpablePrefabs); 
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
