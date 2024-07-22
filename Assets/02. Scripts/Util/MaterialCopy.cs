using UnityEngine;

public class MaterialCopy : MonoBehaviour
{
    void Awake()
    {
        var renderer = GetComponent<Renderer>();

        Material[] originalMaterials = renderer.materials;
        Material[] copiedMaterials = new Material[originalMaterials.Length];

        for (int i = 0; i < originalMaterials.Length; i++)
        {
            Material originalMaterial = originalMaterials[i];
            Material copiedMaterial = new Material(originalMaterial);

            copiedMaterials[i] = copiedMaterial;
        }

        renderer.materials = copiedMaterials;
    }
}
