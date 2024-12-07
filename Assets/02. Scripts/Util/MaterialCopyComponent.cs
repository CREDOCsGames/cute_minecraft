using UnityEngine;

namespace Util
{
    public class MaterialCopyComponent : MonoBehaviour
    {
        private void Awake()
        {
            if (!TryGetComponent<Renderer>(out var renderer))
            {
                return;
            }

            MaterialHelper.CopyMaterials(renderer);
        }
    }


    public static class MaterialHelper
    {
        public static void CopyMaterials(Renderer renderer)
        {
            var originalMaterials = renderer.sharedMaterials;
            var copiedMaterials = new Material[originalMaterials.Length];

            for (var i = 0; i < originalMaterials.Length; i++)
            {
                var originalMaterial = originalMaterials[i];
                var copiedMaterial = Object.Instantiate(originalMaterial);
                copiedMaterial.name = originalMaterial.name;

                copiedMaterials[i] = copiedMaterial;
            }

            renderer.materials = copiedMaterials;
        }
    }
}