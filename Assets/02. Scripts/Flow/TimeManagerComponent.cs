using UnityEngine;

namespace Flow
{
    public class TimeManagerComponent : MonoBehaviour
    {
        public float _timeScale;

        private void Update()
        {
            Time.timeScale = _timeScale;
        }

        public void SetScale(float t)
        {
            _timeScale = t;
            Time.timeScale = _timeScale;
        }

        private void OnDestroy()
        {
            SetScale(1f);
        }
    }
}