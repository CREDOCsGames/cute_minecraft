using UnityEngine;

public class MaterialCopyComponent : MonoBehaviour
{
    void Awake()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            return;
        }
        MaterialUtil.CopyMaterials(renderer);
    }

}


public static class MaterialUtil
{
    public static void CopyMaterials(Renderer renderer)
    {
        var originalMaterials = renderer.sharedMaterials;
        var copiedMaterials = new Material[originalMaterials.Length];

        for (int i = 0; i < originalMaterials.Length; i++)
        {
            var originalMaterial = originalMaterials[i];
            var copiedMaterial = GameObject.Instantiate(originalMaterial);
            copiedMaterial.name = originalMaterial.name;

            copiedMaterials[i] = copiedMaterial;
        }

        renderer.materials = copiedMaterials;
    }
}