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
        // Verifica se já foi ativado ou se não é o Player
        if (hasTriggered || !other.gameObject.CompareTag("Player")) return;
        
        hasTriggered = true;
        
        if (buildOrDestroy)
        {
            // Cria a estrada e a configura
            Vector3 novaPosicao = startPosition; 
            GameObject road = Instantiate(roadPrefab, novaPosicao, Quaternion.identity);
            float roadLength = 100f;

            // Adiciona e armazena os componentes de uma vez
            AddObstaclesV2 addObstacles = road.AddComponent<AddObstaclesV2>();
            AddCollectables addCollectables = road.AddComponent<AddCollectables>();
            
            addCollectables.Create(collectables, roads, roadLength, collectableStartPosition);
            addObstacles.Create(obstaclePrefabs, roads, roadLength, obstacleStartPosition, jumpablePrefabs); 
        }
        else
        {
            // Destroi o objeto pai, que é a estrada anterior
            Destroy(transform.parent.gameObject);
        }
    }
}
