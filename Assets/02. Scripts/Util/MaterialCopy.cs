using UnityEngine;

public class MaterialCopy : MonoBehaviour
{
    void Awake()
    {
        var renderer = GetComponent<Renderer>();

        var originalMaterials = renderer.materials;
        var copiedMaterials = new Material[originalMaterials.Length];

        for (int i = 0; i < originalMaterials.Length; i++)
        {
            var originalMaterial = originalMaterials[i];
            var copiedMaterial = new Material(originalMaterial);

            copiedMaterials[i] = copiedMaterial;
        }

        renderer.materials = copiedMaterials;
    }
}
