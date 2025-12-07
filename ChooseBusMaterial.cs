using UnityEngine;

public class ChooseBusMaterial : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public Material[] materials;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (materials.Length != 0)
            meshRenderer.material = materials[Random.Range(0, materials.Length)];
    }
}
