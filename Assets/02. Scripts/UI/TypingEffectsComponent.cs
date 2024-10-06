using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame
{
    enum EffectType
    {
        Duration, Interval
    }

    public class TypingEffectsComponent : MonoBehaviour
    {
        public UnityEvent OnTypingStartEvent;
        public UnityEvent OnTypingEndEvent;
        [Header("References")]
        [SerializeField] TextMeshProUGUI UI;
        [Header("Options")]
        [SerializeField] EffectType Type;
        [SerializeField] float Duration;
        [SerializeField] float FixedInterval;

        public void StartEffects()
        {
            var text = UI.text;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            StopAllCoroutines();
            StartCoroutine(TypringText(text));
        }

        IEnumerator TypringText(string text)
        {
            OnTypingStartEvent.Invoke();
            var delay = Type is EffectType.Duration ? Duration / text.Length : FixedInterval;
            for (var i = 1; i <= text.Length; i++)
            {
                UI.text = text.Substring(0, i);
                yield return new WaitForSeconds(delay);
            }
            OnTypingEndEvent.Invoke();
        }

    }
}

