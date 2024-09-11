using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Util
{
    [CreateAssetMenu(menuName = "Custom/Util/Colorize")]
    public class Colorize : UniqueScriptableObject<Colorize>
    {
        public void Invoke(List<MeshRenderer> materials, Color color)
        {
            LateColor(materials, color);
        }

        void Fixed(List<MeshRenderer> materials, Color color)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].material.color = color;
            }
        }

        void Lerp(List<MeshRenderer> materials, Color color)
        {
            CoroutineRunner.Instance.StartCoroutine(Coroutine(materials, color));
        }

        void LateColor(List<MeshRenderer> materials, Color color)
        {
            var coroutineRunner = CoroutineRunner.Instance;
            if(coroutineRunner == null)
            {
                Fixed(materials, color);
                return;
            }
            CoroutineRunner.Instance.StartCoroutine(Late(materials, color));
        }

        IEnumerator Late(List<MeshRenderer> materials, Color color)
        {
            yield return new WaitForSeconds(0.5f);
            Fixed(materials, color);
        }

        IEnumerator Coroutine(List<MeshRenderer> materials, Color color)
        {
            float duration = 1f;
            float time = 0;
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

                for (int i = 0; i < colors.Count; i++)
                {
                    materials[i].material.color = Vector4.Lerp(colors[i], color, time / duration);
                }
                time += Time.deltaTime;
                yield return null;
            }

        }

    }
}
