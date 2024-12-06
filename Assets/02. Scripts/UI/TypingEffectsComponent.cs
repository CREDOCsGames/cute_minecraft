using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    enum EffectType
    {
        Duration,
        Interval
    }

    public class TypingEffectsComponent : MonoBehaviour
    {
        public UnityEvent OnTypingStartEvent;
        public UnityEvent OnTypingEndEvent;

        [Header("References")]
        [SerializeField] private TextMeshProUGUI _ui;

        [Header("Options")]
        [SerializeField] private EffectType _type;
        [SerializeField] private float _duration;
        [SerializeField] private float _fixedInterval;

        public void StartEffects()
        {
            var text = _ui.text;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            StopAllCoroutines();
            StartCoroutine(TypringText(text));
        }

        private IEnumerator TypringText(string text)
        {
            OnTypingStartEvent.Invoke();
            var delay = _type is EffectType.Duration ? _duration / text.Length : _fixedInterval;
            for (var i = 1; i <= text.Length; i++)
            {
                _ui.text = text.Substring(0, i);
                yield return new WaitForSeconds(delay);
            }

            OnTypingEndEvent.Invoke();
        }
    }
}