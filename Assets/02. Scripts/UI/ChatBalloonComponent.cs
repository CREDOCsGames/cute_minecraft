using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class ChatBalloonComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onShowEvent;
        [SerializeField] private UnityEvent _onHideEvent;

        [SerializeField] private TextMeshProUGUI _ui;
        [SerializeField] private SpriteRenderer _e;
        [SerializeField] private SpriteRenderer _w;
        [SerializeField] private SpriteRenderer _s;
        [SerializeField] private SpriteRenderer _n;
        [SerializeField] private SpriteRenderer _c;
        [SerializeField] private SpriteRenderer _ne;
        [SerializeField] private SpriteRenderer _nw;
        [SerializeField] private SpriteRenderer _se;
        [SerializeField] private SpriteRenderer _sw;
        [SerializeField] private SpriteRenderer _arrow;

        [SerializeField] private float _heightUnit = 1;
        private float _widthUnit = 3;
        [Range(1f, 500f)][SerializeField] private float _interval = 23;
        public bool IsShow { get; private set; }

        public void Show(bool bShow)
        {
            IsShow = bShow;
            _ui.gameObject.SetActive(IsShow);
            _e.gameObject.SetActive(IsShow);
            _w.gameObject.SetActive(IsShow);
            _s.gameObject.SetActive(IsShow);
            _n.gameObject.SetActive(IsShow);
            _c.gameObject.SetActive(IsShow);
            _ne.gameObject.SetActive(IsShow);
            _nw.gameObject.SetActive(IsShow);
            _se.gameObject.SetActive(IsShow);
            _sw.gameObject.SetActive(IsShow);
            _arrow.gameObject.SetActive(IsShow);

            if (!bShow)
            {
                Clear();
                _onHideEvent.Invoke();
            }
            else
            {
                _onShowEvent.Invoke();
            }
        }

        public void SetText(string text)
        {
            _ui.text = text;
        }

        private void Clear()
        {
            _widthUnit = 3;
            PlaceParts();
            _ui.text = "";
        }

        private void PlaceParts()
        {
            var transform1 = _n.transform;
            transform1.localScale = Vector3.up + Vector3.forward + Vector3.right * _widthUnit;
            var sprite = _c.sprite;
            var localPosition = _heightUnit * sprite.bounds.extents.y * Vector3.up;
            transform1.localPosition = localPosition;

            var transform2 = _s.transform;
            transform2.localScale = transform1.localScale;
            var localPosition1 = -localPosition;
            transform2.localPosition = localPosition1;


            var transform3 = _e.transform;
            transform3.localScale = Vector3.right + Vector3.forward + Vector3.up * _heightUnit;
            var position = _widthUnit * sprite.bounds.extents.x * Vector3.right;
            transform3.localPosition = position;

            var transform4 = _w.transform;
            transform4.localScale = transform3.localScale;
            var position1 = -position;
            transform4.localPosition = position1;

            _c.transform.localScale = Vector3.right * _widthUnit + Vector3.up * _heightUnit + Vector3.forward;
            _arrow.transform.localPosition = localPosition1;

            _ne.transform.localPosition = localPosition + position;
            _nw.transform.localPosition = localPosition + position1;
            _se.transform.localPosition = localPosition1 + position;
            _sw.transform.localPosition = localPosition1 + position1;
        }

        private void Awake()
        {
            Show(false);
        }

        private void Update()
        {
            _widthUnit = 1 + _ui.rectTransform.sizeDelta.x / _interval;
            if (IsShow)
            {
                PlaceParts();
            }
        }
    }
}