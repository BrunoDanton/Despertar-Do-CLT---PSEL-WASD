using UnityEngine;
using System.Collections.Generic; 
using System.Linq; // Para usar .Sum() e .FirstOrDefault() (opcional, mas mais limpo)

public class JumpableCollectableSpawner : MonoBehaviour
{
    [System.Serializable]
    public class CollectableData
    {
        public GameObject body;
        [Range(0f, 1f)] 
        public float AppearChance;
    }

    [Header("Configuração do Coletável")]
    public List<CollectableData> possibleCollectables = new List<CollectableData>();
    
    public float verticalOffset = 1.5f; 
    public float maxRandomValue = 2f;

    void Start()
    {
        TrySpawnCollectable();
    }

    private void TrySpawnCollectable()
    {
        if (possibleCollectables.Count == 0) return;

        // 1. Decidir qual coletável deve spawnar
        GameObject collectableToSpawn = null;
        float totalChance = possibleCollectables.Sum(data => data.AppearChance);

        if (totalChance > 0f)
        {
            float randomValue = Random.Range(0f, maxRandomValue); 
            float currentChanceSum = 0f;

            foreach (var data in possibleCollectables)
            {
                currentChanceSum += data.AppearChance;

                // Seleção por faixa de chance
                if (randomValue <= currentChanceSum)
                {
                    collectableToSpawn = data.body;
                    break; 
                }
            }
        }
        
        // 2. Se um coletável foi selecionado, spawna e configura
        if (collectableToSpawn != null)
        {
            // Posição de spawn: posição do obstáculo + deslocamento vertical
            Vector3 spawnPosition = transform.position + new Vector3(0f, verticalOffset, 0f);

            // Instancia o coletável
            GameObject collectable = Instantiate(collectableToSpawn, spawnPosition, Quaternion.identity);

            // Adiciona e configura os Componentes
            collectable.AddComponent<DestroyObstacle>(); 
            collectable.AddComponent<DestroyCollectable>(); 
            collectable.AddComponent<RoadMovement>();
            collectable.AddComponent<RotateObject>();

            // 3. Configura o Collider
            BoxCollider boxCollider = collectable.GetComponent<BoxCollider>();
            if (boxCollider == null)
            {
                boxCollider = collectable.AddComponent<BoxCollider>();
            }
            
            boxCollider.isTrigger = true;
        }
    }
}