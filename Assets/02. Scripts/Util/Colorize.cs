using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    [CreateAssetMenu(menuName = "Custom/Util/Colorize")]
    public class Colorize : UniqueScriptableObject<Colorize>
    {
        public void Invoke(List<MeshRenderer> materials, Color color)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Fixed(materials, color);
                return;
            }
#endif
            LateColor(materials, color);
        }

        private static void Fixed(List<MeshRenderer> materials, Color color)
        {
            if (!Application.isPlaying)
            {
                materials.ForEach(MaterialHelper.CopyMaterials);
            }

            for (var i = 0; i < materials.Count; i++)
            {
                var c = materials[i].sharedMaterial.color;
                c.r = color.r;
                c.g = color.g;
                c.b = color.b;
                materials[i].sharedMaterial.color = c;
            }
        }

        private void Lerp(List<MeshRenderer> materials, Color color)
        {
            CoroutineRunner.Instance.StartCoroutine(Coroutine(materials, color));
        }

        private void LateColor(List<MeshRenderer> materials, Color color)
        {
            var coroutineRunner = CoroutineRunner.Instance;
            if (coroutineRunner == null)
            {
                Fixed(materials, color);
                return;
            }

            CoroutineRunner.Instance.StartCoroutine(Late(materials, color));
        }

        private static IEnumerator Late(List<MeshRenderer> materials, Color color)
        {
            yield return new WaitForSeconds(0.5f);
            Fixed(materials, color);
        }

        private static IEnumerator Coroutine(List<MeshRenderer> materials, Color color)
        {
            const float duration = 1f;
            var time = 0f;
            List<Color> colors = new();
            foreach (var material in materials)
            {
                colors.Add(material.material.color);
            }

            while (true)
            {
                if (time >= duration)
                {
                    break;
                }

                for (var i = 0; i < colors.Count; i++)
                {
                    materials[i].material.color = Vector4.Lerp(colors[i], color, time / duration);
                }

                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}